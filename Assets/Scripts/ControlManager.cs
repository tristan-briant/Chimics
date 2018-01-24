using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlManager : MonoBehaviour
{
    public LevelManager LVM;
  
    void Start()
    {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>(); 
    }


    public void Validate() {
        LVM.CurrentLevel().GetComponent<GameController>().Validate();
    }

    public void Clear()
    {
        LVM.CurrentLevel().GetComponent<GameController>().ClearLevel();
    }

    public void ResetLevel()
    {
        LVM.CurrentLevel().GetComponent<GameController>().ResetLevel();
    }

    public void NextLevel()
    {
        LVM.LoadNextLevel();
    }

    public void Correction()
    {
        LVM.CurrentLevel().GetComponent<GameController>().ShowCorrection();
    }

    public void Next()
    {
        LVM.CurrentLevel().GetComponent<GameControllerArrow>().NextStep();
    }

    public void Previous()
    {
        LVM.CurrentLevel().GetComponent<GameControllerArrow>().PreviousStep();
    }

}