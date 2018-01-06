using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlManager : MonoBehaviour
{
    public levelManager LVM;
    //public GameObject pg;

    public void Validate() {
        LVM.CurrentReaction().GetComponent<gameController>().Validate();
    }

    public void Clear()
    {
        LVM.CurrentReaction().GetComponent<gameController>().ClearLevel();
    }

    public void ResetLevel()
    {
        LVM.CurrentReaction().GetComponent<gameController>().ResetLevel();
    }

    public void NextLevel()
    {
        LVM.LoadNextReaction();
    }

}