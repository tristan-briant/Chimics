using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerArrowDoublet : GameControllerArrow {

    int MaxDoublets = 4;
    GameObject AtomeChanged = null;

    public GameObject DoubletButton;
    public GameObject ArrowButton;


    public void AddDoublet(GameObject acc)
    {
        
        if (acc.GetComponents<Doublet>().Length >= MaxDoublets) return; // Already to many doublets, Stop adding 

        Doublet d = acc.AddComponent<Doublet>();
        d.Atome = acc;
        d.DrawDoublet();
        doublets.Add(d.doublet);

        UpdateArrow(acc);
        
    }

    public void UpdateArrow(GameObject acc) {
        if (acc)
        {
            foreach (ElementManager em in acc.GetComponentsInChildren<ElementManager>())
            {
                if ( em.gameObject == acc) continue; // Pas l'atome lui même, seuls ses doublets sup
                if (!em.inReaction ) continue;
                Arrow arrow = em.arrow;

                Arrow ar = gameObject.AddComponent<Arrow>();
                ar.atome = arrow.atome;
                ar.liaison = arrow.liaison;

                ar.DrawCurvedArrow();
                arrow.liaison.GetComponent<ElementManager>().arrow = ar;
                arrow.atome.GetComponent<ElementManager>().arrow = ar;

                arrow.Remove();
            }
        }
    }

    override public void LateUpdate()
    {
       
        if (DoubletButton.GetComponent<Toggle>().isOn)
        {
            foreach (GameObject go in accepteurs)
                if (go.GetComponent<ElementManager>().isSelected)
                {
                    AddDoublet(go);
                    go.GetComponent<ElementManager>().unSelectElement();
                }

            foreach (GameObject go in doublets)
                if (go.GetComponent<ElementManager>().isSelected)
                {
                    if (go.name == "Doublet Sup")
                    {
                        RemoveThisDoublet(go);
                        break; // la collection de doublet est modifié il faut s'arréter 
                    }
                    else go.GetComponent<ElementManager>().unSelectElement();
                }
        }


        base.LateUpdate();

    }

    public void RemoveThisDoublet(GameObject db) {

        Doublet[] ds = db.transform.parent.GetComponents<Doublet>();

        foreach(Doublet d in ds)
        {
            if (d.doublet == db)
            {
                doublets.Remove(d.doublet);
                d.Remove();
            }
            UpdateArrow(db.transform.parent.gameObject);
        }

    }

    public void RemoveDoublet(GameObject acc)
    {
        
        Doublet[] ds = acc.GetComponents<Doublet>();
        if (ds.Length > 0)
        {
            Doublet d = ds[ds.Length - 1];
            doublets.Remove(d.doublet);
            d.Remove();

        }

        UpdateArrow(acc);

    }

    public override int Score()
    {
        Transform sol = transform.Find("Solutions");
        Arrow[] arrows = transform.GetComponents<Arrow>();

        int score = 0;

        int test = -1;

        foreach (Solutions s in sol.transform.GetComponents<Solutions>())
        {
           
            int test0 = s.TestReaction(arrows);
            if (test0 > test)
            {
                test = test0;
            }
        }

        score = test;

 
        SolutionDoublets soldoub = sol.GetComponent<SolutionDoublets>();

        score += (100*soldoub.TestSolution())/ soldoub.MaxScore();

        Debug.Log("Score  : " + (score/2) );

        return score / 2;
    }

    override public void Validate()
    {
       
        if(Score()==100)
        {
            RemoveTip();
            failCount = 0;
            step++;
            transform.parent.parent.GetComponent<resize>().ReZoom();
            ClickableDisable();
            ResetElements();

            Animator anim = GetComponent<Animator>();
            if (anim != null && anim.enabled)
            {
                ClickableDisable();
                ClearLevel();
                anim.SetTrigger("SuccessTrigger");
            }
            else
                WinLevel();
        }
        else
        {
            failCount++;
            int percent = Score();
            if (percent < 10)
                StartCoroutine(FailAnimation());
            else if (percent < 100)
                StartCoroutine(WarningAnimation("Doublets complétés à " + percent + "%"));
            else
                StartCoroutine(WarningAnimation("Réaction incomplète"));

        }
    }


    public override void ShowCorrection()
    {
        SolutionDoublets soldoub = transform.GetComponentInChildren<SolutionDoublets>();
        soldoub.CorrectDoublet();

        Solutions[] solutions = transform.Find("Solutions").GetComponents<Solutions>();

        Solutions bestSolution;


        for (int st = 0; st <= stepNumber; st++) // Donne la correction de l'étape st
        {
            List<Arrow> arrows = new List<Arrow>();

            foreach (Arrow ar in transform.GetComponents<Arrow>())  // On sélectionne uniquement les flèches de l'étape
            {
                if (ar.step == st)
                {
                    arrows.Add(ar);
                }
            }

            int test = -1;
            bestSolution = null;

            foreach (Solutions s in solutions)
            {
                if (s.step != st) continue;

                int test0 = s.TestReaction(arrows.ToArray());
                if (test0 > test)
                {
                    test = test0;
                    bestSolution = s;
                }
            }

            foreach (Arrow ar in arrows)
            {
                if (bestSolution.TestOneArrow(ar))
                    ar.SetGood();
                else
                    ar.SetWrong();
            }

            bestSolution.CompleteReaction(arrows);

        }


    }

    override public void SetupLevel(bool notused)
    {
        base.SetupLevel(true);
        bool playable = !corrected;

        FloatingButtons = GameObject.FindGameObjectWithTag("Controls");
        if (playable && training)
        {
            FloatingButtons.transform.Find("Clear").gameObject.SetActive(true);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(true);
        }

        if (playable && !training)
        {
            FloatingButtons.transform.Find("Clear").gameObject.SetActive(true);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);
        }
        if (!playable)
        {
            FloatingButtons.transform.Find("Clear").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);
        }

        if (stepNumber > 0)
        {
            anim.SetTrigger("reset");
            FloatingButtons.transform.Find("NextStep").gameObject.SetActive(true);
            FloatingButtons.transform.Find("PreviousStep").gameObject.SetActive(false);
        }
        else
        {
            FloatingButtons.transform.Find("NextStep").gameObject.SetActive(false);
            FloatingButtons.transform.Find("PreviousStep").gameObject.SetActive(false);
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


    override public void ClearLevel()
    {
        base.ClearLevel();

        foreach (GameObject acc in GameObject.FindGameObjectsWithTag("Accepteur"))
        {
            Doublet[] doublets = acc.transform.GetComponents<Doublet>();
            foreach (Doublet d in doublets)
                RemoveThisDoublet(d.doublet);
        }
    }

    bool IsElement(GameObject go, GameObject[] array)
    {
        foreach (GameObject iterator in array)
        {
            if (go == iterator) return true;
        }
        return false;
    }

}
