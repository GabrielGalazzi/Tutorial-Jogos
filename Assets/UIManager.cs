using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public GameObject restartButton;   // botao selecionado ao terminar (para jogar com controle)
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text finalText;
    public GameObject[] hearts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameOver)
        {
            if (!endGamePanel.activeSelf)
            {
                EndGame();
            }
            return;
        }

        GameController.time += Time.deltaTime;

        timeText.text = "Tempo: " + GameController.time.ToString("F1") + "s";
        scoreText.text = "Pontos: " + GameController.score;

        // Mostra um coracao para cada vida
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < GameController.lives);
        }
    }

    void EndGame()
    {
        Time.timeScale = 0;

        // Salva o recorde
        int record = PlayerPrefs.GetInt("recorde", 0);
        if (GameController.score > record)
        {
            record = GameController.score;
            PlayerPrefs.SetInt("recorde", record);
        }

        finalText.text = "Fim de jogo!\n" +
                         "Tempo: " + GameController.time.ToString("F1") + "s\n" +
                         "Moedas: " + GameController.coins + "\n" +
                         "Pontos: " + GameController.score + "\n" +
                         "Recorde: " + record;

        // Esconde o HUD, as informacoes agora aparecem no painel
        timeText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }

        endGamePanel.SetActive(true);

        // Deixa o botao selecionado para funcionar com o controle (botao A)
        EventSystem.current.SetSelectedGameObject(restartButton);
    }
}
