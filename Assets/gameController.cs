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

    void Start () {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        if (transform.FindChild("Tip") != null)
            Tip = transform.FindChild("Tip").gameObject;


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
        resetElements();
    }
	
	
	void LateUpdate () {

        if (animPlaying == true) return;

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

    
    /*public void resetLevel() {
       
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
        
    }*/


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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LVM.Menu.SetActive(true);
    }

}

