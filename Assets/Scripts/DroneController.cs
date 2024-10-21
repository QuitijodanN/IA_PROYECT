using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float rotationSpeed = 90f;  // Grados por segundo
    public float rotationAngle = 45f;  // Ángulo de rotación máxima
    public float pauseDuration = 1f;   // Duración de la pausa en segundos

    private float currentRotation = 0f;
    private bool rotatingRight = true;

    private bool isPaused = false;

    public Light pointLight;  // Referencia a la luz del dron
    private Color originalColor;
    private Quaternion initialRotation; // Guardar la rotación inicial

    void Start()
    {
        if (pointLight != null) {
            originalColor = pointLight.color;  // Guardamos el color original de la luz
        }

        initialRotation = transform.localRotation;  // Guardar la rotación original
        StartCoroutine(RotateDrone());
    }

    IEnumerator RotateDrone()
    {
        while (true) {
            // Si no está en pausa, realiza la rotación
            if (!isPaused) {
                float rotationThisFrame = rotationSpeed * Time.deltaTime;

                if (rotatingRight) {
                    currentRotation += rotationThisFrame;
                    if (currentRotation >= rotationAngle) {
                        rotatingRight = false;
                        currentRotation = rotationAngle;  // Limitar rotación
                        isPaused = true;  // Pausar la rotación
                        yield return new WaitForSeconds(pauseDuration);  // Pausa antes de cambiar la dirección
                        isPaused = false;
                    }
                }
                else {
                    currentRotation -= rotationThisFrame;
                    if (currentRotation <= -rotationAngle) {
                        rotatingRight = true;
                        currentRotation = -rotationAngle;  // Limitar rotación
                        isPaused = true;  // Pausar la rotación
                        yield return new WaitForSeconds(pauseDuration);  // Pausa antes de cambiar la dirección
                        isPaused = false;
                    }
                }

                // Aplicar rotación relativa a la rotación inicial
                transform.localRotation = initialRotation * Quaternion.Euler(0, currentRotation, 0);
            }
            yield return null;  // Esperar un frame antes de continuar
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            if (pointLight != null) {
                pointLight.color = Color.red;  // Cambia la luz a rojo
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            if (pointLight != null) {
                pointLight.color = originalColor;  // Vuelve al color original
            }
        }
    }
}