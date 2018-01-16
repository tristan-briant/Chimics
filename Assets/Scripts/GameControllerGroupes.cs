using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerGroupes : GameController {

    public int solution = 0;

    bool started = false;
    public GameObject BtnValidate;
  
    public void Awake()
    {
        transform.localPosition = new Vector3(0, 0, 0);

        transform.GetComponent<Image>().enabled = false;

        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Controls = GameObject.FindGameObjectWithTag("Controls");
        Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        ResetButton = GameObject.FindGameObjectWithTag("Reset");


        Transform[] children = GetComponentsInChildren<Transform>();

        Transform sol = transform.Find("Solutions");
        if (sol.GetComponent<Text>())
            sol.GetComponent<Text>().enabled = false;


        foreach (Transform child in children)
        {
            if (child.CompareTag("Accepteur"))
            {
                accepteurs.Add(child.gameObject);
                child.GetComponent<ElementManager>().multiSelectable = true;
            }
            if (child.CompareTag("Doublet"))
            {
                /*if(child.GetComponent<Button>())
                    child.GetComponent<Button>().interactable = false;*/
                CanvasGroup canvas = child.gameObject.AddComponent<CanvasGroup>();
                canvas.blocksRaycasts = false;
            }

        }

    }


    public override void LateUpdate()
    {
        
        foreach(ElementManager em in transform.GetComponentsInChildren<ElementManager>())
        {
            if (em.inReaction && em.isSelected)
                em.unSelectElement();
        }


    }


    public void NameGroup(GameObject btn) {

        int selectedCount = 0;
        foreach (GameObject go in accepteurs)
            if (go.GetComponent<ElementManager>().isSelected) selectedCount++;

        if (selectedCount < 1) return;
        GameObject[] listElements = new GameObject[selectedCount];

        selectedCount = 0;
        foreach (GameObject go in accepteurs)
            if (go.GetComponent<ElementManager>().isSelected) listElements[selectedCount++] = go;


        Groupe gr = gameObject.AddComponent<Groupe>();
        gr.elements = listElements;
        gr.groupName = btn;
        gr.color = btn.GetComponent<Image>().color;
        gr.DrawGroupe();

        foreach (GameObject go in listElements)
        {
            go.GetComponent<ElementManager>().ReactWith(btn);
            go.GetComponent<ElementManager>().unSelectElement();
        }

    }

    override public void Validate()
    {
        Transform sol = transform.Find("Solutions");

        SolutionGroupes [] solutions = sol.GetComponents<SolutionGroupes>();
        Groupe[] grs = transform.GetComponents<Groupe>();

        bool test = false;

        if (grs.Length == solutions.Length) {

            test = true;

            foreach (Groupe g in grs) {
                bool groupeOk = false;

                foreach(SolutionGroupes s in solutions)
                {
                    if (s.TestGroupes(g) == 1)
                        groupeOk = true;
                }

                if (!groupeOk) test = false;
            }

        }

        if (test)
        {
            WinLevel();
        }
        else
        {
            failCount++;
            StartCoroutine(FailAnimation());
        }
    }

    IEnumerator WarningAnimation()
    {
        transform.parent.parent.GetComponent<resize>().ReZoom();
        ClickableDisable();
        ResetElements();
        transform.parent.parent.Find("Warning").GetComponent<Animator>().SetTrigger("FailTrigger");
        yield return new WaitForSeconds(1.0f);
        ClickableEnable();
        ShowTip();
    }

    /*override public IEnumerator FailAnimation()
    {
        transform.parent.parent.GetComponent<resize>().ReZoom();
        ClickableDisable();
        ResetElements();
        transform.parent.parent.Find("Fail").GetComponent<Animator>().SetTrigger("FailTrigger");
        yield return new WaitForSeconds(1.5f);
        //ClearLevel();
        ClickableEnable();
        ShowTip();
    }*/

    /*IEnumerator FailAnimation()
    {
        //BtnValidate.SetActive(false);

        transform.parent.parent.GetComponent<resize>().ReZoom();
        transform.parent.parent.Find("Fail").GetComponent<Animator>().SetTrigger("FailTrigger");
        yield return new WaitForSeconds(1.5f);
        BtnValidate.SetActive(true);
        //Selector.ValidateChoice(false);


        //ShowTip();
    }*/

    override public void WinLevel()
    {
        //BtnValidate.SetActive(false);

        Debug.Log("gagné");
        if (LVM.completedLevel < LVM.currentLevel + 1)
        {
            LVM.completedLevel = LVM.currentLevel + 1;
            Debug.Log("level+1");
        }

        transform.parent.parent.Find("Check").GetComponent<Animator>().SetTrigger("SuccessTrigger");
    }


    override public void ResetLevel()
    {
        base.ResetLevel();

        Groupe[] groupes = transform.GetComponents<Groupe>();

        foreach (Groupe it in groupes)
        {
            it.Remove(0.0f);
        }

        foreach (GameObject ob in Buttons)
        {
            ob.SetActive(true);
        }
        ResetButton.SetActive(false);

        //Controls.SetActive(true);
    }


    override public void ClearLevel()
    {
        //Debug.Log("Clear Level");
        ResetElements(); // déselectionne les éléments

        Groupe[] groupes = transform.GetComponents<Groupe>();

        /*if(groupes.Length>0)
            groupes[groupes.Length-1].Remove(0.2f);*/
        foreach (Groupe it in groupes)
        {
            it.Remove(0.2f);
        }

    }
}
