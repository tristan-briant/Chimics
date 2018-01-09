using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doublet : MonoBehaviour {

    public GameObject  Atome;
    public GameObject doublet;

    public float distance = 50; // distance from element

    void CreateDoublet()
    {
        GameObject go = Instantiate(Resources.Load("Doublet")) as GameObject; ;
    }


}
