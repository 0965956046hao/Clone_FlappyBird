using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public Text score;
    public GameObject startScreen;
    public Image deadScreen;
    public Image winScreen;
    public Text ScoreOnTable;
    public Text highScore;
    public GameObject rankedScreen;
    public Text listRanked;
    public Text myRank;
    public Image newImg;
    public Text myHighScore;
    public Image pauseButton;
    public Image pauseScreen;
    public Text continuteCooldown;
    public Image continuteButton;

    public float waitToContinute;
    private float continuteLoadCounter;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        pauseScreen.gameObject.SetActive(false);
        startScreen.gameObject.SetActive(true);
        winScreen.gameObject.SetActive(false);
        newImg.gameObject.SetActive(false);
        deadScreen.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
        rankedScreen.gameObject.SetActive(false);

        continuteLoadCounter = waitToContinute;
        score.text = "0";
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        score.text = GameManager.instance.score.ToString();
        ScoreOnTable.text = score.text;
    }

    public void StartGame()
    {
        score.gameObject.SetActive(true);
        startScreen.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void FailGame()
    {
        deadScreen.gameObject.SetActive(true);
        score.gameObject.SetActive(false);
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        pauseButton.gameObject.SetActive(false);
    }

    public void WinGame()
    {
        winScreen.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
    }    
    
    public void ShowRanked()
    {
        myHighScore.text = GameManager.instance.highScore.ToString();
        myRank.text = GameManager.instance.myRanked.ToString();
        rankedScreen.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        pauseScreen.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void ExitRanked()
    {
        rankedScreen.gameObject.SetActive(false);
    }

    public void WaitToContinute()
    {
        if (GameManager.instance.isContinute)
        {
            Time.timeScale = 1f;
            if (continuteLoadCounter > 0)
            {
                continuteCooldown.text = continuteLoadCounter.ToString();
                continuteLoadCounter -= Time.deltaTime;

            }
            else
            {
                GameManager.instance.isStart = true;
                pauseScreen.gameObject.SetActive(false);
                pauseButton.gameObject.SetActive(true);
                continuteLoadCounter = waitToContinute;
                GameManager.instance.isContinute = false;
                GameManager.instance.isPause = false;
            }
            continuteButton.gameObject.SetActive(false);
            continuteCooldown.gameObject.SetActive(true);
        }
        else
        {
            continuteButton.gameObject.SetActive(true);
            continuteCooldown.gameObject.SetActive(false);
        }
    }
}
