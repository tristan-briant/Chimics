using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gameController : MonoBehaviour {
    List<GameObject> accepteurs = new List<GameObject>();
    List<GameObject> doublets = new List<GameObject>();
    public int failCount; // nombre d'echec sur le level en cours
    public int step;    // Pour les réaction mutli étape, n° de l'étape
    //public bool animPlaying = false;
    levelManager LVM;
    public Transform Tips;
    Animator anim;
    public GameObject canvas;
    //resize ZoomManager;
    GameObject[] Buttons;

    public void Start()
    {
        
    }

    void Awake () {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        //if (transform.Find("Tips") != null)
        Tips = transform.Find("Tips");
        //ZoomManager = transform.parent.parent.GetComponent<resize>();

        Buttons = GameObject.FindGameObjectsWithTag("Buttons");

        transform.localPosition = new Vector3(0, 0, 0);
        //transform.localScale = new Vector3(1, 1, 1);

        transform.GetComponent<Image>().enabled = false;
        anim = GetComponent<Animator>();

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.CompareTag("Accepteur"))
                accepteurs.Add(child.gameObject);
            
            if (child.CompareTag("Doublet"))
                doublets.Add(child.gameObject);
            
        }

        failCount = 0;
        ResetElements();
    }

	
	void LateUpdate () {

        GameObject liaison=null, atome=null;

        foreach (GameObject go in doublets)
            if (go.GetComponent<ElementManager>().isSelected) liaison = go;

        foreach (GameObject go in accepteurs)
            if (go.GetComponent<ElementManager>().isSelected) atome = go;



        if (liaison && atome)
        {
            Arrow ar = gameObject.AddComponent<Arrow>();
            ar.atome = atome;
            ar.liaison = liaison;
            
            ar.DrawCurvedArrow();
           
            liaison.GetComponent<ElementManager>().ReactWith(atome);
            atome.GetComponent<ElementManager>().ReactWith(liaison);

        }

    }

    public void Validate()
    {
        Transform sol = transform.Find("Solutions");

        Arrow[] lm = transform.GetComponents<Arrow>();
        int length = lm.Length;

        GameObject[] acc = new GameObject[length];
        GameObject[] don = new GameObject[length]; ;

        int count = 0;
        foreach (Arrow iterator in lm)
        {
            acc[count] = iterator.atome;
            don[count++] = iterator.liaison;
        }

        bool win = false;

        Solutions[] solutions = sol.GetComponents<Solutions>();
        Debug.Log("nombre de sol " + solutions.Length);

        foreach (Solutions s in solutions)
        {
            if (s.TestReaction(acc, don) == 1)
                win = true;
        }

        if (win)
        {
            RemoveTip();
            failCount=0;
            step++;
            transform.parent.parent.GetComponent<resize>().ReZoom();
            GetComponent<Animator>().SetTrigger("successTrigger");
        }
        else
        {
            failCount++;
            transform.parent.parent.GetComponent<resize>().ReZoom();
            transform.parent.parent.Find("Fail").GetComponent<Animator>().SetTrigger("FailTrigger");
        }
    }




    public void WinLevel(){
        Debug.Log("gagné");
        if (LVM.completedLevel < LVM.currentLevel + 1)
            {
                LVM.completedLevel = LVM.currentLevel + 1;
                Debug.Log("level+1");
            }

        transform.parent.parent.Find("Check").GetComponent<Animator>().SetTrigger("SuccessTrigger");
    }

    public void ResetElements()
    {
        foreach (GameObject go in accepteurs)
        {
            go.GetComponent<ElementManager>().isSelected = false;
            go.GetComponent<ElementManager>().inReaction = false;
        }
        foreach (GameObject go in doublets)
        {
            go.GetComponent<ElementManager>().isSelected = false;
            go.GetComponent<ElementManager>().inReaction = false;
        }


    }

    public void ResetLevel() {
        ResetElements(); // déselectionne les éléments

        Arrow[] arrows = transform.GetComponents<Arrow>();

        foreach (Arrow it in arrows)
        {
            it.Remove(0.0f);
        }

        // si on load le level on enlève un éventuel check
        Animator anim = transform.parent.parent.Find("Check").GetComponent<Animator>();
        if(anim.isActiveAndEnabled)
            anim.SetTrigger("reset");

        transform.parent.parent.GetComponent<resize>().InitResize(transform);

        failCount = 0;

        ShowTip();
    }

    public void ClearLevel()
    {
        ResetElements(); // déselectionne les éléments

        Arrow[] arrows = transform.GetComponents<Arrow>();

        foreach (Arrow it in arrows)
        {
            it.Remove(0.2f);
        }

    }


    public void ShowTip()
    {
        if (!Tips) return; // Si pas de Tips pas de Tips !

        for(int i = 0; i < Tips.childCount; i++)
        {
            Transform tip= Tips.GetChild(i);
            TipManager tm = tip.GetComponent<TipManager>();

            if (!tm || (tm.ShowInStep != step && tm.ShowInStep >= 0))
            {
                tip.gameObject.SetActive(false);
                return;
            }

            if (failCount >= tm.ShowAfterNFail && ( failCount < tm.HideAfterNFail || tm.HideAfterNFail < 0))
            {
                tip.gameObject.SetActive(true);
            }
            else
            {
                tip.gameObject.SetActive(false);
            }

        }
    }

    public void RemoveTip()
    {
        if (!Tips) return; // Si pas de Tips pas de Tips !

        for (int i = 0; i < Tips.childCount; i++)
        {
            Transform tip = Tips.GetChild(i);
            tip.gameObject.SetActive(false);
        }
    }


    public void ClickableDisable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        foreach (GameObject ob in Buttons)
        {
            ob.SetActive(false);
            //ob.transform.GetComponent<Button>().interactable = false;
        }

    }

    public void ClickableEnable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        foreach (GameObject ob in Buttons)
        {
            ob.SetActive(true);
            //ob.transform.GetComponent<Button>().interactable = true;
        }
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LVM.ReactionSelector.SetActive(true);
            LVM.Game.SetActive(false);
        }
    }

   

}

