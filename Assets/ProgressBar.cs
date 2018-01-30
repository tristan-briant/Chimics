using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour {

    public int progress = 0; // pourcentage de progression
    private GameObject go;

    // Update is called once per frame
    void Update () {
        go.GetComponent<Slider>().value = 0.01f * progress;
	}
}
