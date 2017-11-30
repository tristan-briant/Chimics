using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {

    private gameController gc;

	// Use this for initialization
	void Start () {
        gc = transform.parent.GetComponent<gameController>();
	}
	
    public void ClearGame()
    {
        gc.ResetElements();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
