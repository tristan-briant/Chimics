using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerArrow : GameController
{



    override public void Awake()
    {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Tips = transform.Find("Tips");

        //Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        //Controls = GameObject.FindGameObjectWithTag("Controls");
        //ResetButton = GameObject.FindGameObjectWithTag("Reset");
        //bool gameActive = LVM.Game.activeSelf;
        //LVM.Game.SetActive(true);
        //FloatingButtons = GameObject.FindGameObjectWithTag("Controls");
        FloatingButtons = LVM.Game.transform.Find("FloatingButtons").gameObject;
        //LVM.Game.SetActive(gameActive);

        transform.localPosition = new Vector3(0, 0, 0);

        transform.GetComponent<Image>().enabled = false;
        anim = GetComponent<Animator>();

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.CompareTag("Accepteur"))
                accepteurs.Add(child.gameObject);

            if (child.CompareTag("Doublet"))
                doublets.Add(child.gameObject);

        }

        // Enlève le texte pour ne pas être visible sur le jeu
        Transform sol = transform.Find("Solutions");
        if (sol.GetComponent<Text>())
            sol.GetComponent<Text>().enabled = false;


    }

    public override int Score()
    {
        Transform sol = transform.Find("Solutions");
        int score = 0;

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
           

            foreach (Solutions s in sol.transform.GetComponents<Solutions>())
            {
                if (s.step != st) continue;

                int test0 = s.TestReaction(arrows.ToArray());
                if (test0 > test)
                {
                    test = test0;
                 }
            }

            score += test;

        }

        return score/ (stepNumber+1);
        
    }


    override public void Validate()
    {
        Transform sol = transform.Find("Solutions");

        Arrow[] lm = transform.GetComponents<Arrow>();
       
        int score = 0;
        foreach (Solutions s in sol.GetComponents<Solutions>())
        {
            int test = s.TestReaction(lm);

            if (test > score)
                score = test;
        }

        if (score == 100)
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
        else if (score > 0) 
        {
            StartCoroutine(WarningAnimation("Score : " + score + "%"));
        }
        else
        {
            failCount++;
            StartCoroutine(FailAnimation());
        }
    }


    override public void LateUpdate()
    {

        GameObject liaison = null, atome = null;

        foreach (GameObject go in doublets)
            if (go.GetComponent<ElementManager>().isSelected)
            {
                if (!go.GetComponent<ElementManager>().inReaction) liaison = go;
                else go.GetComponent<ElementManager>().unSelectElement();
            }

        foreach (GameObject go in accepteurs)
            if (go.GetComponent<ElementManager>().isSelected)
            {
                if (!go.GetComponent<ElementManager>().inReaction) atome = go;
                else go.GetComponent<ElementManager>().unSelectElement();
            }


        if (liaison && atome)
        {
            Arrow ar = gameObject.AddComponent<Arrow>();
            atome.GetComponent<ElementManager>().unSelectElement();
            liaison.GetComponent<ElementManager>().unSelectElement();

            ar.atome = atome;
            ar.liaison = liaison;
            ar.step = step;

            ar.DrawCurvedArrow();

            liaison.GetComponent<ElementManager>().ReactWith(atome);
            liaison.GetComponent<ElementManager>().arrow = ar;

            atome.GetComponent<ElementManager>().ReactWith(liaison);
            atome.GetComponent<ElementManager>().arrow = ar;
            
        }


        if (stepNumber > 0)
        {
            if(step>0) FloatingButtons.transform.Find("PreviousStep").gameObject.SetActive(true);
            else FloatingButtons.transform.Find("PreviousStep").gameObject.SetActive(false);

            if(step<stepNumber) FloatingButtons.transform.Find("NextStep").gameObject.SetActive(true);
            else FloatingButtons.transform.Find("NextStep").gameObject.SetActive(false);

        }
    }

    public override void ShowCorrection()
    {

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

            foreach(Arrow ar in arrows){
                if (bestSolution.TestOneArrow(ar))
                    ar.SetGood();
                else
                    ar.SetWrong();
            }

            bestSolution.CompleteReaction(arrows);
            
        }
    }

    override public void ResetElements()
    {
       
        foreach (ElementManager em in gameObject.GetComponentsInChildren<ElementManager>())
        {
           em.reset();
        }
    }



    override public void ResetLevel()
    {
        Debug.Log("reset");
        base.ResetLevel();

        foreach (Arrow it in transform.GetComponents<Arrow>())
        {
            it.Remove(0.0f);
        }

        ResetElements();

        SetupLevel(true);

    }


    override public void SetupLevel(bool notused)
    {
        base.SetupLevel(true);
        bool playable = !corrected;

        FloatingButtons.SetActive(true);

        if (playable && training)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            FloatingButtons.transform.Find("Clear").gameObject.SetActive(true);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(true);
        }

        if (playable &&  !training)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            FloatingButtons.transform.Find("Clear").gameObject.SetActive(true);
            FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
            FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);
        }
        if (!playable) {
            GetComponent<CanvasGroup>().blocksRaycasts = false;

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
        ResetElements(); // déselectionne les éléments

        foreach (Arrow it in transform.GetComponents<Arrow>())
        {
            it.Remove(0.1f);
        }
        
    }

    Time timeInhib;
    public void NextStep()
    {
        anim.SetBool("Previous", false);
        if (anim.GetBool("Next")) return; // already next
        if (step < stepNumber)
        {
            step++;
            anim.SetTrigger("Next");
        }
    }

    public void PreviousStep()
    {
        anim.SetBool("Next", false);
        if (anim.GetBool("Previous")) return; // already next
        if (step >0)
        {
            step--;
            anim.SetTrigger("Previous");
        }
    }

}
