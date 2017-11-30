using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gameController : MonoBehaviour {
    List<GameObject> accepteurs = new List<GameObject>();
    List<GameObject> doublets = new List<GameObject>();
    public int failCount;
    public bool animPlaying = false;
    levelManager LVM;
    GameObject Tip;
    Animator anim;

    void Start () {
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        if (transform.Find("Tip") != null)
            Tip = transform.Find("Tip").gameObject;


        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);

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

        if (animPlaying == true) return;

        bool accepteurSelected, doubletSelected, accepteurSuccess, doubletSuccess;

        accepteurSelected = false;
        accepteurSuccess = false;

        foreach (GameObject go in accepteurs)
        {
            if (go.GetComponent<ElementManager>().isSelected)
            {
                accepteurSelected=true;
                if (go.GetComponent<ElementManager>().success)
                    accepteurSuccess = true;
            }
        }

        doubletSelected = false;
        doubletSuccess = false;

        GameObject liaison=null, atome=null;

        foreach (GameObject go in doublets)
            if (go.GetComponent<ElementManager>().isSelected) liaison = go;

        foreach (GameObject go in accepteurs)
            if (go.GetComponent<ElementManager>().isSelected) atome = go;



        if (liaison && atome)
        {
            DrawCurvedArrowBetween(liaison.transform, atome.transform);
            liaison.GetComponent<ElementManager>().ReactWith(atome);
            atome.GetComponent<ElementManager>().ReactWith(liaison);

        }


        /*foreach (GameObject go in doublets)
        {
            if (go.GetComponent<ElementManager>().isSelected)
            {
                doubletSelected = true;
                if (go.GetComponent<ElementManager>().success)
                    doubletSuccess = true;
            }
        }

        if (doubletSuccess && accepteurSuccess)
        {
            animPlaying = true;
            resumeWinAnimation();
            GetComponent<Animator>().SetTrigger("successTrigger");
           
        }
        else if (accepteurSelected && doubletSelected)
        {
            animPlaying = true;
            failCount++;
            GetComponent<Animator>().SetTrigger("failTrigger");

        }*/
    }

    public void WinLevel(){
        if (LVM.completedLevel < LVM.currentLevel + 1)
            {
                LVM.completedLevel = LVM.currentLevel + 1;
                Debug.Log("level+1");
            }

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

            Transform line = transform.Find("line"); // élimine les lignes qui existent
        while (line != null)
        {
            DestroyImmediate(line.gameObject);
            line = transform.Find("line"); // élimine les lignes qui existent
        }
    }

    
    /*public void resetLevel() {
       
        resetElements();
        foreach (GameObject go in accepteurs)
            if(go.activeInHierarchy)
                go.GetComponent<ElementManager>().reset();
        foreach (GameObject go in doublets)
            if (go.activeInHierarchy)
                go.GetComponent<ElementManager>().reset();

        failCount = 0;
        GetComponent<Animator>().SetTrigger("reset");
        GetComponent<Animator>().ResetTrigger("successTrigger");
        GetComponent<Animator>().ResetTrigger("failTrigger");
        animPlaying = false;
        Tip.SetActive(false);
        ClickableEnable();
        
    }*/


    public void ShowTip()
    {
        if (Tip!= null && failCount > 3)
            Tip.SetActive(true);
    }

    public void ClickableDisable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ClickableEnable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Debug.Log("click again");
    }

    public void PrepareNextSuccess()
    {
        foreach (GameObject go in accepteurs)
            if (go.activeInHierarchy)
                go.GetComponent<ElementManager>().success=false;
        foreach (GameObject go in doublets)
            if (go.activeInHierarchy)
                go.GetComponent<ElementManager>().success = false;

        failCount = 0;

        if(transform.Find("Tip2")!=null)
            Tip = transform.Find("Tip").gameObject;
        animPlaying = false;
    }

    public void stopWinAnimation()
    {
        anim.SetFloat("winspeed",0);
        animPlaying = false;
    }

    public void resumeWinAnimation()
    {
        anim.SetFloat("winspeed", 1);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LVM.Menu.SetActive(true);
    }

    void DrawCurvedArrowBetween(Transform liaison, Transform atome) // Une flèche courbe entre une liaison et un atome
    {
        int NSample = 20;
        float h = 1.0f;
        float width = 0.1f;
        //Color color = new Color(66 / 255.0f, 66 / 255.0f, 66 / 255.0f);
        Color color = new Color(0x3E / 255.0f, 0x3E / 255.0f, 0x8B / 255.0f);

        Vector3 start = liaison.position;
        Vector3 end = atome.position;

        start.z = 0;
        end.z = 0;

        Collider2D colliderAtome = atome.GetComponent<Collider2D>();
        Collider2D colliderLiaison = liaison.GetComponent<Collider2D>();

 
        GameObject Arrow = new GameObject()
        {
            name = "line"
        };
        Arrow.transform.parent = transform;

        GameObject Line = new GameObject();
        GameObject Head = new GameObject();

        Line.transform.parent = Arrow.transform;
        Head.transform.parent = Arrow.transform;

        Line.AddComponent<LineRenderer>();
        LineRenderer lr = Line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = width;
        lr.numCapVertices = 10;
        lr.positionCount = NSample;

        Head.AddComponent<LineRenderer>();
        LineRenderer head = Head.GetComponent<LineRenderer>();
        head.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        head.startColor = head.endColor = color;
        head.startWidth = 3 * width; head.endWidth = 0;
        head.positionCount = 2;

        // Les maths pour calculer le centre, le rayon de courbure, et l'angle d'ouverture de la flèche

        Vector3 r = Vector3.Normalize(end - start);
        float d = Vector3.Distance(end, start);
        float R = d * d / 8.0f / h + h * 0.5f; // Rayon de courbure
        Vector3 perp = new Vector3(r.y, -r.x,0); // Vecteur perpendiculaire à start-end 

        Vector3 center = 0.5f * (start + end) + perp * (R - h); // Centre de la courbe

        float angle = -Vector3.Angle(start - center, end - center) / 180 * Mathf.PI;  // angle d'ouverture
        float angle0 = Vector3.Angle(start - center, Vector3.right) / 180 * Mathf.PI;  // angle de départ

        if (start.y - center.y < 0) angle0 = -angle0;  // Quelques corrections suivant l'orientation de la flèche
        if (R < h) angle = -2 * Mathf.PI - angle;

        /*DrawLine(start, end, new Color(255, 255, 0));
        DrawLine(center, end, new Color(255, 255, 0));
        //DrawLine(center, center - R * perp, new Color(255, 255, 0));
        DrawLine(center, center - R * new Vector3(Mathf.Cos(angle0 + Mathf.PI / 2), Mathf.Sin(angle0 + Mathf.PI / 2), 0), new Color(255, 255, 0));*/



        int k = 0; // nombre de point (< NSample car on élimine ceux dans le collider)

        Vector3[] vectors = new Vector3[NSample + 1];


        for (int i = 0; i < NSample + 1; i++)
        {
            Vector3 vec = center + new Vector3(Mathf.Cos(angle0 + angle / NSample * i) * R, Mathf.Sin(angle0 + angle / NSample * i) * R, 0);

            if (!colliderLiaison.OverlapPoint(vec) && !colliderAtome.OverlapPoint(vec))
            // On ne retient le point que s'il ne touche ni l'atome ni la liaison
            {
                vectors[k] = vec;
                k++;
            }

        }

        /*vectors[0] = center;
        vectors[k-2] = center;*/

        lr.positionCount = k - 1;

        for (int i = 0; i < k - 1; i++)
        {
            lr.SetPosition(i, vectors[i]);
        }

        head.SetPosition(0, vectors[k - 2]);
        head.SetPosition(1, vectors[k - 1]);

    }

    void DrawLine(Vector3 start, Vector3 end, Color color) // Juste une ligne simple (mais ne sert à rien)
    {
        GameObject myLine = new GameObject()
        {
            name = "line"
        };
        myLine.transform.parent = transform;

        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = 0.1f;
        lr.SetPosition(0, start );
        lr.SetPosition(1, end );
    }

}

