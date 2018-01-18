using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlManager : MonoBehaviour
{
    public LevelManager LVM;
  
    public void Validate() {
        LVM.CurrentReaction().GetComponent<GameController>().Validate();
    }

    public void Clear()
    {
        LVM.CurrentReaction().GetComponent<GameController>().ClearLevel();
    }

    public void ResetLevel()
    {
        LVM.CurrentReaction().GetComponent<GameController>().ResetLevel();
    }

    public void NextLevel()
    {
        LVM.LoadNextReaction();
    }

    public void Correction()
    {
        LVM.CurrentReaction().GetComponent<GameController>().ShowCorrection();
    }

    public void Next()
    {
        LVM.CurrentReaction().GetComponent<GameControllerArrow>().NextStep();
    }

    public void Previous()
    {
        LVM.CurrentReaction().GetComponent<GameControllerArrow>().PreviousStep();
    }

}