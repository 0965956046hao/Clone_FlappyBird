using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapParallax : MonoBehaviour
{
    public List<GameObject> listSkies = new List<GameObject>();
    public List<GameObject> listPipes = new List<GameObject>();
    public static MapParallax instance;
    public float mapSpeed;
    public GameObject enableSky;
    public GameObject bird;
    private int skyIndex;
    public float pipeSpace;
    public int deductionDistance;

    // Random vị trí ống lên xuống
    public float minY;
    public float maxY;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        minY = -2f;
        maxY = 2f;
        skyIndex = 1;
        enableSky = listSkies[skyIndex];
        deductionDistance = 1;
        PipesRelax();
    }

    // Update is called once per frame
    void Update()
    {
        MoveSky();
        if(GameManager.instance.isStart)
            MovePipe();
        CompleteShortenPipes();
    }

    private float CountSpace(GameObject sky, GameObject bird)
    {
        return Vector3.Distance(sky.transform.position, bird.transform.position);
    }

    private void MoveSky()
    {
        if (CountSpace(enableSky, bird) < 1)
        {
            if (skyIndex == 1)
            {
                skyIndex = 0;
                enableSky = listSkies[skyIndex];
                enableSky.transform.position = new Vector3(20f, 0, 0);
            }
            else
            {
                skyIndex = 1;
                enableSky = listSkies[skyIndex];
                enableSky.transform.position = new Vector3(20f, 0, 0);
            }

        }
        foreach (GameObject item in listSkies)
            item.transform.position += new Vector3(-1 * Time.deltaTime * mapSpeed, 0f, 0f);
    }

    private void MovePipe()
    {
        foreach (GameObject item in listPipes)
        {
            item.transform.position += new Vector3(-1 * Time.deltaTime * mapSpeed, 0f, 0f);
        }    
    }

    private void PipesRelax()
    {
        foreach (GameObject item in listPipes)
            item.transform.position = new Vector3(pipeSpace *(listPipes.IndexOf(item)+1), Random.Range(-2f, 2f), 0f);
    }

    private void CompleteShortenPipes()
    {
        if (deductionDistance == 5)
        {
            deductionDistance = 1;
            pipeSpace--;
            GameManager.instance.scoreToShortenPipes += 25;
            minY += 0.5f;
            maxY -= 0.5f;
        }
    }
}
