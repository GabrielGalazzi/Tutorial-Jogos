using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer sprite;
    public float lifetime = 8f;   // segundos ate a moeda sumir

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        // Pisca nos ultimos 2 segundos para avisar que vai sumir
        if (lifetime < 2f)
        {
            sprite.enabled = (int)(lifetime * 10) % 2 == 0;
        }

        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
