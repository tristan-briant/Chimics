using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerSpectreIR : GameController {

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
            if (child.CompareTag("Doublet"))
            {
                doublets.Add(child.gameObject);
                child.GetComponent<ElementManager>().multiSelectable = true;
            }
            if (child.CompareTag("Accepteur"))
            {
                if (child.GetComponent<Button>())
                    child.GetComponent<Button>().interactable = false;
            }

        }

    }


    override public void LateUpdate()
    {

        foreach (LineSelectorManager ls in transform.GetComponentsInChildren<LineSelectorManager>()) {
            if (ls.isSelected) 
            {
                foreach (GameObject go in doublets) {
                    ElementManager em = go.GetComponent<ElementManager>();
                    if (em.isSelected) {
                        if (em.AbsorptionLine != ls.gameObject)
                            em.IdentifyAbsoptionLine(ls.gameObject);
                        else
                            em.IdentifyAbsoptionLine(null);

                        em.unSelectElement();


                    }
                }
            }
        }

        
    }

    public void NameLine(GameObject btn)
    {
       
        foreach (LineSelectorManager ls in transform.GetComponentsInChildren<LineSelectorManager>()) {
            if (ls.isSelected)
            {
                ls.SetID(btn);
            }

        }

    }

    public void NameGroup(GameObject btn)
    {

        int selectedCount = 0;
        foreach (GameObject go in accepteurs)
            if (go.GetComponent<ElementManager>().isSelected) selectedCount++;

        if (selectedCount < 2) return;
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
            go.GetComponent<ElementManager>().ReactWith(btn);

    }

    override public void Validate()
    {
        Transform sol = transform.Find("Solutions");

        SolutionSpectre[] solutions = sol.GetComponents<SolutionSpectre>();

        /*******  Calcul du nombre d'item à trouver  ****/
        int SolutionCountTotal = 0;
        foreach (SolutionSpectre s in solutions)
        {
            SolutionCountTotal++; //identification du nom
            SolutionCountTotal += s.elements.Count; //identification des liaisons
        }

        /*******  Calcul du nombre d'item trouvé par le joueur  ****/

        int SolutionCountUser = 0;
        foreach (LineSelectorManager ls in transform.GetComponentsInChildren<LineSelectorManager>())
        {
            foreach (SolutionSpectre s in solutions)
            {
                if (ls.ID == s.LineName) SolutionCountUser++; //identification du nom
            }
        }

        foreach (GameObject go in doublets)
        {
            ElementManager em = go.GetComponent<ElementManager>();

            foreach (SolutionSpectre s in solutions)
            {
                if (em.AbsorptionLine == s.Line && s.elements.Contains(em.gameObject)) //identification des liaisons
                    SolutionCountUser++;

            }
        }

        foreach (GameObject go in doublets)
        {
            ElementManager em = go.GetComponent<ElementManager>();

            if (em.AbsorptionLine != null)
            {
                foreach (SolutionSpectre s in solutions)
                {
                    if (em.AbsorptionLine == s.Line && !s.elements.Contains(em.gameObject)) //identification des erreurs
                        SolutionCountUser--;
                }
            }
        }

        Debug.Log("Trouver : " + SolutionCountUser + " / " + SolutionCountTotal);

        if (SolutionCountUser < SolutionCountTotal)
        {
            int percent = Mathf.Clamp(10 * Mathf.FloorToInt(10 * SolutionCountUser / SolutionCountTotal), 0, 100);
            StartCoroutine(WarningAnimation("Complété à " + percent + "%"));
        }
        else
        {
            WinLevel();
        }

    }

    /*override public IEnumerator FailAnimation()
    {
        transform.parent.parent.GetComponent<resize>().ReZoom();
        ClickableDisable();
        ResetElements();
        transform.parent.parent.Find("Fail").GetComponent<Animator>().SetTrigger("FailTrigger");
        yield return new WaitForSeconds(1.5f);
        ClearLevel();
        ClickableEnable();
        ShowTip();
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

        transform.Find("Validate").gameObject.SetActive(false);
        transform.Find("Next").gameObject.SetActive(true);
    }


    override public void ResetLevel()
    {


        if (transform.parent)
        {
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

        }
        ResetElements();

        foreach (LineSelectorManager ls in transform.GetComponentsInChildren<LineSelectorManager>())
            ls.ResetLineSelector();

        Groupe[] groupes = transform.GetComponents<Groupe>();

        foreach (Groupe it in groupes)
        {
            it.Remove(0.0f);
        }

        failCount = 0;
        step = 0;

        ShowTip();
        ResetButton.SetActive(false);

        Controls.SetActive(false);

        if (gameObject.name.Contains("Tuto"))
        {
            foreach (GameObject ob in Buttons)
            {
                ob.SetActive(false);
            }
        }
        else
        {
            ClickableEnable();
        }

        transform.Find("Validate").gameObject.SetActive(true);
        transform.Find("Next").gameObject.SetActive(false);

    }


    override public void ClearLevel()
    {
        Debug.Log("Clear Level");
        ResetElements(); // déselectionne les éléments

        Groupe[] groupes = transform.GetComponents<Groupe>();

        foreach (Groupe it in groupes)
        {
            it.Remove(0.2f);
        }

    }

    override public void ResetElements()
    {
        foreach (GameObject go in accepteurs)
        {
            go.GetComponent<ElementManager>().reset();
        }
        foreach (GameObject go in doublets)
        {
            go.GetComponent<ElementManager>().reset();

        }
    }
}
