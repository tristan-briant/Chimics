using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementManager : MonoBehaviour {

    public bool isSelected = false;
    public bool inReaction = false;
    public GameObject highlight;
    public GameObject react;
    public bool multiSelectable = false;
    public GameObject AbsorptionLine;
    public Arrow arrow;

    public void selectElement()
    {
        //if (inReaction) return; //Already part of a reaction

        bool s= isSelected;
        GameObject[] goSameType;

        if (!multiSelectable)
        {
            goSameType = GameObject.FindGameObjectsWithTag(gameObject.tag); // Find all GO with same tag

            foreach (GameObject go in goSameType) // and unselect them
            {
                ElementManager el = go.GetComponent<ElementManager>();
                if (el == null)
                    Debug.Log(go.name);
                else
                    //go.GetComponent<ElementManager>().isSelected = false;
                    go.GetComponent<ElementManager>().unSelectElement();
            }
        }

        isSelected = !s; //toggle selection
		GetComponent<Animator>().SetBool("selected", isSelected);
    }

	public void unSelectElement(){
		isSelected = false;

			GetComponent<Animator>().SetBool("selected", isSelected);
	}

    public void ReactWith(GameObject go)
    {
        react = go;
        inReaction = true;
        //unSelectElement();
        //isSelected = false;
    }

    private void Awake()
    {
        highlight = transform.Find("Highlight").gameObject;

        if(highlight.GetComponent<Canvas>()==null)
            highlight.AddComponent<Canvas>();

        Canvas canvas = highlight.GetComponent<Canvas>();
        highlight.SetActive(true); // mandatory to access overrideSorting !!
        canvas.overrideSorting=true;
        canvas.sortingOrder = -1;
        highlight.SetActive(false);
    }

    void Update () {
		
        //GetComponent<Animator>().SetBool("selected", isSelected);

    }


	void OnEnable(){
		if (isSelected)
		if (GetComponent<Animator> ().isActiveAndEnabled) {
			GetComponent<Animator> ().SetBool ("selected", true);
		}
	}

    public void reset()
    {
        isSelected = false;
		inReaction = false;
		if (GetComponent<Animator> ().isActiveAndEnabled) {
			GetComponent<Animator> ().SetTrigger ("reset");
			GetComponent<Animator> ().SetBool ("selected", false);
		}

        AbsorptionLine = null;
        if (transform.Find("Vibration")) 
            transform.Find("Vibration").gameObject.SetActive(false);
        //success = firstsuccess;
    }

    public void IdentifyAbsoptionLine(GameObject Line)
    {
        Debug.Log("identifier");
        AbsorptionLine = Line;
        Transform vib = transform.Find("Vibration");
        if (!vib) { Debug.Log(this); return; }

        if (Line)
        {
            vib.gameObject.SetActive(true);
            Color c = Line.GetComponent<LineSelectorManager>().color;
            vib.GetComponent<Image>().color = c;
        }
        else
        {
            vib.gameObject.SetActive(false);
        }

    }

}
