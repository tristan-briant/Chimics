using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class Solutions : MonoBehaviour {

    public GameObject[] accepteurs1;
    public GameObject[] donneurs1;

    public GameObject[] accepteurs2;
    public GameObject[] donneurs2;
    public int step = 0; // This solution is only valide for step n


    public bool TestOneArrow(Arrow ar) {
        if (IsElement(ar.atome, accepteurs1) && IsElement(ar.liaison, donneurs1)) return true;
        if (ar.liaison.name == "Doublet Sup" && IsElement(ar.atome, accepteurs1) && IsElement(ar.liaison.transform.parent.gameObject, donneurs1)) return true;
        if (IsElement(ar.atome, accepteurs2) && IsElement(ar.liaison, donneurs2)) return true;
        if (ar.liaison.name == "Doublet Sup" && IsElement(ar.atome, accepteurs2) && IsElement(ar.liaison.transform.parent.gameObject, donneurs2)) return true;
        return false;
    }

    public int TestReaction(Arrow[] arrows)
    {
        List<Arrow> arrowsInStep = new List<Arrow>();
        foreach (Arrow ar in arrows)
            if (ar.step == step) arrowsInStep.Add(ar);

        if (arrowsInStep.Count < 1) return 0;
        int points = 0;
        int weight = 100;

        if (accepteurs2.Length == 0) weight = 100; // nombre de point par flèche bonne
        else weight = 50;

        foreach(Arrow ar in arrowsInStep)
        {
            if (TestOneArrow(ar)) points += weight;
            else points -= weight;
        }

        if (points < 0) return 0;
        return points;
    }


    public void CompleteReaction(List<Arrow> arrows)
    {
        List<Arrow> arrowsSup = new List<Arrow>();

        bool first=false, second = false;

        if (donneurs2.Length == 0) second = true;

        foreach(Arrow ar in arrows)
        {
            if (TestOneArrow(ar) && IsElement(ar.atome, accepteurs1)) first = true;
            if (TestOneArrow(ar) && IsElement(ar.atome, accepteurs2)) second = true;
        }

        if (!first) {
            Arrow ar = transform.parent.gameObject.AddComponent<Arrow>();
            ar.atome = accepteurs1[0];
            ar.liaison = donneurs1[0];
            ar.color = new Color(1,0,0,0);
            ar.step = step;
            ar.DrawCurvedArrow();
        }

        if (!second)
        {
            Arrow ar = transform.parent.gameObject.AddComponent<Arrow>();
            ar.atome = accepteurs2[0];
            ar.liaison = donneurs2[0];
            ar.color = new Color(1, 0, 0, 0);
            ar.step = step;
            ar.DrawCurvedArrow();
        }
    }

    public int TestReaction(GameObject[] acc, GameObject[] don)
    {
        // Return an integer between 0 and 100%

        if (acc.Length < 1) return 0;

        for (int i = 0; i < don.Length; i++)
        {
            if (don[i].name == "Doublet Sup")
                don[i] = don[i].transform.parent.gameObject; // Si c'est un doublet sup on le remplace par son parent
        }

        // Reaction en un coup
        if (accepteurs2.Length == 0) {

            if (acc.Length == 1) {
                if (IsElement(acc[0], accepteurs1) && IsElement(don[0], donneurs1)) return 100;
                else return 0;
            }
            else return 0;

        }
        else 
        {
            if (acc.Length == 1) // Reaction incomplete ou fausse
            {
                Debug.Log("incomplet ou faux");
                if (IsElement(acc[0], accepteurs1) && IsElement(don[0], donneurs1)) return 50;
                else if (IsElement(acc[0], accepteurs2) && IsElement(don[0], donneurs2)) return 50;
                else return 0;
            }
             


            if (acc.Length == 2)
            {
                if (IsElement(acc[0], accepteurs1) && IsElement(don[0], donneurs1)
                    && IsElement(acc[1], accepteurs2) && IsElement(don[1], donneurs2)) return 100;
                if (IsElement(acc[1], accepteurs1) && IsElement(don[1], donneurs1)
                               && IsElement(acc[0], accepteurs2) && IsElement(don[0], donneurs2)) return 100;
            }
            return 0;

        }
    }

    bool IsElement(GameObject go, GameObject[] array)
    {
        foreach(GameObject iterator in array)
        {
            if (go == iterator) return true;
        }
        return false;
    }


}
