using UnityEngine;

// Fisica de patinacao usada por TODOS os patinadores (jogador e inimigos):
// a velocidade aumenta aos poucos e, quanto mais rapido, mais dificil fazer curva.
public class SkaterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    public float maxSpeed = 5.5f;        // velocidade maxima deste patinador
    public float acceleration = 3.2f;    // quanto a velocidade aumenta por segundo
    public float friction = 1.5f;        // quanto a velocidade diminui sem apertar nada
    public float brakeForce = 12f;       // quanto a velocidade diminui ao frear
    public float turnSpeedSlow = 720f;   // graus por segundo quando esta devagar
    public float turnSpeedFast = 120f;   // graus por segundo quando esta na velocidade fastSpeed
    public float fastSpeed = 5.5f;       // velocidade em que fica mais dificil virar
    public float knockback = 3.5f;       // velocidade ao trombar com outro patinador

    public Vector2 direction = Vector2.right;
    public float currentSpeed = 0f;
    public bool braking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Chamado no FixedUpdate de quem controla o patinador.
    // movement e para onde ele quer ir (como um joystick). Vector2.zero = nao apertar nada.
    public void Move(Vector2 movement)
    {
        if (movement.magnitude > 0.1f)
        {
            float angle = Vector2.Angle(direction, movement);

            if (angle > 120f && currentSpeed > 0.5f)
            {
                // Quer ir para o lado contrario: freia
                braking = true;
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, brakeForce * Time.fixedDeltaTime);
            }
            else
            {
                braking = false;

                if (currentSpeed < 0.5f)
                {
                    // Parado: pode virar para qualquer lado na hora
                    direction = movement.normalized;
                }
                else
                {
                    // Quanto mais rapido, mais devagar consegue virar
                    float turnSpeed = Mathf.Lerp(turnSpeedSlow, turnSpeedFast, currentSpeed / fastSpeed);
                    direction = Vector3.RotateTowards(direction, movement.normalized, turnSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 0f);
                }

                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
            }
        }
        else
        {
            // Sem apertar nada: continua deslizando e perde velocidade aos poucos
            braking = false;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, friction * Time.fixedDeltaTime);
        }

        rb.linearVelocity = direction * currentSpeed;

        // Vira o sprite para o lado que esta indo
        if (direction.x < -0.1f)
        {
            sprite.flipX = true;
        }
        else if (direction.x > 0.1f)
        {
            sprite.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Inimigo")
        {
            // Trombou com outro patinador: os dois sao empurrados para longe
            direction = normal;
            currentSpeed = knockback;
        }
        else
        {
            // Bateu na parede ou no cone: quica e perde metade da velocidade
            direction = Vector2.Reflect(direction, normal);
            currentSpeed = currentSpeed * 0.5f;
        }
    }
}
