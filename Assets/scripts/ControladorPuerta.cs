using UnityEngine;

public class ControladorPuerta : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    public Transform puerta;
    public float anguloAbierta = 90f;
    public float velocidadApertura = 5f;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    // Usamos un contador en lugar de un simple verdadero/falso. 
    // En VR, pueden entrar 2 manos y la cabeza. La puerta solo debe cerrar cuando el contador vuelva a 0.
    private int partesDelJugadorEnArea = 0;
    private bool jugadorEnArea = false;

    void Start()
    {
        // SISTEMA DE SEGURIDAD: Evita que Unity colapse o detenga el simulador si olvidaste asignar la puerta
        if (puerta == null)
        {
            Debug.LogError("¡Falta asignar la puerta en el objeto " + gameObject.name + "!");
            return;
        }

        rotacionCerrada = puerta.rotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, anguloAbierta, 0);
    }

    void Update()
    {
        // SISTEMA DE SEGURIDAD: Si no hay puerta, no intentes rotar nada.
        if (puerta == null) return;

        if (jugadorEnArea)
        {
            puerta.rotation = Quaternion.Slerp(puerta.rotation, rotacionAbierta, Time.deltaTime * velocidadApertura);
        }
        else
        {
            puerta.rotation = Quaternion.Slerp(puerta.rotation, rotacionCerrada, Time.deltaTime * velocidadApertura);
        }
    }

    private void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            partesDelJugadorEnArea++; // Sumamos 1 por cada parte del cuerpo que entra
            jugadorEnArea = true;
        }
    }

    private void OnTriggerExit(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            partesDelJugadorEnArea--; // Restamos 1 cuando sale una parte del cuerpo

            // Si el contador llega a 0 (o menos, por seguridad), el jugador salió por completo
            if (partesDelJugadorEnArea <= 0)
            {
                partesDelJugadorEnArea = 0;
                jugadorEnArea = false;
            }
        }
    }
}