using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerNomenclature : gameController {

    public GameObject NameSelector;

    override public void LateUpdate()
    {
        string name="";

        foreach(Transform part in NameSelector.transform)
        {
            string st = "-????-";
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
        transform.Find("Next").gameObject.SetActive(true);
    }

    public override void ResetLevel()
    {
        base.ResetLevel();

        ResetButton.SetActive(false);

        Controls.SetActive(false);

        transform.Find("Validate").gameObject.SetActive(true);
        transform.Find("Next").gameObject.SetActive(false);



       
        Transform t = transform.parent.parent.Find("Check");
        Animator a = t.GetComponent<Animator>();
        if (a.isActiveAndEnabled) a.SetTrigger("reset");

        t = transform.parent.parent.Find("Fail");
        a = t.GetComponent<Animator>();
        if (a.isActiveAndEnabled) a.SetTrigger("reset");

        t = transform.parent.parent.Find("Warning");
        a = t.GetComponent<Animator>();
        if (a.isActiveAndEnabled) a.SetTrigger("reset");

        transform.parent.parent.GetComponent<resize>().InitResize(transform);

        

        foreach (Transform part in NameSelector.transform)
        {
            if (part.childCount > 1)
            {
                part.GetComponent<ToggleGroup>().allowSwitchOff = true;
                foreach (Toggle tog in part.GetComponentsInChildren<Toggle>()) 
                {
                    tog.isOn = false;
                    Debug.Log("reset");
                }
                part.GetComponent<ToggleGroup>().allowSwitchOff = false;
            }
            else
            {
                part.GetComponentInChildren<Toggle>().isOn = true;
                Debug.Log("reset1");
            }
        }
    }


}
