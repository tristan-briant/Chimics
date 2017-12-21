using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PreviousManager : MonoBehaviour {
    levelManager LVM;

    void Start()
    {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LVM.currentLevel > 0 || LVM.currentReaction > 0)
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;
    }
}