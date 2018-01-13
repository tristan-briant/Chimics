using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Doublet : MonoBehaviour {

    public GameObject  Atome;
    public GameObject doublet;

    public float distance = 45; // distance from element

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
        doublet = Instantiate(Resources.Load("Doublet")) as GameObject;
        doublet.name= "Doublet Sup";

        doublet.transform.SetParent(Atome.transform);
        doublet.transform.localScale = Vector3.one;

        DispachDoublet(Atome);
    }

    
    public void DispachDoublet(GameObject At) {
        float[] ouverture = new float[4];
        float[] startAngle = new float[4];
        float epsilon = .01f;

        List<GameObject> elements = new List<GameObject>(); // element a éviter de collisionner;
        List<GameObject> otherdoublets = new List<GameObject>();
        otherdoublets.AddRange(GameObject.FindGameObjectsWithTag("Doublet").ToList<GameObject>());
        otherdoublets.AddRange(GameObject.FindGameObjectsWithTag("Accepteur").ToList<GameObject>());
       
        Doublet[] doublets = At.transform.GetComponents<Doublet>();
        int number = doublets.Length;

        for (int i = 0; i < number; i++)
            otherdoublets.Remove(doublets[i].doublet);

        Vector3 center = At.transform.position;

        float a = 0;

        float R = distance * At.transform.lossyScale.x;

        for ( a = 2*Mathf.PI; a > 0; a -= epsilon) {
            Vector3 vec = center +  new Vector3(Mathf.Cos(a) * R, Mathf.Sin(a) * R, 0);
            bool touched = false;

            foreach(GameObject go in otherdoublets)
            {
                if (go.transform.GetComponent<Collider2D>().OverlapPoint(vec)) {
                    touched =true;
                }
            }
            if (touched) break;
        }

        float start = a;
        ouverture[0] = 2 * Mathf.PI;

        for (int i = 0; i < 4; i++)
        {
            
            bool isout = false;

            for ( ; a < start + 2 * Mathf.PI; a += epsilon)
            {
                Vector3 vec = center + new Vector3(Mathf.Cos(a) * R, Mathf.Sin(a) * R, 0);

                bool touched = false;

                foreach (GameObject go in otherdoublets)
                {
                    if (go.transform.GetComponent<Collider2D>().OverlapPoint(vec))
                    {
                       // DrawLine(center, vec, Color.blue);
                        touched = true;
                    }
                }
                if (!touched && !isout)
                {
                    isout = true;
                    startAngle[i]=a;
                }
                if (touched && isout)
                {
                    ouverture[i] = a - startAngle[i];
                    break;
                }
            }
            
        }


        int[] index = new int[number];
        float ouvertureDoublet = Mathf.PI/3f;

        for (int i = 0; i < number; i++)
        {
            index[i] = 0;
            float max = ouverture[0];

            for (int k = 1; k < 4; k++)
            {
                if (ouverture[k] > max) {
                    max = ouverture[k];
                    index[i] = k;
                }
            }

            ouverture[index[i]] -= ouvertureDoublet; 
        }


        int[] nInOuv = new int[4]; // Nombre de doublet dans l'ouverture i
        int[] count = new int[4]; // Nombre de doublet dans l'ouverture i

        for (int i = 0; i < number; i++)
        {
            ouverture[index[i]] += ouvertureDoublet;
            nInOuv[index[i]]++;
        }

        for (int i = 0; i < number; i++) 
        {
            float angleSpacer = Mathf.Clamp(ouverture[index[i]] / (nInOuv[index[i]]), Mathf.PI / 3, Mathf.PI / 2);

            //float offset = 0.5f *( ouverture[index[i]] - Mathf.PI / 2 * (nInOuv[index[i]]-1));
            //float angle = startAngle[index[i]] + count[index[i]] * ouverture[index[i]] / (nInOuv[index[i]] ) + 0.5f*ouverture[index[i]] / (nInOuv[index[i]]);
            float offset = 0.5f * (ouverture[index[i]] - angleSpacer * (nInOuv[index[i]] - 1));
            float angle = startAngle[index[i]] + offset + count[index[i]] * angleSpacer; //Mathf.PI / 2;
            count[index[i]]++;
            doublets[i].doublet.transform.localPosition = new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0);
            doublets[i].doublet.transform.localRotation = Quaternion.Euler(0, 0, angle *  180.0f /Mathf.PI + 90.0f);
           
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
        //DispachDoublet(at);
    }



    void DrawLine(Vector3 start, Vector3 end, Color color) // Juste une ligne simple (for debugging purpose)
    {
        GameObject myLine = new GameObject()
        {
            name = "line"
        };
        myLine.transform.parent = transform;

        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

}
