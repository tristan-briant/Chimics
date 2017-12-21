using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateController : MonoBehaviour {

    public levelManager LVM;
    public GameObject pg;
    
    //front end for accessing level gamecontroller via the fail animation

    public void ResetElements()
    {
        LVM.CurrentReaction().GetComponent<gameController>().ResetElements();
    }

    public void ClearLevel()
    {
        LVM.CurrentReaction().GetComponent<gameController>().ClearLevel();
    }

    public void ClickableDisable()
    {
        LVM.CurrentReaction().GetComponent<gameController>().ClickableDisable();
    }

    public void ClickableEnable()
    {
        LVM.CurrentReaction().GetComponent<gameController>().ClickableEnable();
    }

    public void ShowTip()
    {
        LVM.CurrentReaction().GetComponent<gameController>().ShowTip();
    }

}
