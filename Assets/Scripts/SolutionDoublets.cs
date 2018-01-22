using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionDoublets : MonoBehaviour {

    [Header("Ajout d'un doublet")]
    public GameObject[] Plus1;
    [Header("Ajout de 2 doublets")]
    public GameObject[] Plus2;
    [Header("Ajout de 3 doublets")]
    public GameObject[] Plus3;
    [Header("Ajout de 4 doublets")]
    public GameObject[] Plus4;

    public int TestSolution()
    {
        int test = 0;
        foreach (GameObject acc in GameObject.FindGameObjectsWithTag("Accepteur"))
        {
            Doublet[] doublets = acc.transform.GetComponents<Doublet>();
            int number = doublets.Length;

            int k = 1;
            if (IsElement(acc, Plus1)) { test += k - Mathf.Abs(number - k); continue; }
            k++;
            if (IsElement(acc, Plus2)) { test += k - Mathf.Abs(number - k); continue; }
            k++;
            if (IsElement(acc, Plus3)) { test += k - Mathf.Abs(number - k); continue; }
            k++;
            if (IsElement(acc, Plus4)) { test += k - Mathf.Abs(number - k); continue; }

            test -= number;

        }


        return test;
    }

    public int MaxScore() {
        return Plus1.Length + 2 * Plus2.Length + 3 * Plus3.Length + 4 * Plus4.Length;
    }

    public void CorrectDoublet() {
        foreach (GameObject acc in GameObject.FindGameObjectsWithTag("Accepteur"))
        {
            Doublet[] dbs = acc.transform.GetComponents<Doublet>();
            List<Doublet> doublets = new List<Doublet>();
            //doublets.AddRange(dbs);

            foreach (Doublet d in dbs)
            {
                ElementManager el = d.doublet.GetComponent<ElementManager>();
                if (el.inReaction==true)
                {
                    Debug.Log("inreact");
                    doublets.Insert(0, d);
                }
                else doublets.Add(d);
            }

            int number = doublets.Count;

            int k = 0;
            if (IsElement(acc, Plus1)) { k = 1; }
            if (IsElement(acc, Plus2)) { k = 2; }
            if (IsElement(acc, Plus3)) { k = 3; }
            if (IsElement(acc, Plus4)) { k = 4; }

            for (int i = 0; i < k - number; i++)
            {
                Doublet d = acc.AddComponent<Doublet>();
                d.Atome = acc;
                d.color = Color.red;
                d.DrawDoublet();
               
            }

            for (int i = 0; i < number && i< k; i++)
            {
                doublets[i].SetGood();
            }

            for (int i = k; i < number; i++)
            {
                doublets[i].SetWrong();
            }

            transform.GetComponentInParent<GameControllerArrowDoublet>().UpdateArrow(acc);
        }
    }


    bool IsElement(GameObject go, GameObject[] array)
    {
        foreach (GameObject iterator in array)
        {
            if (go == iterator) return true;
        }
        return false;
    }

}
