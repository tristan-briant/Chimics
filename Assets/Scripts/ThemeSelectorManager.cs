using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeSelectorManager : MonoBehaviour {

    public void Start()
    {
        transform.localPosition = Vector3.zero;
    }


    public void LoadActivity(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            for(int k=transform.parent.childCount-1;k>0; k--)
            {
                if (transform.parent.GetChild(k).gameObject.activeSelf == true)
                {
                    transform.parent.GetChild(k).gameObject.SetActive(false);
                    return;
                }
            }
            Application.Quit();
        }
    }
   
}
