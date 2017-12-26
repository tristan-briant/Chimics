using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class titleManager : MonoBehaviour {

    public GameObject levelm;
    levelManager LVM;

	// Use this for initialization
	void Start () {
        LVM = levelm.GetComponent<levelManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (LVM.LevelName().Contains("Tuto") || LVM.LevelName().Contains("Dida"))
            GetComponent<Text>().text = LVM.LevelName();
        else
            GetComponent<Text>().text = LVM.LevelName() + " - Réaction " + (LVM.currentReaction + 1);

        transform.parent.GetComponent<Image>().color = LVM.LevelColor[LVM.currentLevel];
	}
}
