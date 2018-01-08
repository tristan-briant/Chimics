using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateController : MonoBehaviour {

    public LevelManager LVM;
    public GameObject pg;
    
    //front end for accessing level gamecontroller via the fail animation

    public void ResetElements()
    {
        LVM.CurrentReaction().GetComponent<GameController>().ResetElements();
    }

    public void ClearLevel()
    {
        LVM.CurrentReaction().GetComponent<GameController>().ClearLevel();
    }

    public void ClickableDisable()
    {
        LVM.CurrentReaction().GetComponent<GameController>().ClickableDisable();
    }

    public void ClickableEnable()
    {
        LVM.CurrentReaction().GetComponent<GameController>().ClickableEnable();
    }

    public void ShowTip()
    {
        LVM.CurrentReaction().GetComponent<GameController>().ShowTip();
    }

}
