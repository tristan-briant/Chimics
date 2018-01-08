using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class titleManager : MonoBehaviour {

    public GameObject levelm;
    levelManager LVM;

    public bool levelNameOn = true;
    public bool subLevelNameOn = false;
    public bool subLevelNumberOn = false;


    public string subLevelName = "";


	// Use this for initialization
	void Start () {
        LVM = levelm.GetComponent<levelManager>();
	}
	
	// Update is called once per frame
	void Update () {
        LevelParameters parameters = LVM.Parameters[LVM.currentLevel];
        string title = "";

        if (parameters.levelNameOn) title += parameters.LevelName;
        if (parameters.levelNameOn && parameters.subLevelNameOn) title += " - ";
        if (parameters.subLevelNameOn) title += parameters.subLevelName;
        if (parameters.subLevelNumberOn) title += " " + (LVM.currentReaction + 1);

        GetComponent<Text>().text = title;
        transform.parent.GetComponent<Image>().color = LVM.LevelColor[LVM.currentLevel];
	}
}
