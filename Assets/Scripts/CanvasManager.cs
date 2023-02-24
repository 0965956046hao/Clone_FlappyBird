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
        score.text = "0";
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        score.text = GameManager.instance.score.ToString();
        ScoreOnTable.text = score.text;
    }
}
