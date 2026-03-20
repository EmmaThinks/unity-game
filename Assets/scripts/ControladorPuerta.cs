using UnityEngine;

public class ControladorPuerta : MonoBehaviour
{
    [Header("Configuraci�n de la Puerta")]
    public Transform puerta;
    public float anguloAbierta = 90f;
    public float velocidadApertura = 5f;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    private int partesDelJugadorEnArea = 0;
    private bool jugadorEnArea = false;

    void Start()
    {

        if (puerta == null)
        {
            Debug.LogError("�Falta asignar la puerta en el objeto " + gameObject.name + "!");
            return;
        }

        rotacionCerrada = puerta.rotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, 0, anguloAbierta);
    }

    void Update()
    {
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
            partesDelJugadorEnArea++; 
            jugadorEnArea = true;
        }
    }

    private void OnTriggerExit(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            partesDelJugadorEnArea--; 


            if (partesDelJugadorEnArea <= 0)
            {
                partesDelJugadorEnArea = 0;
                jugadorEnArea = false;
            }
        }
    }
}