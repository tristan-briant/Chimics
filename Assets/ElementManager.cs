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

        Transform molecule = transform.parent;
        for (int i = 0; i < molecule.childCount; i++)
        {
            if (molecule.GetChild(i).GetComponent<ElementManager>()!=null)
                molecule.GetChild(i).GetComponent<ElementManager>().isSelected = false;
        }

        isSelected = !s; //toggle selection

        molecule.GetComponent<MoleculeManager>().selected = isSelected;
        molecule.GetComponent<MoleculeManager>().successfull = isSelected && success;
    }

    private void Start()
    {
        highlight = transform.FindChild("Highlight").gameObject;
        if (highlight.activeInHierarchy)
            success = true;
        else
            success = false;

    }

    void Update () {
            highlight.SetActive(isSelected);
    }
}
