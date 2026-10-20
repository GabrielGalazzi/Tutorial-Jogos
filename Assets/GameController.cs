using UnityEngine;

public static class GameController
{
    public static int coins;
    public static int lives;
    public static float time;

    public static bool gameOver
    {
        get { return lives <= 0; }
    }

    // 1 ponto por segundo sobrevivido + 10 pontos por moeda
    public static int score
    {
        get { return (int)time + coins * 10; }
    }

    public static void Init()
    {
        coins = 0;
        lives = 3;
        time = 0;
        Time.timeScale = 1;
    }

    public static void Collect()
    {
        coins++;
    }

    public static void TakeHit()
    {
        lives--;
    }
}
