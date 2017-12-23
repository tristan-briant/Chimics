using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gameController : MonoBehaviour {
    List<GameObject> accepteurs = new List<GameObject>();
    List<GameObject> doublets = new List<GameObject>();
    public int failCount; // nombre d'echec sur le level en cours
    public int step;    // Pour les réaction mutli étape, n° de l'étape
    levelManager LVM;
    public Transform Tips;
    Animator anim;
    public GameObject canvas;
    GameObject[] Buttons;
    GameObject ResetButton;

    public void Start()
    {
        //ResetLevel();
    }

    void Awake () {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        Tips = transform.Find("Tips");
 
        Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        ResetButton = GameObject.FindGameObjectWithTag("Reset");

        transform.localPosition = new Vector3(0, 0, 0);

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

        // Enlève le texte pour ne pas être visible sur le jeu
        Transform sol = transform.Find("Solutions");
        if (sol.GetComponent<Text>())
            sol.GetComponent<Text>().enabled = false;

        
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

            Animator anim = GetComponent<Animator>();
            if (anim != null && anim.enabled)
                anim.SetTrigger("successTrigger");
            else
                WinLevel();
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

        ResetButton.SetActive(true);
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
        
        // On reset l'animation
        anim = GetComponent<Animator>();
        if (anim && anim.isActiveAndEnabled)
            anim.SetTrigger("reset");

        // si on load le level on enlève un éventuel check
        
        if (transform.parent)
        {
            Transform t = transform.parent.parent.Find("Check");
            Animator anim = t.GetComponent<Animator>();
            if (anim.isActiveAndEnabled)
                anim.SetTrigger("reset");

            transform.parent.parent.GetComponent<resize>().InitResize(transform);

        }

        if (gameObject.name.Contains("Tuto") == false) 
            ResetElements(); // déselectionne les éléments

        Arrow[] arrows = transform.GetComponents<Arrow>();

        foreach (Arrow it in arrows)
        {
            it.Remove(0.0f);
        }
 
        failCount = 0;
        step = 0;

        ShowTip();
        ResetButton.SetActive(false);

        if (gameObject.name.Contains("Tuto"))
        {
            foreach (GameObject ob in Buttons)
            {
                ob.SetActive(false);
            }
        }
        else
        {
            ClickableEnable();
        }
    }

    public void ClearLevel()
    {
        Debug.Log("Clear Level");
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

