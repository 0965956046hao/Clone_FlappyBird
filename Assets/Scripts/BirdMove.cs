using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMove : MonoBehaviour
{
    public static BirdMove instance;
    public GameObject theBody;
    public float fallSpeed;
    public float flySpeed;
    private bool isFly;
    public float flyCoolDown;
    private float flyCounter;

    private float x;
    private float y;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        theBody.transform.position = Vector3.zero;
        x = 1;
        y = 1;
        isFly = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isStart)
        {
            BirdFly(CountAngle());
        }    
    }

    private void BirdFall(float angle)
    {
        theBody.transform.rotation = Quaternion.Euler(0, 0, angle);
        theBody.transform.position += new Vector3(0f, -1 * Time.deltaTime * fallSpeed, 0f);
    }

    private void BirdFly(float angle)
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.PlaySFX(1);
            isFly = true;
        }

        if (isFly)
        {
            x = 1;
            y = 1;
            theBody.transform.rotation = Quaternion.Euler(0, 0, angle);
            theBody.transform.position += new Vector3(0f, 1 * Time.deltaTime * flySpeed, 0f);
            if(flyCounter > 0)
            {
                flyCounter -= Time.deltaTime;
            }    
            else
            {
                flyCounter = flyCoolDown;
                this.isFly = false;
            }    
        }
        else { 
            BirdFall(CountAngle());
        }
    }

    private float CountAngle()
    {
        if (y > -1)
        {
            y -= Time.deltaTime * 2f;
        }
        else if (x > 0)
        {
            x -= Time.deltaTime * 2f;
        }
        return Mathf.Atan2(y , x) * Mathf.Rad2Deg;
    }
}
