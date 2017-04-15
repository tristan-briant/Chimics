using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {
    GameObject[] accepteurs;
    GameObject[] doublets;
    public int failCount;
    public bool animPlaying = false;
    // Transform molecule = transform.parent;

 

    // Use this for initialization
    void Start () {
        accepteurs = GameObject.FindGameObjectsWithTag("Accepteur"); // Find all Accepteurs
        doublets = GameObject.FindGameObjectsWithTag("Doublet"); // Find all doublets

        failCount = 0;
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
            GetComponent<Animator>().SetBool("success", true);
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

   


}
