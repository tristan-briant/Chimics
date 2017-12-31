using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionGroupes : MonoBehaviour
{
    public GameObject GroupName;

    public GameObject[] elements;
 


    public int TestGroupes(Groupe gr)
    {
        if (gr.groupName != GroupName) return 0;

        if (gr.elements.Length != elements.Length) return 0;

        foreach(GameObject go in gr.elements)
        {
            if (!IsElement(go, elements)) return 0;
        }

        return 1;
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
