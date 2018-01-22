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



    bool IsElement(GameObject go, GameObject[] array)
    {
        foreach (GameObject iterator in array)
        {
            if (go == iterator) return true;
        }
        return false;
    }

}
