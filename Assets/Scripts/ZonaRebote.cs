using UnityEngine;

public class ZonaRebote : MonoBehaviour
{
    public float fuerzaRebote = 8f;
    public float impulsoArriba = 4f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D cuerpo = collision.rigidbody;

        if (cuerpo == null)
        {
            return;
        }

        Vector2 direccion = -collision.GetContact(0).normal;

        Vector2 impulso = direccion * fuerzaRebote;

        // Agregamos impulso vertical al rebote.
        impulso += Vector2.up * impulsoArriba;

        cuerpo.linearVelocity = Vector2.zero;

        cuerpo.AddForce(
            impulso,
            ForceMode2D.Impulse
        );

        Jugador jugador = cuerpo.GetComponent<Jugador>();

        if (jugador != null)
        {
            jugador.BloquearPorRebote();
        }
    }
}