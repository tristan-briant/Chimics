using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class titleManager : MonoBehaviour {

    public GameObject levelm;
    levelManager lvm;

	// Use this for initialization
	void Start () {
        lvm = levelm.GetComponent<levelManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (lvm.LevelName().Contains("Tuto"))
            GetComponent<Text>().text = lvm.LevelName();
        else
            GetComponent<Text>().text = lvm.LevelName() + " - Réaction " + (lvm.currentReaction + 1);

        transform.parent.GetComponent<Image>().color = lvm.LevelColor[lvm.currentLevel];
	}
}
