using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;
    [SerializeField] Text       scoreText;
    [SerializeField] TextMeshProUGUI meterText;
    [SerializeField] TextMeshProUGUI gameoverMeterText;

    //SE
    [SerializeField] AudioClip gameClearSE;
    [SerializeField] AudioClip gameOverSE;
    AudioSource audioSource;

    const int MAX_SCORE = 9999;
    int score = 0;
    private int meter = 0;
    private float time = 0;

    private void Start()
    {
        scoreText.text = score.ToString();
        audioSource = GetComponent<AudioSource>();
        meterText.text = "0 m";
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 0.1f)
        {
            meter += 1;
            time = 0;
        }
        meterText.text = meter + " m";
    }

    public void AddScore(int val)
    {
        score += val;
        if (score >= MAX_SCORE)
        { 
            score = MAX_SCORE;
        }
        scoreText.text = score.ToString();
    }
    public void GameOver()
    {
        meterText.gameObject.SetActive(false);
        gameOverText.SetActive(true);
        gameoverMeterText.gameObject.SetActive(true);
        gameoverMeterText.text = "TOTAL DISTANCE is ..." + meter + " m";
        audioSource.PlayOneShot(gameOverSE);
        Invoke("RestartScene", 2.0f);
    }

    public void GameClear()
    {
        gameClearText.SetActive(true);
        audioSource.PlayOneShot(gameClearSE);
        Invoke("RestartScene", 2.0f);
    }


    public void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }

}
