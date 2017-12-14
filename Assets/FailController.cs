using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailController : MonoBehaviour {

    public levelManager LVM;
    public GameObject pg;
    
    //front end for accessing level gamecontroller via the fail animation

    public void ResetElements()
    {
        pg.transform.GetChild(LVM.currentLevel).GetComponent<gameController>().ResetElements();
    }

    public void ClearLevel()
    {
        pg.transform.GetChild(LVM.currentLevel).GetComponent<gameController>().ClearLevel();
    }

    public void ClickableDisable()
    {
        pg.transform.GetChild(LVM.currentLevel).GetComponent<gameController>().ClickableDisable();
    }

    public void ClickableEnable()
    {
        pg.transform.GetChild(LVM.currentLevel).GetComponent<gameController>().ClickableEnable();
    }
}
