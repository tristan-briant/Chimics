using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour {

    public int ShowAfterNFail = 3; // appear after n fail  
    public int HideAfterNFail = -1; // if <0 never diseapear
    public int ShowInStep = -1; //only show in step n of the multistep reaction if 0< allways show
    
}
