using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour {

    public float periode = 1.5f;
	// Update is called once per frame
	void Update () {
        GetComponent<CanvasGroup>().alpha = 0.7f + 0.3f * Mathf.Sin(2 * Mathf.PI * Time.time / periode);
	}
}
