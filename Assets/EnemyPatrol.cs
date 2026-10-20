using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    private SkaterMovement skater;

    public Vector2[] points;     // pontos da rota (o patinador vai de um para o outro)
    private int currentPoint = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skater = GetComponent<SkaterMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 target = points[currentPoint];

        // Chegou perto do ponto: vai para o proximo (e volta para o primeiro no fim)
        if (Vector2.Distance(rb.position, target) < 0.6f)
        {
            currentPoint++;
            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
            }
            target = points[currentPoint];
        }

        // Patina ate o ponto com a mesma fisica do jogador
        skater.Move(target - rb.position);
    }
}
