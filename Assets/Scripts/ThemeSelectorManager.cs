using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeSelectorManager : MonoBehaviour {

	
    public void LoadActivity(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

}
