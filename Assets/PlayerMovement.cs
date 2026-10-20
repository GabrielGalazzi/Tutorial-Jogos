using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private SkaterMovement skater;
    private SpriteRenderer sprite;
    AudioSource audio;

    public AudioSource glideAudio;   // som de deslizar (loop)
    public AudioClip stopSound;      // som de frear
    public AudioClip crashSound;     // som de batida

    private float hitTimer = 0f;     // tempo invencivel depois de levar uma batida

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skater = GetComponent<SkaterMovement>();
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Controle (joystick / Xbox): analogico esquerdo ou setas do controle
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue() + Gamepad.current.dpad.ReadValue();
            if (stick.magnitude > 0.2f)
            {
                movement = stick;
            }
        }

        // Patina com a mesma fisica dos outros patinadores
        bool wasBraking = skater.braking;
        skater.Move(movement);

        // Som de frear quando comeca a frear rapido
        if (skater.braking && !wasBraking && skater.currentSpeed > skater.maxSpeed * 0.4f)
        {
            audio.PlayOneShot(stopSound);
        }

        // Som de deslizar fica mais alto quanto mais rapido
        glideAudio.volume = skater.currentSpeed / skater.maxSpeed;

        // Pisca enquanto esta invencivel
        if (hitTimer > 0f)
        {
            hitTimer -= Time.fixedDeltaTime;
            sprite.enabled = (int)(hitTimer * 10) % 2 == 0;
        }
        else
        {
            sprite.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coletavel")
        {
            audio.Play();
            GameController.Collect();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Inimigo")
        {
            if (hitTimer <= 0f)
            {
                audio.PlayOneShot(crashSound);
                GameController.TakeHit();
                hitTimer = 1.5f;

                if (GameController.gameOver)
                {
                    glideAudio.Stop();
                }
            }
        }
        else if (collision.relativeVelocity.magnitude > 2f)
        {
            // Bateu forte na parede ou no cone
            audio.PlayOneShot(crashSound, collision.relativeVelocity.magnitude / skater.maxSpeed);
        }
    }
}
