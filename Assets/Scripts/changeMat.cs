using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMat : MonoBehaviour
{
    public Material metal;
    public Material original;
    public Material current;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = original;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            GetComponent<Renderer>().material = metal;
            print("metal");
        }
        else
        {
            GetComponent<Renderer>().material = original;
        }
    }
}
