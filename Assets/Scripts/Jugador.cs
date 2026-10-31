using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    public float velocidad = 4f;
    public float fuerzaSalto = 7f;
    private Rigidbody2D rb;
    private float movimiento;
    private bool enSuelo;
    private bool terminado;
    private float tiempoRebote;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        movimiento = value.Get<Vector2>().x;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && enSuelo && !terminado && tiempoRebote <= 0f)
        {
            enSuelo = false;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x, 0f
            );

            rb.AddForce(
                Vector2.up * fuerzaSalto, 
                ForceMode2D.Impulse
            );
        }
    }

    void FixedUpdate()
    {
        if (terminado)
        {
            return;
        }

        if(tiempoRebote > 0f)
        {
            tiempoRebote -= Time.fixedDeltaTime;
            return;
        }

        rb.linearVelocity = new Vector2(
            movimiento * velocidad, 
            rb.linearVelocity.y
        );
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.5f && rb.linearVelocity.y <= 0.1f)
            {
                enSuelo = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        enSuelo = false;
    }

    public void BloquearPorRebote()
    {
        tiempoRebote = 0.35f;
        enSuelo = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (terminado)
        {
            return;
        }

        if (other.gameObject.name == "Lava")
        {
            terminado = true;
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }

        else if(other.gameObject.name == "Meta")
        {
            terminado = true;
            Debug.Log("Nivel superado");
        }
    }
}
