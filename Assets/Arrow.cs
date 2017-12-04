using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {


    public GameObject arrow;  // La flèche

    public GameObject liaison; // le donneur (1ère extremité)
    public GameObject atome; // l'accepteur (2ème extrémité, la pointe)


    //Parametres géométriques
    public int NSample = 20;
    public float h = 1.0f;
    public float width = 0.1f;

    // Couleur
    public Color color = new Color(0x3E / 255.0f, 0x3E / 255.0f, 0x8B / 255.0f, 0);

    // parametre d'animation (fade in and out)
    float TimeStart;
    float TimeEnd;
    bool isfadein = true;
    bool toBeRemoved = false;
    public float fadeDuration = 0.2f;

    void Start () {
        FadeIn(fadeDuration);
    }
	
	void Update () {
        float t = Time.time;
        if (t < TimeEnd)
        {
            for (int i = 0; i < arrow.transform.childCount; i++)
            {
                LineRenderer lr = arrow.transform.GetChild(i).GetComponent<LineRenderer>();
                Color c = lr.startColor;
                if (isfadein)
                    c.a = (t - TimeStart) / (TimeEnd - TimeStart);
                else
                    c.a = 1 - (t - TimeStart) / (TimeEnd - TimeStart);

                lr.startColor = c;
                lr.endColor = c;
            }

        }
        else if (toBeRemoved)  // Eventually destroy the arrow if planned to be
        {
            Destroy(arrow);
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
            Destroy(arrow);
    }



    //public void DrawCurvedArrowBetween(Transform liaison, Transform atome) // Une flèche courbe entre une liaison et un atome
    public void DrawCurvedArrow()  // Une flèche courbe entre la liaison et l'atome
    {
        
 
        Vector3 start = liaison.transform.position;
        Vector3 end = atome.transform.position;

        start.z = 0;
        end.z = 0;

        Collider2D colliderAtome = atome.GetComponent<Collider2D>();
        Collider2D colliderLiaison = liaison.GetComponent<Collider2D>();


        arrow = new GameObject()
        {
            name = "line"
        };
        arrow.transform.parent = transform;

        /*arrow.AddComponent<LineManager>();                       // Store the end of the arrow
        LineManager lm = arrow.GetComponent<LineManager>();
        lm.atome = atome.gameObject;
        lm.liaison = liaison.gameObject;*/



        GameObject Line = new GameObject();                       // Add a line and an head                                
        GameObject Head = new GameObject();

        Line.transform.parent = arrow.transform;
        Head.transform.parent = arrow.transform;

        Line.AddComponent<LineRenderer>();
        LineRenderer lr = Line.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = width;
        lr.numCapVertices = 10;
        lr.positionCount = NSample;

        Head.AddComponent<LineRenderer>();
        LineRenderer head = Head.GetComponent<LineRenderer>();
        head.material = new Material(Shader.Find("Sprites/Default"));
        head.startColor = head.endColor = color;
        head.startWidth = 3 * width; head.endWidth = 0;
        head.positionCount = 2;

        // Les maths pour calculer le centre, le rayon de courbure, et l'angle d'ouverture de la flèche

        Vector3 r = Vector3.Normalize(end - start);
        float d = Vector3.Distance(end, start);
        float R = d * d / 8.0f / h + h * 0.5f; // Rayon de courbure
        Vector3 perp = new Vector3(r.y, -r.x, 0); // Vecteur perpendiculaire à start-end 

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
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }


}
