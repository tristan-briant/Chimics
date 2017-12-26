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
        if (LVM.currentLevel < LVM.reactions.Length - 1 || LVM.currentReaction < LVM.reactions[LVM.currentLevel].Length-1 )
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;
    }
}
