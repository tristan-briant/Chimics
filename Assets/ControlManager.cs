using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlManager : MonoBehaviour
{
    public levelManager LVM;
    public GameObject pg;

    public void Validate() {
        //pg.transform.GetChild(LVM.currentLevel).GetComponent<gameController>().Validate();
        LVM.CurrentReaction().GetComponent<gameController>().Validate();
    }

    public void Clear()
    {
        //pg.transform.GetChild(LVM.currentLevel).GetComponent<gameController>().ClearLevel();
        LVM.CurrentReaction().GetComponent<gameController>().ClearLevel();
    }

}