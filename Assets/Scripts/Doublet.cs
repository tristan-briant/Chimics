using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Doublet : MonoBehaviour {

    public GameObject  Atome;
    public GameObject doublet;

    public float distance = 40; // distance from element

    public int NumberOfDoublet() {

        List<Doublet> doublets = new List<Doublet>();

        foreach (Doublet d in transform.GetComponents<Doublet>())
        {
            if (d.Atome == Atome)
                doublets.Add(d);
        }
        return doublets.Count;
    }

    public void DrawDoublet()
    {
        Debug.Log("nouveau doublet");
        doublet = Instantiate(Resources.Load("Doublet")) as GameObject;

        doublet.transform.SetParent(Atome.transform);
        doublet.transform.localScale = Vector3.one;

        DispachDoublet(Atome);

    }


    public void DispachDoublet(GameObject At) {

        List<Doublet> doublets = new List<Doublet>();
        foreach (Doublet d in At.transform.GetComponents<Doublet>())
        {
            if (d.Atome == Atome)
                doublets.Add(d);
        }
        int number = doublets.Count;

        for (int i = 0; i < number; i++)
        {
            doublets[i].doublet.transform.localPosition = new Vector3(distance * Mathf.Cos(2 * Mathf.PI / number * i),
                distance * Mathf.Sin(2 * Mathf.PI / number * i), 0);
            doublets[i].doublet.transform.localRotation = Quaternion.Euler(0, 0, 360.0f / number * i + 90.0f);

            /*if (doublets[i].GetComponent<ElementManager>().inReaction) {
                foreach()
            }*/
        }
    }

    public void Remove()
    {
        ElementManager em = doublet.GetComponent<ElementManager>();
        if (em.inReaction)
        {
            em.arrow.atome.GetComponent<ElementManager>().reset();
            em.arrow.liaison.GetComponent<ElementManager>().reset();


            em.arrow.Remove(0.2f);

        }

        GameObject at = Atome;
        Destroy(doublet);
       
        DestroyImmediate(this);

        DispachDoublet(at);
        
 
    }
       


}
