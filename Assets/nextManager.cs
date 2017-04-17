using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nextManager : MonoBehaviour {
    levelManager LVM;
    
    void Start()
    {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (LVM.currentLevel >= LVM.completedLevel || LVM.currentLevel==LVM.maxLevel-1)
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;
    }
}
