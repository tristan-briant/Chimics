using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour {

    public bool success;
    public int failCount = 0;
    GameObject molecule1, molecule2, tip;

	// Use this for initialization
	void Start () {
        var rect=GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);

        molecule1 = transform.FindChild("Molecule1").gameObject;
        molecule2 = transform.FindChild("Molecule2").gameObject;
        tip = transform.FindChild("Molecule2").gameObject;

    }




    // Update is called once per frame
    void Update () {
       
        success = true;
        bool fail = true;

        /*for (int i = 0; i < transform.childCount; i++) {
            fail &= transform.GetChild(i).GetComponent<MoleculeManager>().selected;
            success &= transform.GetChild(i).GetComponent<MoleculeManager>().successfull;
        }*/

        fail &= molecule1.GetComponent<MoleculeManager>().selected;
        success &= molecule1.GetComponent<MoleculeManager>().successfull;
        fail &= molecule2.GetComponent<MoleculeManager>().selected;
        success &= molecule2.GetComponent<MoleculeManager>().successfull;


        if (success)
        {
            //Do something
        }
        else if (fail)
        {
            failCount++;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MoleculeManager>().resetMolecule();
            }
        }

	}
}
