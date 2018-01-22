using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : GameController
{

    public GameObject ReadMoreButton;
    bool started = false;

    public void ReadMore()
    {
        Debug.Log("More !");
        if (anim.isActiveAndEnabled)
            anim.SetTrigger("Next");
    }

    public void ReadMoreToNext()
    {
        Button btn = ReadMoreButton.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(LVM.LoadNextLevel);
    }

    public void LoadNext()
    {
        LVM.LoadNextLevel();
    }
    
    public override void SetupLevel(bool playable)
    {
        base.SetupLevel(playable);

        FloatingButtons.transform.Find("Clear").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);
        if(FloatingButtons.transform.Find("NextStep"))
            FloatingButtons.transform.Find("NextStep").gameObject.SetActive(false);
        if(FloatingButtons.transform.Find("PreviousStep"))
            FloatingButtons.transform.Find("PreviousStep").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Correction").gameObject.SetActive(false);
    }


}
