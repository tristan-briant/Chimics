using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateController : MonoBehaviour {

    public LevelManager LVM;
    public GameObject pg;
    
    //front end for accessing level gamecontroller via the fail animation

    public void ResetElements()
    {
        LVM.CurrentLevel().GetComponent<GameController>().ResetElements();
    }

    public void ClearLevel()
    {
        LVM.CurrentLevel().GetComponent<GameController>().ClearLevel();
    }

    public void ClickableDisable()
    {
        LVM.CurrentLevel().GetComponent<GameController>().ClickableDisable();
    }

    public void ClickableEnable()
    {
        LVM.CurrentLevel().GetComponent<GameController>().ClickableEnable();
    }

    public void ShowTip()
    {
        LVM.CurrentLevel().GetComponent<GameController>().ShowTip();
    }

}
