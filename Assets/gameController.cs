using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gameController : MonoBehaviour {
    List<GameObject> accepteurs = new List<GameObject>();
    List<GameObject> doublets = new List<GameObject>();
    public int failCount;
    public bool animPlaying = false;
    levelManager LVM;
    GameObject Tip;
    Animator anim;
    public GameObject canvas;

    void Start () {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        if (transform.Find("Tip") != null)
            Tip = transform.Find("Tip").gameObject;


        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);

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

        failCount = 0;
        ResetElements();
    }

	
	void LateUpdate () {

        //if (animPlaying == true) return;

        GameObject liaison=null, atome=null;

        foreach (GameObject go in doublets)
            if (go.GetComponent<ElementManager>().isSelected) liaison = go;

        foreach (GameObject go in accepteurs)
            if (go.GetComponent<ElementManager>().isSelected) atome = go;



        if (liaison && atome)
        {
            Arrow ar = gameObject.AddComponent<Arrow>();
            ar.atome = atome;
            ar.liaison = liaison;
            
            //ar.width = 0.02f * canvas.transform.GetComponent<Rect>().height/500;
            ar.DrawCurvedArrow();
            //ar.DrawCurvedArrowBetween(liaison.transform, atome.transform);


            /*Arrow arrow=new Arrow();
            arrow.DrawCurvedArrowBetween(liaison.transform, atome.transform,transform);*/
            liaison.GetComponent<ElementManager>().ReactWith(atome);
            atome.GetComponent<ElementManager>().ReactWith(liaison);

        }

    }

    public void Validate()
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

        Solutions[] solutions = sol.GetComponents<Solutions>();
        Debug.Log("nombre de sol " + solutions.Length);

        foreach (Solutions s in solutions)
        {
            if (s.TestReaction(acc, don) == 1)
                win = true;
        }

        if (win)
        {
            //WinLevel();
            //Debug.Log("gagné");
            GetComponent<Animator>().SetTrigger("successTrigger");
            return;
        }
        else
        {
            Debug.Log("perdu");
            //animPlaying = true;
            failCount++;
            //Lance le fail
            transform.parent.parent.Find("Fail").GetComponent<Animator>().SetTrigger("FailTrigger");
        }
    }



    public void WinLevel(){
        Debug.Log("gagné");
        if (LVM.completedLevel < LVM.currentLevel + 1)
            {
                LVM.completedLevel = LVM.currentLevel + 1;
                Debug.Log("level+1");
            }

        transform.parent.parent.Find("Check").GetComponent<Animator>().SetTrigger("SuccessTrigger");
    }

    public void ResetElements()
    {
        foreach (GameObject go in accepteurs)
        {
            go.GetComponent<ElementManager>().isSelected = false;
            go.GetComponent<ElementManager>().inReaction = false;
        }
        foreach (GameObject go in doublets)
        {
            go.GetComponent<ElementManager>().isSelected = false;
            go.GetComponent<ElementManager>().inReaction = false;
        }


    }

    public void ResetLevel() {
        ResetElements(); // déselectionne les éléments

        Arrow[] arrows = transform.GetComponents<Arrow>();

        foreach (Arrow it in arrows)
        {
            it.Remove(0.0f);
        }

        // si on load le level on enlève un éventuel check
        transform.parent.parent.Find("Check").GetComponent<Animator>().SetTrigger("reset");

    }

    public void ClearLevel()
    {
        ResetElements(); // déselectionne les éléments

        Arrow[] arrows = transform.GetComponents<Arrow>();

        foreach (Arrow it in arrows)
        {
            it.Remove(0.2f);
        }

    }


    public void ShowTip()
    {
        if (Tip!= null && failCount > 3)
            Tip.SetActive(true);
    }

    public void ClickableDisable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ClickableEnable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Debug.Log("click again");
    }

    /*public void PrepareNextSuccess()
    {
        foreach (GameObject go in accepteurs)
            if (go.activeInHierarchy)
                go.GetComponent<ElementManager>().success=false;
        foreach (GameObject go in doublets)
            if (go.activeInHierarchy)
                go.GetComponent<ElementManager>().success = false;

        failCount = 0;

        if(transform.Find("Tip2")!=null)
            Tip = transform.Find("Tip").gameObject;
        animPlaying = false;
    }

    public void stopWinAnimation()
    {
        anim.SetFloat("winspeed",0);
        animPlaying = false;
    }

    public void resumeWinAnimation()
    {
        anim.SetFloat("winspeed", 1);
    }*/

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LVM.Menu.SetActive(true);
            LVM.Level.SetActive(false);
        }
    }

   

}

