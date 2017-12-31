using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groupe : MonoBehaviour {


    public GameObject groupe;  // La groupe
    GameObject[] Segments;                       // Add a line and an head                                
    
    public GameObject[] elements;
    public GameObject groupName;

    //Parametres géométriques
    /*public int NSample = 40;
    public float h = 0.2f;*/
    public float width = 0.25f;
   
  
    // Couleur
    public Color color = new Color(0x60 / 255.0f, 0x60 / 255.0f, 0xDB / 255.0f, 0);

    // parametre d'animation (fade in and out)
    float TimeStart;
    float TimeEnd;
    bool isfadein = true;
    bool toBeRemoved = false;
    public float fadeDuration = 0.2f;
  


    Vector3[] vectors;
    Vector3[] vectorsHead;

    RectTransform PageRect; // besoin du niveau de zoom de la page

    void Awake () {

        FadeIn(fadeDuration);
  
        PageRect = transform.parent.parent.GetComponent<RectTransform>(); 

    }
	
	void Update () {

        float t = Time.time;
        float scale = PageRect.localScale.x;  // recupère le niveau de zoom

        for (int i = 0; i < Segments.Length; i++)
        {
            LineRenderer lr = Segments[i].GetComponent<LineRenderer>();
            lr.widthMultiplier = scale;
        }


            if (t <= TimeEnd + 0.1f)
        {

            Color c = color;
            if (isfadein)
                c.a = (t - TimeStart) / (TimeEnd - TimeStart);
            else
                c.a = (1 - (t - TimeStart) / (TimeEnd - TimeStart));


            for (int i = 0; i < Segments.Length; i++)
            {
                LineRenderer lr = Segments[i].GetComponent<LineRenderer>();

                
                lr.widthMultiplier = scale;

                lr.startColor = lr.endColor = c;
            }


        }
        else
        {
            if (toBeRemoved)  // Eventually destroy the arrow if planned to be
            {
                Destroy(groupe);
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
            Destroy(groupe);
            Destroy(this);
        }
           

    }


    public void DrawGroupe()  // Dessine le groupe
    {
        if (elements.Length < 2) return;

        float scale = PageRect.localScale.x; // Niveau de zoom
        color.a = 0;


        groupe = new GameObject()
        {
            name = "groupe"
        };
        groupe.transform.parent = transform;

        Segments = new GameObject[elements.Length-1];

        for (int i = 0; i < elements.Length - 1; i++)
        {
            Segments[i] = new GameObject();
            Segments[i].transform.parent = groupe.transform;
            LineRenderer lr = Segments[i].AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            //lr.material = new Material(Shader.Find("Particles/Additive"));
            lr.startColor = lr.endColor = color;
            lr.startWidth = width;
            lr.endWidth = width; // * 0.8f;
            lr.numCapVertices = 10;
            lr.positionCount = 2;
            lr.useWorldSpace = false;
            lr.sortingOrder = -1;

            Vector3 start = elements[i].transform.position;
            Vector3 end = elements[i+1].transform.position;
            start.z = 0;
            end.z = 0;

            lr.SetPosition(0, start);
            lr.SetPosition(1, end);

        }


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
