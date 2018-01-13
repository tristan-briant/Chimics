using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerArrow : GameController
{

    void Awake()
    {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Tips = transform.Find("Tips");

        Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        Controls = GameObject.FindGameObjectWithTag("Controls");
        ResetButton = GameObject.FindGameObjectWithTag("Reset");


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
        else if (halfWin)
        {
            StartCoroutine(WarningAnimation("Réaction incomplète"));
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

            ar.DrawCurvedArrow();

            liaison.GetComponent<ElementManager>().ReactWith(atome);
            liaison.GetComponent<ElementManager>().arrow = ar;

            atome.GetComponent<ElementManager>().ReactWith(liaison);
            atome.GetComponent<ElementManager>().arrow = ar;
            
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

        ResetButton.SetActive(false);
        Controls.SetActive(true);

        ClickableEnable();

    }

    override public void ClearLevel()
    {

        ResetElements(); // déselectionne les éléments

        foreach (Arrow it in transform.GetComponents<Arrow>())
        {
            it.Remove(0.1f);
        }


    }
}
