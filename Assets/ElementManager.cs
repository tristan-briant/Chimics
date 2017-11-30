using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour {

    public bool isSelected = false;
    public bool inReaction = false;
    public bool success = false;
    public bool firstsuccess = false; // used to reset to initial state for multiple step reactions
    public GameObject highlight;
    public GameObject react;

    public void selectElement()
    {
        if (inReaction) return; //Already part of a reaction

        bool s= isSelected;
        GameObject[] goSameType;

        goSameType = GameObject.FindGameObjectsWithTag(gameObject.tag); // Find all GO with same tag

        foreach (GameObject go in goSameType) // and unselect them
        {
            go.GetComponent<ElementManager>().isSelected = false;
        }

        isSelected = !s; //toggle selection

    }

    public void ReactWith(GameObject go){
        react = go;
        inReaction = true;
        isSelected = false;
    }

    private void Start()
    {
        highlight = transform.Find("Highlight").gameObject;

        if (highlight.activeInHierarchy || firstsuccess)
            success = true;

        highlight.SetActive(false);
        isSelected = false;
    }

    void Update () {
        //highlight.SetActive(isSelected);
        GetComponent<Animator>().SetBool("selected", isSelected);

    }

    public void reset()
    {
        GetComponent<Animator>().SetBool("selected", false);
        isSelected = false;
        GetComponent<Animator>().SetTrigger("reset");
        success = firstsuccess;
    }


}
