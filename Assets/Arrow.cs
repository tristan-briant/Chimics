using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {


    public GameObject arrow;  // La flèche
    GameObject Line;                       // Add a line and an head                                
    GameObject Head;

    public GameObject liaison; // le donneur (1ère extremité)
    public GameObject atome; // l'accepteur (2ème extrémité, la pointe)

    public int collision = 0, collisionbis = 0;


    //Parametres géométriques
    public int NSample = 20;
    public float h = 0.2f;
    public float width = 0.03f;
    public float headAngle = 60;
    public float headLength = 0.10f;

    Vector3 startPos;

    // Couleur
    public Color color = new Color(0x3E / 255.0f, 0x3E / 255.0f, 0x8B / 255.0f, 0);

    // parametre d'animation (fade in and out)
    float TimeStart;
    float TimeEnd;
    bool isfadein = true;
    bool toBeRemoved = false;
    public float fadeDuration = 0.2f;
    //public GameObject[] elements;
    public List<GameObject> elements = new List<GameObject>(); // element a éviter de collisionner;

    Vector3[] vectors;
    Vector3[] vectorsHead;

    RectTransform PageRect; // besoin du niveau de zoom de la page

    void Awake () {

        Line = new GameObject();
        Head = new GameObject();

        FadeIn(fadeDuration);
        GameObject[] accepteurs, doublets;

        PageRect = transform.parent.parent.GetComponent<RectTransform>(); 

        accepteurs = GameObject.FindGameObjectsWithTag("Accepteur");
        doublets = GameObject.FindGameObjectsWithTag("Doublet");

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.CompareTag("Accepteur"))
                elements.Add(child.gameObject);

            if (child.CompareTag("Doublet"))
                elements.Add(child.gameObject);

        }

    }
	
	void Update () {


        LineRenderer lr1 = Line.GetComponent<LineRenderer>();
        LineRenderer lr2 = Head.GetComponent<LineRenderer>();

        float scale = PageRect.localScale.x;  // recupère le niveau de zoom
        lr1.widthMultiplier = scale;
        lr2.widthMultiplier = scale;


        float t = Time.time;

        if (t <= TimeEnd + 0.1f)
        {

            Color c = color;
            if (isfadein)
                c.a = (t - TimeStart) / (TimeEnd - TimeStart);
            else
                c.a = (1 - (t - TimeStart) / (TimeEnd - TimeStart));

            lr1.startColor = lr1.endColor = c;
            lr2.startColor = lr2.endColor = c;


        }
        else
        {
            if (toBeRemoved)  // Eventually destroy the arrow if planned to be
            {
                Destroy(arrow);
                Destroy(this);
            }
        }
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
        
 
        Vector3 start = liaison.transform.position;
        Vector3 end = atome.transform.position;

        start.z = 0;
        end.z = 0;

        Collider2D colliderAtome = atome.GetComponent<Collider2D>();
        Collider2D colliderLiaison = liaison.GetComponent<Collider2D>();

        float scale = PageRect.localScale.x; // Niveau de zoom

        float hs = h * scale;
        float headLengthS = headLength * scale;

        arrow = new GameObject()
        {
            name = "arrow"
        };
        arrow.transform.parent = transform;

        startPos = arrow.transform.parent.position;


        Line.transform.parent = arrow.transform;
        Head.transform.parent = arrow.transform;

        Line.AddComponent<LineRenderer>();
        LineRenderer lr = Line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width * 0.8f;
        lr.numCapVertices = 10;
        lr.positionCount = NSample+1;
        lr.useWorldSpace = false;

        Head.AddComponent<LineRenderer>();
        LineRenderer head = Head.GetComponent<LineRenderer>();
        head.material = new Material(Shader.Find("Sprites/Default"));
        head.startColor = head.endColor = color;
        //head.startWidth = 3 * width; head.endWidth = 0;
        head.startWidth = width;
        head.endWidth = width * 0.8f;
        head.numCapVertices = 10;
        head.numCornerVertices = 10;
        head.positionCount = 3;
        head.useWorldSpace = false;



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
        Vector3 centerbis = 0.5f * (start + end) - perp * (R - h); // Centre de la courbe

        float anglebis = -Vector3.Angle(start - centerbis, end - centerbis) / 180 * Mathf.PI;  // angle d'ouverture
        float angle0bis = Vector3.Angle(start - centerbis, Vector3.right) / 180 * Mathf.PI;  // angle de départ

        if (start.y - centerbis.y < 0) angle0bis = -angle0bis;  // Quelques corrections suivant l'orientation de la flèche
        anglebis = -anglebis;
        if (R < h) anglebis = 2 * Mathf.PI - anglebis;

        /*center = centerbis;
        angle = anglebis;
        angle0 = angle0bis;*/

        /*DrawLine(start, end, new Color(255, 255, 0));
        DrawLine(center, end, new Color(255, 255, 0));*/
        /*DrawLine(center, center - R * perp, new Color(255, 255, 0));
        DrawLine(center, center - R * new Vector3(Mathf.Cos(angle0 + Mathf.PI / 2), Mathf.Sin(angle0 + Mathf.PI / 2), 0), new Color(255, 255, 0));*/



        int k = 0; // nombre de point (< NSample car on élimine ceux dans le collider)
        int kbis = 0; // nombre de point (< NSample car on élimine ceux dans le collider)

        
        vectors = new Vector3[NSample + 1];
        vectorsHead = new Vector3[3];
        Vector3[] vectorsbis = new Vector3[NSample + 1];

 

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
                /*for (int n = 0; n < elements.Count; n++)
                {
                    collision++;
                }*/
                

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

        if (collision > collisionbis) {
            k = kbis;
            for (int i = 0; i < k; i++) vectors[i] = vectorsbis[i];
        }


        /*vectors[0] = center;
        vectors[k-2] = center;*/

        lr.positionCount = k-1 ;

        for (int i = 0; i < lr.positionCount; i++)
        {
            lr.SetPosition(i, vectors[i]);
        }

        Vector3 last = vectors[k - 3] - vectors[k - 2];
        float a = Vector3.Angle(last, Vector3.right);
        if (last.y < 0) a = -a;  // Quelques corrections suivant l'orientation de la flèche

        vectorsHead[0] = vectors[k - 1] + headLengthS * Vector3FromAngle(a + headAngle * 0.5f);
        vectorsHead[1] = vectors[k - 1];
        vectorsHead[2] = vectors[k - 1] + headLengthS * Vector3FromAngle(a - headAngle * 0.5f);

        for (int i = 0; i < 3; i++)
        {
            head.SetPosition(i, vectorsHead[i]);
        }

        /*head.SetPosition(0,  vectors[k - 1] + headLength * Vector3FromAngle(a + headAngle * 0.5f));
        head.SetPosition(1,  vectors[k - 1]);
        head.SetPosition(2,  vectors[k - 1] + headLength * Vector3FromAngle(a - headAngle * 0.5f));*/

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
}
