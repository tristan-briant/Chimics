using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tipManager : MonoBehaviour {

    //levelManager LVM;
    //gameController GC;
	// Use this for initialization
	void Start () {
        Vector3 pos = transform.localPosition;
        transform.localPosition = new Vector3(0, 0, pos.z);
        //GC =transform.parent.transform.GetComponent<gameController>();
        gameObject.SetActive(false);

    }
    
    
}
