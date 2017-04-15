using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour {

    //public bool success;
    //public int failCount = 0;
    //GameObject molecule1, molecule2, tip;

	// Use this for initialization
	void Start () {
        var rect=GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        
    }

    
  
}
