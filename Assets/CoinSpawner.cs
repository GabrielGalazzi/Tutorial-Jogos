using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int maxCoins = 6;
    public float spawnInterval = 1.5f; // segundos entre uma moeda e outra
    public float rangeX = 8.5f;        // ate onde a moeda pode aparecer para os lados
    public float rangeY = 4.1f;        // ate onde a moeda pode aparecer para cima e para baixo
    public float freeSpace = 0.7f;     // espaco livre que precisa ter em volta da moeda
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            // As moedas ficam como filhos deste objeto, entao e so contar os filhos
            if (transform.childCount < maxCoins)
            {
                // Escolhe um lugar aleatorio da pista
                Vector2 position = new Vector2(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY));

                // So cria a moeda se o lugar estiver livre (sem cone, parede, patinador ou outra moeda)
                if (Physics2D.OverlapCircle(position, freeSpace) == null)
                {
                    Instantiate(coinPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
