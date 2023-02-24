using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float pipeSpace;
    public float lengthOfAllPipes;

    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pipeSpace = MapParallax.instance.pipeSpace;
        lengthOfAllPipes = MapParallax.instance.pipeSpace * 4;
        OnInvisible();
    }
    private void OnInvisible()
    {
        if(transform.position.x <= -3.5 && GameManager.instance.score < 100)
        {
            // thu ngắn khoảng cách của các ống khi đạt được mức điểm tương ứng
            if(GameManager.instance.score >= GameManager.instance.scoreToShortenPipes)
            {
                transform.position += new Vector3(lengthOfAllPipes + pipeSpace - MapParallax.instance.deductionDistance, 0f, 0f);
                MapParallax.instance.deductionDistance++;
            }
            // duy trì khoảng cách các ống 
            else  
                transform.position += new Vector3(lengthOfAllPipes + pipeSpace, 0f, 0f);

            // thay đổi độ cao của ống 
            transform.position = new Vector3(transform.position.x, RandomPosition(MapParallax.instance.minY,MapParallax.instance.maxY), 0f);
        }
    }

    private float RandomPosition(float min, float max)
    {
        return Random.Range(min, max);
    }    
}
