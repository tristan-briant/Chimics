using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerArrowDoublet : GameControllerArrow {

    int MaxDoublets = 4;
    GameObject AtomeChanged = null;

    public void  AddDoublet() {
        
        foreach(GameObject acc in accepteurs)
        {
            if (acc.GetComponent<ElementManager>().isSelected) {

                /*int count = 0;
                foreach (Doublet doub in acc.GetComponents<Doublet>())
                    if (doub.Atome == acc) count++;*/

                if (acc.GetComponents<Doublet>().Length >= MaxDoublets) break; // Already to many doublets, Stop adding 

                Doublet d = acc.AddComponent<Doublet>();
                d.Atome = acc;   
                d.DrawDoublet();
                doublets.Add(d.doublet);

                AtomeChanged = acc;

            }
        }

        foreach (Arrow ar in transform.GetComponents<Arrow>()) {
            ar.FadeIn(0.2f);
            ar.DrawCurvedArrow();
        }

    }

    override public void LateUpdate()
    {
        if (AtomeChanged)
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

        }
        base.LateUpdate();

    }

    public void RemoveDoublet()
    {

        foreach (GameObject acc in accepteurs)
        {
            if (acc.GetComponent<ElementManager>().isSelected)
            {
                Doublet[] ds = acc.GetComponents<Doublet>();
                if (ds.Length > 0)
                {
                    Doublet d = ds[ds.Length - 1];
                    doublets.Remove(d.doublet);
                    d.Remove();

                    AtomeChanged = acc;
                }
            }
        }

        foreach (Arrow ar in transform.GetComponents<Arrow>())
        {
            if (!ar.toBeRemoved)
            {
                ar.FadeIn(0.2f);
                ar.DrawCurvedArrow();
            }
        }

    }
}
