using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelManager : MonoBehaviour {

    public int currentLevel;
    public int currentReaction;

    public int completedLevel=0;
    public int maxLevel;
    public GameObject Playground;
    public GameObject ReactionSelector;
    public GameObject LevelSelector;
    public GameObject Game;

    [Header("Debug Mode")]
    public bool debug = false;

    public Transform[][] reactions;
    
    private void Awake()
    {
  
        Transform levels = GameObject.Find("Levels").transform;

        reactions = new Transform[levels.childCount][];

        int n = 0;

        foreach (Transform child in levels) {
            int nreact = child.childCount;

            reactions[n] = new Transform[nreact];

            for (int i = 0; i < nreact; i++) 
            {
                Transform r = child.GetChild(0);
 
                r.SetParent(Playground.transform);
                r.localScale = new Vector3(1, 1, 1);
                r.localPosition = new Vector3(0, 0, 0);
                reactions[n][i] = r;
            }

            n++;
        }

        

    }

    private void Start()
    {
        LevelSelector.SetActive(true);
        ReactionSelector.SetActive(false);
        Game.SetActive(false);
    }

    public void LoadNextReaction()
    {
        if (currentReaction < reactions[currentLevel].Length - 1)
        {
            currentReaction++;
        }
        else if (currentLevel < reactions.Length - 1) {
            currentLevel++;
            currentReaction = 0;
        }

        LoadReaction(currentReaction);
    }

    public void LoadPreviousReaction()
    {
        if (currentReaction > 0) 
        {
            currentReaction--;
        }
        else if (currentLevel > 0)
        {
            currentLevel--;
            currentReaction = reactions[currentLevel].Length - 1;
        }

        LoadReaction(currentReaction);
    }

    public void LoadLevel(int level)
    {
        if (level >= reactions.Length)
            return;

        currentLevel = level;

        LevelSelector.SetActive(false); 
        ReactionSelector.SetActive(true); 
        Game.SetActive(false);
    }


        public void LoadReaction(int reaction)
    {
        if (reaction >= reactions[currentLevel].Length)
            return;

        currentReaction = reaction;

        foreach (Transform child in Playground.transform)
            child.gameObject.SetActive(false);

        Transform lv = reactions[currentLevel][currentReaction];

        lv.gameObject.SetActive(true);
        lv.GetComponent<gameController>().ResetLevel();
        //lv.GetComponent<gameController>().ClickableEnable();
        //lv.GetComponent<gameController>().failCount = 0;

        LevelSelector.SetActive(false); // desactive le menu
        ReactionSelector.SetActive(false); // desactive le menu
        Game.SetActive(true);

        //currentLevel = levelNumber;


    }

    public Transform CurrentReaction()
    {
        return reactions[currentLevel][currentReaction];

    }
    


}
