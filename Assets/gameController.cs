using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gameController : MonoBehaviour {
    GameObject[] accepteurs;
    GameObject[] doublets;
    public int failCount;
    public bool animPlaying = false;
    // Transform molecule = transform.parent;
    levelManager LVM;
    GameObject Tip;
    Animator anim;

    // Use this for initialization
    void Start () {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        if (transform.FindChild("Tip") != null)
            Tip = transform.FindChild("Tip").gameObject;

        var rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        transform.GetComponent<Image>().enabled = false;
        anim = GetComponent<Animator>();

        accepteurs = GameObject.FindGameObjectsWithTag("Accepteur"); // Find all Accepteurs
        doublets = GameObject.FindGameObjectsWithTag("Doublet"); // Find all doublets

        failCount = 0;
        resetElements();
    }
	
	
	void LateUpdate () {

        if (animPlaying == true) return;
        //GetComponent<Animator>().ResetTrigger("successTrigger");

        bool accepteurSelected, doubletSelected, accepteurSuccess, doubletSuccess;

        accepteurSelected = false;
        accepteurSuccess = false;

        foreach (GameObject go in accepteurs) 
        {
            if (go.GetComponent<ElementManager>().isSelected)
            {
                accepteurSelected=true;
                if (go.GetComponent<ElementManager>().success)
                    accepteurSuccess = true;
            }
        }

        doubletSelected = false;
        doubletSuccess = false;

        foreach (GameObject go in doublets) 
        {
            if (go.GetComponent<ElementManager>().isSelected)
            {
                doubletSelected = true;
                if (go.GetComponent<ElementManager>().success)
                    doubletSuccess = true;
            }
        }

        if (doubletSuccess && accepteurSuccess)
        {
            animPlaying = true;
            resumeWinAnimation();
            GetComponent<Animator>().SetTrigger("successTrigger");
           
        }
        else if (accepteurSelected && doubletSelected)
        {
            animPlaying = true;
            failCount++;
            GetComponent<Animator>().SetTrigger("failTrigger");

        }
    }

    public void WinLevel(){
        if (LVM.completedLevel < LVM.currentLevel + 1)
            {
                LVM.completedLevel = LVM.currentLevel + 1;
                Debug.Log("level+1");
            }

    }

    public void resetElements()
    {
        foreach (GameObject go in accepteurs)
            go.GetComponent<ElementManager>().isSelected = false;
        foreach (GameObject go in doublets)
            go.GetComponent<ElementManager>().isSelected = false;
    }

    
    public void resetLevel() {
       
        resetElements();
        foreach (GameObject go in accepteurs)
            if(go.activeInHierarchy)
                go.GetComponent<ElementManager>().reset();
        foreach (GameObject go in doublets)
            if (go.activeInHierarchy)
                go.GetComponent<ElementManager>().reset();

        failCount = 0;
        GetComponent<Animator>().SetTrigger("reset");
        GetComponent<Animator>().ResetTrigger("successTrigger");
        GetComponent<Animator>().ResetTrigger("failTrigger");
        animPlaying = false;
        Tip.SetActive(false);
        ClickableEnable();
        
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

    public void PrepareNextSuccess()
    {
        foreach (GameObject go in accepteurs)
            if (go.activeInHierarchy)
                go.GetComponent<ElementManager>().success=false;
        foreach (GameObject go in doublets)
            if (go.activeInHierarchy)
                go.GetComponent<ElementManager>().success = false;

        failCount = 0;

        if(transform.FindChild("Tip2")!=null)
            Tip = transform.FindChild("Tip").gameObject;
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
    }
}
