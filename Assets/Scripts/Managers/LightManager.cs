using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    Light[] lights;
    // Start is called before the first frame update
    void Start()
    {
        int childNum = transform.childCount;
        lights = new Light[childNum];

        for (int i = 0; i < childNum; i++) 
        {
            lights[i] = transform.GetChild(i).GetComponent<Light>();
        }

        for (int i = 0; i < childNum; i++)
        {
            lights[i].enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            Activate();
        }
    }

    public void Activate()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;
        }
    }
}
