using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour {

    public bool selected=false;
    public bool successfull = false;

    public void resetMolecule() //unselect all
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<ElementManager>().isSelected = false;

        selected = false;
        successfull = false;
    }


}
