using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score;
    public int currentPipe;
    public List<GameObject> listPipes = new List<GameObject>();
    public float currentPipeX;
    public float currentPipeY;
    public float birdX;
    public float birdY;
    public bool isStart;
    public int scoreToShortenPipes;
    public GameObject birdAtWin;
    public GameObject heartAtWin;
    public float waitToWinLoad;
    private float winLoadCounter;
    public GameObject pipes;
    private int[] ranked = { 90, 85, 80, 77, 60, 60, 50, 25, 10, 5, 5 };
    private int myRanked;
    public float waitToContinute;
    private float continuteLoadCounter;
    private bool isContinute;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        myRanked = 1;
        scoreToShortenPipes = 25;
        Time.timeScale = 1f;
        isStart = false;
        score = 0;
        currentPipe = 0;
        winLoadCounter = waitToWinLoad;
        continuteLoadCounter = waitToContinute;
        listPipes = MapParallax.instance.listPipes;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentPipeAndScore();
        PipesTrigger();
        if(score == 100)
            WinGame();
        WaitToContinute();
    }

    private void GetCurrentPipeAndScore()
    {
        foreach (GameObject item in listPipes)
        {
            if (Vector3.Distance(item.transform.position, BirdMove.instance.transform.position) < Vector3.Distance(listPipes[currentPipe].transform.position, BirdMove.instance.transform.position))
            {
                currentPipe = listPipes.IndexOf(item);
                AudioManager.instance.PlaySFX(0);
                score++;
            }
        }
    }

    private void PipesTrigger()
    {
        currentPipeX = listPipes[currentPipe].transform.position.x;
        currentPipeY = listPipes[currentPipe].transform.position.y;
        birdX = BirdMove.instance.theBody.transform.position.x;
        birdY = BirdMove.instance.theBody.transform.position.y;
        if (birdX >= currentPipeX - 1.1f && birdX <= currentPipeX + 1.1f)
        {
            if (birdY <= currentPipeY - 0.9f || birdY >= currentPipeY + 0.9f)
            {
                StopGame();
            }
        }
        else if (birdY >= 4.7f || birdY <= -4.7f)
        {
            StopGame();
        }
            
    }   
    
    public void StartGame()
    {
        isStart = true;
        CanvasManager.instance.score.gameObject.SetActive(true);
        CanvasManager.instance.startScreen.gameObject.SetActive(false);
        CanvasManager.instance.pauseButton.gameObject.SetActive(true);
        BirdMove.instance.theBody.transform.position = new Vector3(-1.4f, 0f, 0f);
    }

    public void StopGame()
    {
        if(isStart)
            AudioManager.instance.PlaySFX(2);
        //Time.timeScale = 0f;
        isStart = false;
        CanvasManager.instance.deadScreen.gameObject.SetActive(true);
        CanvasManager.instance.score.gameObject.SetActive(false);

        if (PlayerPrefs.HasKey("HighScore"))
        {
            if (PlayerPrefs.GetInt("HighScore") < score)
            {
                SetNewHighScore();
            }
        }
        else
            SetNewHighScore();
        
        CanvasManager.instance.highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        CanvasManager.instance.pauseButton.gameObject.SetActive(false);
    } 
    
    public void SetNewHighScore()
    {
        CanvasManager.instance.newImg.gameObject.SetActive(true);
        PlayerPrefs.SetInt("HighScore", score);
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinGame()
    {
       isStart = false;
       birdAtWin.gameObject.SetActive(true);
       pipes.gameObject.SetActive(false);
       BirdMove.instance.theBody.transform.position = new Vector3(BirdMove.instance.theBody.transform.position.x, 0f, 0f);
       BirdMove.instance.theBody.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(0, 1) * Mathf.Rad2Deg);
       if (BirdMove.instance.theBody.transform.position.x <= birdAtWin.transform.position.x - 0.85f)
           birdAtWin.transform.position += new Vector3(-1 * Time.deltaTime * MapParallax.instance.mapSpeed, 0f, 0f);
       else
           heartAtWin.gameObject.SetActive(true);

       if (winLoadCounter > 0)
       {
           winLoadCounter -= Time.deltaTime;
       }
       else
       {
           winLoadCounter = waitToWinLoad;
           CanvasManager.instance.winScreen.gameObject.SetActive(true);
           CanvasManager.instance.score.gameObject.SetActive(false);
       }
    }

    public void ShowRanked()
    {
        int stt = 2;
        int highScore = PlayerPrefs.GetInt("HighScore");
        CanvasManager.instance.listRanked.text += 1 + ": Score " + ranked[0].ToString() + "\n";
        if (highScore < ranked[0])
        {
            myRanked++;
            for (int i = 1; i < ranked.Length; i++)
            {
                if (highScore < ranked[i] && ranked[i] != ranked[i - 1])
                    myRanked++;
            }
        }
        for (int i = 1; i < ranked.Length; i++)
        {
            CanvasManager.instance.listRanked.text += stt + ": Score " + ranked[i].ToString() + "\n";
            if (ranked[i] != ranked[i - 1])
                stt++;
        }
        CanvasManager.instance.myHighScore.text = highScore.ToString();
        CanvasManager.instance.myRank.text = myRanked.ToString();
        CanvasManager.instance.rankedScreen.gameObject.SetActive(true);
        CanvasManager.instance.pauseButton.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isStart = false;
        CanvasManager.instance.pauseScreen.gameObject.SetActive(true);
        CanvasManager.instance.pauseButton.gameObject.SetActive(false);
    }

    public void ContinuteGame()
    {
        isContinute = true;
    }

    public void ExitRanked()
    {
        CanvasManager.instance.rankedScreen.gameObject.SetActive(false);
    }

    public void WaitToContinute()
    {
        if (isContinute)
        {
            Time.timeScale = 1f;
            if (continuteLoadCounter > 0)
            {
                CanvasManager.instance.continuteCooldown.text = continuteLoadCounter.ToString();
                continuteLoadCounter -= Time.deltaTime;
                
            }
            else
            {
                isStart = true;
                CanvasManager.instance.pauseScreen.gameObject.SetActive(false);
                CanvasManager.instance.pauseButton.gameObject.SetActive(true);
                continuteLoadCounter = waitToContinute;
                isContinute = false;
            }
            CanvasManager.instance.continuteButton.gameObject.SetActive(false);
            CanvasManager.instance.continuteCooldown.gameObject.SetActive(true);
        }
        else
        {
            CanvasManager.instance.continuteButton.gameObject.SetActive(true);
            CanvasManager.instance.continuteCooldown.gameObject.SetActive(false);
        }    
            
    }
}
