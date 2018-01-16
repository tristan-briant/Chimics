using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour {


    public GameObject arrow;  // La flèche
    GameObject Line;                       // Add a line and an head                                
    GameObject Head;

    public Vector3 position;

    public GameObject liaison; // le donneur (1ère extremité)
    public GameObject atome; // l'accepteur (2ème extrémité, la pointe)
    public int step =0; // arrow available in step 0

    GameObject image;

    //Parametres géométriques
    public int NSample = 40;
    public float h = 0.2f;
    public float width = 0.020f;
    public float headAngle = 60;
    public float headLength = 0.07f;

  
    // Couleur
    public Color color = new Color(0x60 / 255.0f, 0x60 / 255.0f, 0xDB / 255.0f, 0);

    // parametre d'animation (fade in and out)
    float TimeStart;
    float TimeEnd;
    bool isfadein = true;
    public bool toBeRemoved = false;
    public float fadeDuration = 0.2f;
 
    Vector3[] vectors;
    Vector3[] vectorsHead;

    RectTransform PageRect; // besoin du niveau de zoom de la page

    
    void Awake () {

        Line = new GameObject();
        Head = new GameObject();
        
        Line.AddComponent<LineRenderer>();
        Head.AddComponent<LineRenderer>();
        
        arrow = new GameObject()
        {
            name = "arrow"
        };
        image = new GameObject()
        {
            name = "image"
        };


        FadeIn(fadeDuration);
  
        PageRect = transform.parent.parent.GetComponent<RectTransform>();


        LineRenderer lr = Line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        //lr.material = new Material(Shader.Find("Particles/Additive"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width; // * 0.8f;
        lr.numCapVertices = 10;
        lr.positionCount = NSample + 1;
        lr.useWorldSpace = false;
        lr.sortingOrder = 1;

        LineRenderer head = Head.GetComponent<LineRenderer>();
        head.material = new Material(Shader.Find("Sprites/Default"));
        head.startColor = head.endColor = color;
        head.startWidth = width;
        head.endWidth = width; //* 0.8f;
        head.numCapVertices = 10;
        head.numCornerVertices = 10;
        head.positionCount = 3;
        head.useWorldSpace = false;
        head.sortingOrder = 1;

        /*arrow.transform.SetParent(atome.transform);
        Line.transform.SetParent(arrow.transform);
        Head.transform.SetParent(arrow.transform);

        image.AddComponent<Image>();
        image.GetComponent<Image>().color = new Color(0, 0, 0, 0);
       
        image.AddComponent<Button>();
        image.GetComponent<Button>().onClick.AddListener(delegate () { Remove(0.2f); });

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.1f) * PageRect.localScale.x;
        image.transform.SetParent(arrow.transform); //Doit être mis à la fin sinon ajouter un rectTransform ou une Image reset l'animation, pourquoi???*/
 
    }


    void Update () {
        

        LineRenderer lr1 = Line.GetComponent<LineRenderer>();
        LineRenderer lr2 = Head.GetComponent<LineRenderer>();

        float scale = PageRect.localScale.x;  // recupère le niveau de zoom
        lr1.widthMultiplier = scale;
        lr2.widthMultiplier = scale;

        float alphMultiplier = 1;

        CanvasGroup[] cgs = transform.GetComponentsInParent<CanvasGroup>();
        foreach (CanvasGroup cg in cgs) {
            alphMultiplier *= cg.alpha;
        }

        float t = Time.time;

        Color c = color;
        c.a = alphMultiplier;

        if (t <= TimeEnd + 0.1f)
        {


            if (isfadein)
                c.a *= Mathf.Clamp01((t - TimeStart) / (TimeEnd - TimeStart));
            else
                c.a *= Mathf.Clamp01(1 - (t - TimeStart) / (TimeEnd - TimeStart));

            
            /*lr1.startColor = lr1.endColor = c;
            lr2.startColor = lr2.endColor = c;*/


        }
        else
        {
            if (toBeRemoved)  // Eventually destroy the arrow if planned to be
            {
                if(atome)
                    atome.GetComponent<ElementManager>().reset();
                if(liaison)
                    liaison.GetComponent<ElementManager>().reset();
                Destroy(arrow);
                Destroy(this);
            }
        }

        
        lr1.startColor = lr1.endColor = c;
        lr2.startColor = lr2.endColor = c;

    }

    public void FadeIn(float duration)
    {
        TimeStart = Time.time;
        TimeEnd = TimeStart + duration;
        isfadein = true;
    }

    public void FadeOut(float duration)
    {
        TimeStart = Time.time;
        TimeEnd = TimeStart + duration;
        isfadein = false;
    }

    public void Remove(float duration = 0)
    {
        if (duration > 0 && !toBeRemoved)
        {
            FadeOut(duration);
            toBeRemoved = true;
        }
        else
        {
            Destroy(arrow);
            Destroy(this);
        }
           

    }


    public void DrawCurvedArrow()  // Une flèche courbe entre la liaison et l'atome
    {
        arrow.transform.SetParent(atome.transform);
        Line.transform.SetParent(arrow.transform);
        Head.transform.SetParent(arrow.transform);

        image.AddComponent<Image>();
        image.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        image.AddComponent<Button>();
        image.GetComponent<Button>().onClick.AddListener(delegate () { Remove(0.2f); });

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.1f) * PageRect.localScale.x;
        image.transform.SetParent(arrow.transform); //Doit être mis à la fin sinon ajouter un rectTransform ou une Image reset l'animation, pourquoi???

        Vector3 start = liaison.transform.position;
        Vector3 end = atome.transform.position;

        start.z = 0;
        end.z = 0;

        Collider2D colliderAtome = atome.GetComponent<Collider2D>();
        Collider2D colliderLiaison = liaison.GetComponent<Collider2D>();

        float scale = PageRect.localScale.x; // Niveau de zoom*/
        float hs = h * scale;
        float headLengthS = headLength * scale;

        List<GameObject> elements = new List<GameObject>(); // element a éviter de collisionner;
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.CompareTag("Accepteur"))
                elements.Add(child.gameObject);

            if (child.CompareTag("Doublet"))
                elements.Add(child.gameObject);

        }
        LineRenderer lr = Line.GetComponent<LineRenderer>();
        LineRenderer head = Head.GetComponent<LineRenderer>();

        // Les maths pour calculer le centre, le rayon de courbure, et l'angle d'ouverture de la flèche

        Vector3 r = Vector3.Normalize(end - start);
        float d = Vector3.Distance(end, start);
        float R = d * d / 8.0f / hs + hs * 0.5f; // Rayon de courbure
        Vector3 perp = new Vector3(r.y, -r.x, 0); // Vecteur perpendiculaire à start-end 

        // on test d'un coté 
        Vector3 center = 0.5f * (start + end) + perp * (R - hs); // Centre de la courbe

        float angle = -Vector3.Angle(start - center, end - center) / 180 * Mathf.PI;  // angle d'ouverture
        float angle0 = Vector3.Angle(start - center, Vector3.right) / 180 * Mathf.PI;  // angle de départ

        if (start.y - center.y < 0) angle0 = -angle0;  // Quelques corrections suivant l'orientation de la flèche
        if (R < hs) angle = -2 * Mathf.PI - angle;


        // et de l'autre
        Vector3 centerbis = 0.5f * (start + end) - perp * (R - hs); // Centre de la courbe

        float anglebis = -Vector3.Angle(start - centerbis, end - centerbis) / 180 * Mathf.PI;  // angle d'ouverture
        float angle0bis = Vector3.Angle(start - centerbis, Vector3.right) / 180 * Mathf.PI;  // angle de départ

        if (start.y - centerbis.y < 0) angle0bis = -angle0bis;  // Quelques corrections suivant l'orientation de la flèche
        anglebis = -anglebis;
        if (R < hs) anglebis = 2 * Mathf.PI - anglebis;

        /*DrawLine(start, end, new Color(255, 255, 0));
        DrawLine(centerbis, end, new Color(255, 255, 0));*/
        /*DrawLine(center, center - R * perp, new Color(255, 255, 0));
        DrawLine(center, center - R * new Vector3(Mathf.Cos(angle0 + Mathf.PI / 2), Mathf.Sin(angle0 + Mathf.PI / 2), 0), new Color(255, 255, 0));*/



        int k = 0; // nombre de point (< NSample car on élimine ceux dans le collider)
        int kbis = 0; // nombre de point (< NSample car on élimine ceux dans le collider)

        
        vectors = new Vector3[NSample + 1];
        vectorsHead = new Vector3[3];
        Vector3[] vectorsbis = new Vector3[NSample + 1];

        int collision = 0, collisionbis = 0;

        for (int i = 0; i < NSample +1; i++)
        {
            Vector3 vec = center + new Vector3(Mathf.Cos(angle0 + angle / NSample * i) * R, Mathf.Sin(angle0 + angle / NSample * i) * R, 0);

            if (!colliderLiaison.OverlapPoint(vec) && !colliderAtome.OverlapPoint(vec))
            // On ne retient le point que s'il ne touche ni l'atome ni la liaison
            {
                foreach(GameObject ob in elements)
                {
                    if (ob.transform.GetComponent<Collider2D>().OverlapPoint(vec)) collision++;
                } 

                vectors[k] = vec;
                k++;
            }
        }

        for (int i = 0; i < NSample + 1; i++)
        {
            Vector3 vec = centerbis + new Vector3(Mathf.Cos(angle0bis + anglebis / NSample * i) * R, Mathf.Sin(angle0bis + anglebis / NSample * i) * R, 0);

            if (!colliderLiaison.OverlapPoint(vec) && !colliderAtome.OverlapPoint(vec))
            // On ne retient le point que s'il ne touche ni l'atome ni la liaison
            {
                foreach (GameObject ob in elements)
                {
                    if (ob.transform.GetComponent<Collider2D>().OverlapPoint(vec)) collisionbis++;
                }

                vectorsbis[kbis] = vec;
                kbis++;
            }
        }

        //Debug.Log("Collision " + collision + "   " + collisionbis);
        if (collision > collisionbis) {
            k = kbis;
            for (int i = 0; i < kbis; i++) vectors[i] = vectorsbis[i];
        }


        lr.positionCount = k-1 ;
        
        for (int i = 0; i < lr.positionCount; i++)
        {
            lr.SetPosition(i,  vectors[i]);
        }

        position = vectors[lr.positionCount/2];
        image.GetComponent<RectTransform>().position = position;
        float angleImage = Vector3.Angle(Vector3.right, vectors[lr.positionCount / 2 + 1] - position);
        if ((vectors[lr.positionCount / 2 + 1] - position).y<0 ) angleImage = -angleImage;
        image.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0, angleImage);

        Vector3 last = vectors[k - 3] - vectors[k - 2];
        float a = Vector3.Angle(last, Vector3.right);
        if (last.y < 0) a = -a;  // Quelques corrections suivant l'orientation de la flèche

        vectorsHead[0] = vectors[k - 1] + headLengthS * Vector3FromAngle(a + headAngle * 0.5f);
        vectorsHead[1] = vectors[k - 1];
        vectorsHead[2] = vectors[k - 1] + headLengthS * Vector3FromAngle(a - headAngle * 0.5f);

        head.SetPositions(vectorsHead);

        /*for (int i = 0; i < 3; i++)
        {
            head.SetPosition(i, vectorsHead[i]);
        }*/

    }


    void DrawLine(Vector3 start, Vector3 end, Color color) // Juste une ligne simple (for debugging purpose)
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
        lr.startWidth = lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public Vector3 Vector3FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(a), Mathf.Sin(a),0);
    }

    public void SetWrong()
    {
        GameObject sf = Instantiate(Resources.Load("SmallFail")) as GameObject;
        sf.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.2f) * PageRect.localScale.x; 
        sf.transform.SetParent(arrow.transform);
        sf.transform.localPosition = position;
        Canvas cg = sf.AddComponent<Canvas>();
        cg.overrideSorting = true;
        cg.sortingOrder = 1;
    }

    public void SetGood()
    {
        GameObject sf = Instantiate(Resources.Load("SmallCheck")) as GameObject;
        sf.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.2f) * PageRect.localScale.x;
        sf.transform.SetParent(arrow.transform);
        sf.transform.localPosition = position;
        Canvas cg = sf.AddComponent<Canvas>();
        cg.overrideSorting = true;
        cg.sortingOrder = 1;
     }
}
