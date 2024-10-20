using UnityEngine;
using System.Collections;


public class Unit : MonoBehaviour {

   
    public Transform target;
	public float speed = 2f;
	public GameObject waypoints;
	
	//Esto solo se usa si quieres hacer una Machine State en la que pasen cosas con tiempo
    public float tiempoSeguir = 5f;


    Animator animator;
    Vector3[] path;
	int targetIndex;


	Vector3[] patrolWaypoints;
	float originalX,originalZ;
	Quaternion originalRotation;

	//Atributos para saber si te pueden ver los enemigos
	float wallDistance;
	bool canSeeMe = true;
	

	void Start ()
	{
		animator = GetComponent<Animator>();
		originalX = transform.position.x;
		originalZ = transform.position.z;
		originalRotation = transform.rotation;

		patrolWaypoints = new Vector3[waypoints.transform.childCount];

        //Pasamos los gameobjects a waypoints
        for (int i = 0; i < patrolWaypoints.Length; i++) 
			patrolWaypoints[i] = waypoints.transform.GetChild(i).gameObject.transform.position;
        




    }


	
    private void OnTriggerEnter(Collider other)
    {
        //Si el enemigo te ve 
        if (other.gameObject.tag == "Player" && canSeeMe && !animator.GetBool("Detectar"))
		{
			Debug.Log("Detectado");
            //Esto activa la Machine State Detectado/Seguir y para patrulla

            animator.SetBool("Patrulla", false);
            animator.SetBool("Detectado", true);

		}
    }

    private void OnTriggerStay(Collider other)
    {
		//Comprobación de que no hay una pared que te oculte
        if(other.gameObject.tag == "Wall")
		{
           
            wallDistance = Vector3.Distance(other.gameObject.GetComponent<Transform>().position, transform.position);
			float playerD = Vector3.Distance(target.transform.position, transform.position);

			if(wallDistance < playerD) canSeeMe = false;
			else canSeeMe = true;
		}
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.gameObject.tag == "Wall") canSeeMe = true;
		
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
           
        }
	}

	IEnumerator FollowPath() {
		if (animator.GetBool("Detectado")) {
			if (path.Length > 0) {
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

                    /* Debug.Log(currentWaypoint.x);
                     Debug.Log(currentWaypoint.y);
                     Debug.Log(currentWaypoint.z);*/
                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    yield return null;
                }            

            }
        }
      
	}



    public Vector3[] waypointsPatrol => patrolWaypoints;
	public float orPosx => originalX;
	public float orPosz => originalZ;

	public Quaternion orRot => originalRotation;
	
	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
