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
    public List<Sprite> listSkin = new List<Sprite>();
    private float currentPipeX;
    private float currentPipeY;
    private float birdX;
    private float birdY;
    public bool isStart;
    public int scoreToShortenPipes;
    public int scoreToWin;
    public GameObject birdAtWin;
    public GameObject heartAtWin;
    public float waitToWinLoad;
    private float winLoadCounter;
    public GameObject pipes;
    private int[] ranked = { 90, 85, 80, 77, 60, 60, 50, 25, 10, 5, 5 };
    public int highScore;
    public int myRanked;
    public bool isPause;
    public bool isContinute;
    public int currentSkin;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        myRanked = 1;
        Time.timeScale = 1f;
        isStart = false;
        isPause = false;
        score = 0;
        currentPipe = 0;
        currentSkin = PlayerPrefs.GetInt("Skin");
        LoadSkin(currentSkin);
        winLoadCounter = waitToWinLoad;
        listPipes = MapParallax.instance.listPipes;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentPipeAndScore();
        PipesTrigger();
        if(score == scoreToWin)
            WinGame();
        if(isPause)
            CanvasManager.instance.WaitToContinute();
    }

    private void GetCurrentPipeAndScore()
    {
        foreach (GameObject item in listPipes)
        {
            if (Vector3.Distance(item.transform.position, BirdMove.instance.transform.position) < Vector3.Distance(listPipes[currentPipe].transform.position, BirdMove.instance.transform.position))
            {
                currentPipe = listPipes.IndexOf(item);
                AudioManager.instance.GetScoreSFX();
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
        CanvasManager.instance.StartGame();
        BirdMove.instance.theBody.transform.position = new Vector3(-1.4f, 0f, 0f);
    }

    public void StopGame()
    {
        // gọi sound efect 1 lần không bị lặp lại
        if(isStart)
            AudioManager.instance.HitPipesSFX();
        //Time.timeScale = 0f;
        isStart = false;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            if (PlayerPrefs.GetInt("HighScore") < score)
            {
                SetNewHighScore();
            }
        }
        else
            SetNewHighScore();
        CanvasManager.instance.FailGame();
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

       // tạo cut scense khi thắng ( 1 con chim nữ bay lại gần hiện lên trái tim
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
           CanvasManager.instance.WinGame();
       }
    }

    public void ShowRanked()
    {
        // Tính my rank
        highScore = PlayerPrefs.GetInt("HighScore");
        if (highScore < ranked[0])
        {
            myRanked++;
            for (int i = 1; i < ranked.Length; i++)
            {
                if (highScore < ranked[i] && ranked[i] != ranked[i - 1])
                    myRanked++;
            }
        }
        // show list rank
        CanvasManager.instance.listRanked.text += 1 + ": Score " + ranked[0].ToString() + "\n";
        int stt = 2;
        for (int i = 1; i < ranked.Length; i++)
        {
            CanvasManager.instance.listRanked.text += stt + ": Score " + ranked[i].ToString() + "\n";
            if (ranked[i] != ranked[i - 1])
                stt++;
        }
        CanvasManager.instance.ShowRanked();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isStart = false;
        isPause = true;
        CanvasManager.instance.PauseGame();
    }

    public void ContinuteGame()
    {
        isContinute = true;
    }

    public void NextSkin()
    {
        if (currentSkin == listSkin.Count-1)
        {
            currentSkin = 0;
        }
        else
        {
            currentSkin++;
        }
        AudioManager.instance.SwooshingSFX();
        LoadSkin(currentSkin);
    }

    public void PrevousSkin()
    {
        if (currentSkin == 0)
        {
            currentSkin = listSkin.Count - 1;
        }
        else
        {
            currentSkin--;
        }
        AudioManager.instance.SwooshingSFX();
        LoadSkin(currentSkin);
    }

    public void LoadSkin(int current)
    {
        if (current != 0)
            BirdMove.instance.GetComponent<Animator>().enabled = false;
        else
            BirdMove.instance.GetComponent<Animator>().enabled = true;

        PlayerPrefs.SetInt("Skin", current);
        BirdMove.instance.theBody.GetComponent<SpriteRenderer>().sprite = listSkin[current];
    }    

}
