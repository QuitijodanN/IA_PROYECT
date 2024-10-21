using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollRayCast : MonoBehaviour
{
    public float speed = 0.5f; // Velocidad de movimiento hacia adelante
    public bool seek = false;
    public float rayDistance = 100f; // Distancia del rayo
    public float rotationSpeed = 2f; // Velocidad de rotación suave
    public LayerMask targetLayer; // Para filtrar el raycast por capas (opcional)

    Transform target;
    Vector3 visionPoint;
    Vector3[] path;
    int targetIndex;
    bool near = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        // Iniciar el cambio de dirección aleatorio
        StartCoroutine(ChangeDirectionRoutine());
    }

    void Update()
    {
        if (seek && !near)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }
    }
    public void DirectionTarget()
    {
        path = new Vector3[0];
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }
    
    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            CastMultipleRays();
            // Cambiar la dirección de movimiento aleatoriamente
            ChangeDirection();
            // Esperar el tiempo definido antes de cambiar la dirección nuevamente

            yield return new WaitForSeconds(Random.Range(5,7));
        }
    }
    void CastMultipleRays()
    {
        float angleStep = (30 * 2) / 12; // Dividir el ángulo en partes iguales
        float currentAngle = -30; // Empezar en -30 grados

        for (int i = 0; i <= 12; i++)
        {
            // Calcular la rotación para cada rayo
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * transform.forward;

            // Lanzar el raycast
            Ray ray = new Ray(transform.position, rayDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // Verificar si el objeto con el que colisionó es el target predefinido
                if (hit.transform == target)
                {
                    seek = true;
                    return;
                }
                else
                {
                    seek = false;
                }
            }

            // Dibujar el raycast en la escena (solo visible en modo Scene)
            Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);

            // Incrementar el ángulo para el siguiente rayo
            currentAngle += angleStep;
        }
    }
    void ChangeDirection()
    {
        if (!seek)
        {
            PathRequestManager.RandomsRequestPath(transform.position, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (targetIndex < path.Length)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                // Aquí calculamos la dirección hacia el siguiente waypoint
                Vector3 direction = (currentWaypoint - transform.position).normalized;

                // Si existe una dirección válida, rotamos suavemente hacia ella
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                // Mover el objeto hacia el siguiente waypoint
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

                yield return null;
            }
        }        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform == target)
        {
            near = true;
            seek = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            near = false;
        }
    }
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
