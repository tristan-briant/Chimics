﻿using System.Collections;
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
        GetComponent<Text>().text = "Réaction " + (lvm.currentLevel + 1);
	}
}