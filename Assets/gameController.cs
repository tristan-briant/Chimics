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
 

    // Use this for initialization
    void Start () {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();

        var rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        transform.GetComponent<Image>().enabled = false;

        accepteurs = GameObject.FindGameObjectsWithTag("Accepteur"); // Find all Accepteurs
        doublets = GameObject.FindGameObjectsWithTag("Doublet"); // Find all doublets

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
            //GetComponent<Animator>().SetBool("success", true);
            GetComponent<Animator>().SetTrigger("successTrigger");
            if (LVM.completedLevel < LVM.currentLevel+1)
            {
                LVM.completedLevel = LVM.currentLevel+1;
                Debug.Log("level+1");
            }
        }
        else if (accepteurSelected && doubletSelected) 
        {
            
            animPlaying = true;
            failCount++;
            GetComponent<Animator>().SetTrigger("failTrigger");

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
        
        //resetElements();
        foreach (GameObject go in accepteurs)
            go.GetComponent<ElementManager>().reset();
        foreach (GameObject go in doublets)
            go.GetComponent<ElementManager>().reset();

        failCount = 0;
        GetComponent<Animator>().SetTrigger("reset");
        GetComponent<Animator>().ResetTrigger("successTrigger");
        GetComponent<Animator>().ResetTrigger("failTrigger");
        //GetComponent<Animator>().SetBool("success", false);
        animPlaying = false;
    }


}
