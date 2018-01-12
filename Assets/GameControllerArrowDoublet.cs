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

    void UpdateArrow(GameObject acc) {
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


    override public void Validate()
    {
        Transform sol = transform.Find("Solutions");

        Arrow[] lm = transform.GetComponents<Arrow>();
        int length = lm.Length;

        GameObject[] acc = new GameObject[length];
        GameObject[] don = new GameObject[length]; ;

        int count = 0;
        foreach (Arrow iterator in lm)
        {
            acc[count] = iterator.atome;
            don[count++] = iterator.liaison;
        }

        bool win = false;
        bool halfWin = false;

        Solutions[] solutions = sol.GetComponents<Solutions>();
        Debug.Log("nombre de sol " + solutions.Length);

        foreach (Solutions s in solutions)
        {
            int test = s.TestReaction(acc, don);

            if (test == 1)
                win = true;

            if (test == -1)
                halfWin = true;
        }

        SolutionDoublets soldoub = sol.GetComponent<SolutionDoublets>();

        int score = soldoub.TestSolution();

        Debug.Log("Score  : " + score + " / " + soldoub.MaxScore());

        if (score < soldoub.MaxScore()) {
            win = false;
        }

        if (win)
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
            int percent = 10*Mathf.FloorToInt( 10.0f * (float)score / (float)(soldoub.MaxScore()));
            if (percent < 10)
                StartCoroutine(FailAnimation());
            else if (percent < 100)
                StartCoroutine(WarningAnimation("Doublets complétés à " + percent + "%"));
            else
                StartCoroutine(WarningAnimation("Réaction incomplète"));

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
}
