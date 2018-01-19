﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public int currentLevel;
    public int currentReaction;

    public int completedLevel=0;
    public int maxLevel;
    public GameObject Playground;
    public GameObject ActivitiesSelector;
    public GameObject Game;

    [Header("Debug Mode")]
    public bool debug = false;

    public Transform[] levels;
    public string[] LevelNames;
    public string levelName;
    public Color[] LevelColor;
    public LevelParameters[] Parameters;


    private void Start()
    {  
        ActivitiesSelector.SetActive(true);
        Game.SetActive(false);
    }

    public void SetLevels(GameObject[] ls) {

        if (levels != null)
            foreach (Transform child in levels)
            {
                Destroy(child.gameObject);
            }

        levels = new Transform[ls.Length];

        int n = 0;
        foreach (GameObject level in ls)
        {
            Transform lv = Instantiate<GameObject>(level).transform;
            lv.SetParent(Playground.transform);
            lv.localScale = new Vector3(1, 1, 1);
            lv.localPosition = new Vector3(0, 0, 0);
            lv.gameObject.SetActive(false);
            

            levels[n++] = lv;
        }
    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber >= levels.Length)
            return;
         ActivitiesSelector.SetActive(false); // desactive le menu
        Game.SetActive(true);

        currentLevel = levelNumber;

        foreach (Transform child in Playground.transform)
            child.gameObject.SetActive(false);

        Transform lv = levels[currentLevel];

        lv.gameObject.SetActive(true);
        GameController gc = lv.GetComponent<GameController>();
        //gc.ResetLevel();
        gc.SetupLevel(true);
       
        SetTitlePanel();

 
    }

    public Transform CurrentLevel()
    {
        return levels[currentLevel];
    }

    public void LoadNextLevel()
    {

        if (currentLevel < levels.Length - 1)
        {
            /*if (CurrentLevel().gameObject.activeSelf)
                CurrentLevel().GetComponent<GameController>().ResetLevel();*/

            currentLevel++;
            LoadLevel(currentLevel);
        }

    }

    public void LoadPreviousLevel()
    {
        if (0 <currentLevel)
        {
            /*if (CurrentLevel().gameObject.activeSelf)
                CurrentLevel().GetComponent<GameController>().ResetLevel();*/
            currentLevel--;
            LoadLevel(currentLevel);
        }

       
    }


    /*public void LoadNextReaction()
    {

        if(CurrentReaction().gameObject.activeSelf)
            CurrentReaction().GetComponent<GameController>().ResetLevel();

        if (currentReaction < reactions[currentLevel].Length - 1)
        {
            currentReaction++;
        }
        else if (currentLevel < reactions.Length - 1) {
            currentLevel++;
            currentReaction = 0;
        }

        LoadReaction(currentReaction);
    }*/

    /*public void LoadPreviousReaction()
    {
        if (CurrentReaction().gameObject.activeSelf)
            CurrentReaction().GetComponent<GameController>().ResetLevel();

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
    }*/

    /*public void LoadLevel(int level)
    {
        if(CurrentReaction().gameObject.activeSelf)
            CurrentReaction().GetComponent<GameController>().ResetLevel();

        if (level >= reactions.Length)
            return;

        currentLevel = level;

        if (LevelNames[level].Contains("Tuto") || LevelName().Contains("Dida")) // Niveau de tuto On va direct au Game
        {
            LoadReaction(0);
        }
        else
        {
            LevelSelector.SetActive(false);
            ReactionSelector.SetActive(true);
            Game.SetActive(false);
        }*/



    /*public void LoadReaction(int reaction)
    {
        if (reaction >= reactions[currentLevel].Length)
            return;

        currentReaction = reaction;

        foreach (Transform child in Playground.transform)
            child.gameObject.SetActive(false);

        Transform lv = reactions[currentLevel][currentReaction];

        lv.gameObject.SetActive(true);
        GameController gc = lv.GetComponent<GameController>();
        if (gc)
        {
            gc.ResetLevel();
        }
        SetTitlePanel();
        
        LevelSelector.SetActive(false); // desactive le menu
        ReactionSelector.SetActive(false); // desactive le menu
        Game.SetActive(true);

    }*/

    /*public Transform CurrentReaction()
    {
        return reactions[currentLevel][currentReaction];

    }*/

    public string LevelName() {
        // Return the name of the current level

        return LevelNames[currentLevel];
    }

    void SetTitlePanel()
    {
        //LevelParameters parameters =Parameters[currentLevel];
        string title = levelName + " " + (currentLevel + 1);

        /*if (parameters.levelNameOn) title += parameters.LevelName;
        if (parameters.levelNameOn && parameters.subLevelNameOn) title += " - ";
        if (parameters.subLevelNameOn) title += parameters.subLevelName;
        if (parameters.subLevelNumberOn) title += " " + (currentReaction + 1);*/

      

        Game.transform.Find("Panel/Title").GetComponent<Text>().text = title;
        //Game.transform.Find("Panel").GetComponent<Image>().color = LevelColor[currentLevel];

        if ( currentLevel== levels.Length-1)
            Game.transform.Find("Panel/NextLevel").GetComponent<Button>().interactable = false;
        else
            Game.transform.Find("Panel/NextLevel").GetComponent<Button>().interactable = true;

        if (currentLevel == 0)
            Game.transform.Find("Panel/PreviousLevel").GetComponent<Button>().interactable = false;
        else
            Game.transform.Find("Panel/PreviousLevel").GetComponent<Button>().interactable = true;

    }


}
