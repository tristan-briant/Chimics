using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerArrowDoublet : GameControllerArrow {

    int MaxDoublets = 4;
    GameObject AtomeChanged = null;

    public GameObject addButton;
    public GameObject removeButton;


    public void  AddDoublet(GameObject acc) {
        
        /*foreach(GameObject acc in accepteurs)
        {
            if (acc.GetComponent<ElementManager>().isSelected) {*/

                if (acc.GetComponents<Doublet>().Length >= MaxDoublets) return; // Already to many doublets, Stop adding 

                Doublet d = acc.AddComponent<Doublet>();
                d.Atome = acc;   
                d.DrawDoublet();
                doublets.Add(d.doublet);

                //AtomeChanged = acc;

            //}
        //}

        UpdateArrow(acc);
        

    }

    void UpdateArrow(GameObject acc) {
        if (acc)
        {
            foreach (ElementManager em in acc.GetComponentsInChildren<ElementManager>())
            {
                if (!em.inReaction) continue;
                Arrow arrow = em.arrow;

                Arrow ar = gameObject.AddComponent<Arrow>();
                ar.atome = arrow.atome;
                ar.liaison = arrow.liaison;

                ar.DrawCurvedArrow();
                arrow.liaison.GetComponent<ElementManager>().arrow = ar;
                arrow.atome.GetComponent<ElementManager>().arrow = ar;

                arrow.Remove();
            }
        }
    }

    override public void LateUpdate()
    {
        /*if (AtomeChanged)
        {
            foreach (GameObject go in doublets)
                if (go.GetComponent<ElementManager>().isSelected)
                {
                    AtomeChanged.GetComponent<ElementManager>().unSelectElement();
                    AtomeChanged = null;
                }

            foreach (GameObject go in accepteurs)
                if (go.GetComponent<ElementManager>().isSelected && go != AtomeChanged)
                {
                    AtomeChanged.GetComponent<ElementManager>().unSelectElement();
                    AtomeChanged = null;
                }

        }*/

        if (addButton.GetComponent<Toggle>().isOn)
        {
            foreach (GameObject go in accepteurs)
                if (go.GetComponent<ElementManager>().isSelected)
                {
                    AddDoublet(go);
                    go.GetComponent<ElementManager>().unSelectElement();
                }
            foreach (GameObject go in doublets)
                go.GetComponent<ElementManager>().unSelectElement();

        }
        else if (removeButton.GetComponent<Toggle>().isOn)
        {
            foreach (GameObject go in accepteurs)
                if (go.GetComponent<ElementManager>().isSelected)
                {
                    RemoveDoublet(go);
                    go.GetComponent<ElementManager>().unSelectElement();
                }
            foreach (GameObject go in doublets)
            {
                if (go.GetComponent<ElementManager>().isSelected)
                {
                    if (go.name == "Doublet Sup")
                    {
                        RemoveThisDoublet(go);
                        break; // la collection de doublet est modifié il faut s'arréter 
                    }
                    else go.GetComponent<ElementManager>().unSelectElement();
                }
            }

        }


        base.LateUpdate();

    }

    public void RemoveThisDoublet(GameObject db) {

        Doublet[] ds = db.transform.parent.GetComponents<Doublet>();

        foreach(Doublet d in ds)
        {
            if (d.doublet == db)
            {
                doublets.Remove(d.doublet);
                d.Remove();
            }
            UpdateArrow(db.transform.parent.gameObject);
        }

    }

    public void RemoveDoublet(GameObject acc)
    {

        /*foreach (GameObject acc in accepteurs)
        {
            if (acc.GetComponent<ElementManager>().isSelected)
            {*/
                Doublet[] ds = acc.GetComponents<Doublet>();
                if (ds.Length > 0)
                {
                    Doublet d = ds[ds.Length - 1];
                    doublets.Remove(d.doublet);
                    d.Remove();

                    AtomeChanged = acc;
                //}
            //}
        }

        UpdateArrow(acc);

    }
}
