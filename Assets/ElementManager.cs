using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour {

    public bool isSelected = false;
    public bool success = false;
    public GameObject highlight;

    public void selectElement()
    {
        bool s= isSelected;
        GameObject[] goSameType;

        goSameType = GameObject.FindGameObjectsWithTag(gameObject.tag); // Find all GO with same tag

        foreach (GameObject go in goSameType) // and unselect them
        {
            go.GetComponent<ElementManager>().isSelected = false;
        }

        isSelected = !s; //toggle selection

        
    }

    private void Start()
    {
        highlight = transform.FindChild("Highlight").gameObject;

        if (highlight.activeInHierarchy)
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
    }


}
