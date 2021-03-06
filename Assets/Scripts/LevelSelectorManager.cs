﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelSelectorManager : MonoBehaviour {
    public GameObject levelList;
    public GameObject button;
    public GameObject Playground;
    public GameObject levelManager;
    public GameObject Game;
    public GameObject ReactionSelector;
    LevelManager LVM;

    bool started = false;

    void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);

        LVM = levelManager.GetComponent<LevelManager>();

        CreateLevelSelector();

        started = true;
    }



    public void Update()
    {
        /*int cpLevel = LVM.completedLevel;

        for (int i = 0; i < levelList.transform.childCount; i++)
        {
            if (LVM.debug || i < cpLevel + 1)
                levelList.transform.GetChild(i).GetComponent<Button>().interactable = true;
            else
                levelList.transform.GetChild(i).GetComponent<Button>().interactable = false;

        }*/


        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Activities");
        //Application.Quit();
    }

    private void OnEnable()
    {
        if (started)
            CreateLevelSelector();
    }

    public void CreateLevelSelector()
    {

        foreach (Transform child in levelList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        /*for (int i = 0; i < LVM.reactions.Length; i++)
        {
            GameObject go = Instantiate(button);
            go.transform.SetParent(levelList.transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.Find("Text").transform.GetComponent<Text>().text = LVM.LevelNames[i];//"Niveau " + (i + 1);
            go.transform.GetComponent<Image>().color = LVM.LevelColor[i];

            int level = i;
            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(delegate () { LVM.LoadLevel(level); });
        }*/
    }


    public void BackToActivities() {
        SceneManager.LoadScene("Activities");
    }

}
