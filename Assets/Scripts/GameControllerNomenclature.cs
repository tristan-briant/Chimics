using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerNomenclature : GameController {

    public GameObject NameSelector;

    override public void LateUpdate()
    {
        string name="";

        foreach(Transform part in NameSelector.transform)
        {
            string st = "";
            foreach (Toggle tog in part.GetComponent<ToggleGroup>().ActiveToggles())
            {
                    st = tog.GetComponentInChildren<Text>().text;
            }
            name += st;
        }

        transform.Find("Name").Find("Text").GetComponent<Text>().text = name;
    }


    override public void Validate()
    {
        Transform sol = transform.Find("Solutions");

        bool test = false;
        foreach (Text solution in sol.GetComponentsInChildren<Text>()) {
            if (solution.text == transform.Find("Name").Find("Text").GetComponent<Text>().text)
                test = true;
        }
         
        if(test)
        {
            WinLevel();
        }
        else
        {
            failCount++;
            StartCoroutine(FailAnimation());
        }
    }

    override public IEnumerator FailAnimation()
    {
        transform.parent.parent.GetComponent<resize>().ReZoom();
        //ClickableDisable();
        ResetElements();
        transform.parent.parent.Find("Fail").GetComponent<Animator>().SetTrigger("FailTrigger");
        yield return new WaitForSeconds(1.5f);
        ClearLevel();
        //ClickableEnable();
        ShowTip();
    }

    override public void WinLevel()
    {
        Debug.Log("gagné");
        if (LVM.completedLevel < LVM.currentLevel + 1)
        {
            LVM.completedLevel = LVM.currentLevel + 1;
            Debug.Log("level+1");
        }

        transform.parent.parent.Find("Check").GetComponent<Animator>().SetTrigger("SuccessTrigger");

        transform.Find("Validate").gameObject.SetActive(false);
        //transform.Find("Next").gameObject.SetActive(true);
    }

    public override void ResetLevel()
    {
        base.ResetLevel();

        foreach (GameObject ob in Buttons)
        {
            ob.SetActive(false);
        }
        ResetButton.SetActive(false);
        transform.Find("Validate").gameObject.SetActive(true);
        transform.Find("Next").gameObject.SetActive(false);


        foreach (Transform part in NameSelector.transform)
        {

            bool allowSwitchOff = part.GetComponent<ToggleGroup>().allowSwitchOff;

            if (allowSwitchOff == true || part.GetComponentsInChildren<Toggle>().Length > 1)
            {
                part.GetComponent<ToggleGroup>().allowSwitchOff = true;
                foreach (Toggle tog in part.GetComponentsInChildren<Toggle>())
                {
                    tog.isOn = false;

                }
                part.GetComponent<ToggleGroup>().allowSwitchOff = allowSwitchOff;
            }


        }
    }


    public override void SetupLevel(bool notused)
    {
        base.SetupLevel(false);

        bool playable = !corrected;

        FloatingButtons = GameObject.FindGameObjectWithTag("Controls");
        FloatingButtons.SetActive(true);

        if (playable && training)
        {
            FloatingButtons.transform.Find("Clear").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);

            transform.Find("Validate").gameObject.SetActive(true);
            transform.Find("Next").gameObject.SetActive(false);
        }

        if (playable && !training)
        {
            FloatingButtons.transform.Find("Clear").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);

            transform.Find("Validate").gameObject.SetActive(false);
            transform.Find("Next").gameObject.SetActive(false);
        }
        if (!playable)
        {
            FloatingButtons.transform.Find("Clear").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);
        }

        if (debug)
        {
            FloatingButtons.transform.Find("Correction").gameObject.SetActive(true);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(true);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(true);
        }
        else
        {
            FloatingButtons.transform.Find("Correction").gameObject.SetActive(false);
        }

    }

}
