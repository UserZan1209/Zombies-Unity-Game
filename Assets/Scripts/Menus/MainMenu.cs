using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadM1()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadM2()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadM3()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadM4()
    {
        SceneManager.LoadScene(4);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.F1))
            SceneManager.LoadScene(5);
    }
}
