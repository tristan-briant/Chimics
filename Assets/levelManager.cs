using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelManager : MonoBehaviour {

    public int currentLevel;
    public int completedLevel=2;
    public int maxLevel;
    public GameObject Playground;
    public GameObject Menu;

    [Header("Debug Mode")]
    public bool debug = false;

    //public bool success;
    //public int failCount = 0;
    //GameObject molecule1, molecule2, tip;

    private void Start()
    {
        maxLevel = Playground.transform.childCount;
    }

    public void loadNextLevel()
    {
        loadLevel(currentLevel+1);
    }

    public void loadLevel(int levelNumber)
    {
        if (levelNumber >= maxLevel)
            return;

        for (int i = 0; i < maxLevel; i++)
        {
            Transform child = Playground.transform.GetChild(i);
            if (i == levelNumber)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<gameController>().resetLevel();
                Debug.Log("load");
            }
            else
                child.gameObject.SetActive(false);
        }

        Menu.SetActive(false); // desactive le menu

        currentLevel = levelNumber;

    }

}
