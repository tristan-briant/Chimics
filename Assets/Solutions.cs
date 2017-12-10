using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class Solutions : MonoBehaviour {

    public GameObject[] accepteurs1;
    public GameObject[] donneurs1;

    public GameObject[] accepteurs2;
    public GameObject[] donneurs2;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public int TestReaction(GameObject[] acc, GameObject[] don)
    {
        if (acc.Length < 1) return 0;

        // Reaction en un coup
        if (accepteurs2.Length == 0) {

            Debug.Log("en un coup");

            if (acc.Length == 1) {
                if (IsElement(acc[0], accepteurs1) && IsElement(don[0], donneurs1)) return 1;
                else return 0;
            }
            else return 0;

        }
        else //if (accepteurs3.Length == 0)
        {
            Debug.Log("en 2 coup");
            if (acc.Length == 1) // Reaction incomplete ou fausse
            {
                Debug.Log("incomplet ou faux");
                if (IsElement(acc[0], accepteurs1) && IsElement(don[0], donneurs1)) return -1;
                else if (IsElement(acc[0], accepteurs2) && IsElement(don[0], donneurs2)) return -1;
                else return 0;
            }
             


            if (acc.Length == 2)
            {
                if (IsElement(acc[0], accepteurs1) && IsElement(don[0], donneurs1)
                    && IsElement(acc[1], accepteurs2) && IsElement(don[1], donneurs2)) return 1;
                if (IsElement(acc[1], accepteurs1) && IsElement(don[1], donneurs1)
                               && IsElement(acc[0], accepteurs2) && IsElement(don[0], donneurs2)) return 1;
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
