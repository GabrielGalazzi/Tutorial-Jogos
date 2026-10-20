using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private Rigidbody2D rb;
    private SkaterMovement skater;

    public Transform player;
    public float startSpeed = 1.8f;      // velocidade maxima no inicio da partida
    public float speedPerSecond = 0.03f; // quanto fica mais rapido a cada segundo de jogo
    public float maxSpeed = 4.5f;        // limite de velocidade, mesmo depois de muito tempo
    public float waitAfterHit = 2f;      // tempo sem perseguir depois de acertar o jogador
    private float waitTimer = 0f;

    public float avoidDistance = 2.5f;   // distancia em que comeca a desviar dos outros patinadores
    public float avoidStrength = 3f;     // quanto ele desvia
    private GameObject[] enemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skater = GetComponent<SkaterMovement>();
        enemies = GameObject.FindGameObjectsWithTag("Inimigo");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Quanto mais tempo de jogo, mais rapido ele pode ficar
        skater.maxSpeed = Mathf.Min(startSpeed + GameController.time * speedPerSecond, maxSpeed);

        Vector2 movement = Vector2.zero;

        if (waitTimer > 0f)
        {
            // Depois de acertar o jogador, para de perseguir e vai deslizando
            waitTimer -= Time.fixedDeltaTime;
        }
        else
        {
            // Direcao ate o jogador
            movement = ((Vector2)player.position - rb.position).normalized;
        }

        // Desvia dos patinadores que estao perto (quanto mais perto, mais forte desvia)
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != gameObject)
            {
                Vector2 away = rb.position - (Vector2)enemies[i].transform.position;
                float distance = away.magnitude;
                if (distance < avoidDistance)
                {
                    movement += away.normalized * (1f - distance / avoidDistance) * avoidStrength;
                }
            }
        }

        // Patina com a mesma fisica do jogador
        skater.Move(movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            waitTimer = waitAfterHit;
        }
    }
}
