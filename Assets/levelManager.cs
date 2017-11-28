using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelManager : MonoBehaviour {

    public int currentLevel;
    public int completedLevel=0;
    public int maxLevel;
    public GameObject Playground;
    public GameObject Menu;

    [Header("Debug Mode")]
    public bool debug = false;

    public List<GameObject> levels;
    
    private void Awake()
    {
        levels = new List<GameObject>();
 
        foreach (Transform child in Playground.transform)
        {
            levels.Add(Resources.Load<GameObject>("Level/"+ child.gameObject.name));
        }
   
        maxLevel = levels.Count; 
    }

    public void loadNextLevel()
    {
        loadLevel(currentLevel+1);
    }

    public void loadLevel(int levelNumber)
    {
        if (levelNumber >= maxLevel)
            return;

        foreach (Transform child in Playground.transform)
            GameObject.Destroy(child.gameObject);

        GameObject lv=Instantiate(levels[levelNumber]);
        lv.transform.SetParent(Playground.transform);

        Menu.SetActive(false); // desactive le menu

        currentLevel = levelNumber;


    }

}
