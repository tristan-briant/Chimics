using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Groupe : MonoBehaviour {


    public GameObject groupe;  // La groupe
    GameObject[] Segments;                       // Add a line and an head                                
    
    public GameObject[] elements;
    public GameObject groupName;
    public bool correction = false;

    //Parametres géométriques
    public float width = 0.3f;
   
    // Couleur
    public Color color = new Color(0x60 / 255.0f, 0x60 / 255.0f, 0xDB / 255.0f, 0);

    // parametre d'animation (fade in and out)
    float TimeStart;
    float TimeEnd;
    bool isfadein = true;
    bool toBeRemoved = false;
    public float fadeDuration = 0.2f;
  
    RectTransform PageRect; // besoin du niveau de zoom de la page
 

    void Awake () {

        FadeIn(fadeDuration);
  
        PageRect = transform.parent.parent.GetComponent<RectTransform>();
       

    }

    void Update()
    {

        float t = Time.time;
        float scale = PageRect.localScale.x;  // recupère le niveau de zoom

        foreach (LineRenderer lr in groupe.GetComponentsInChildren<LineRenderer>())
        {
            lr.widthMultiplier = scale;
        }


        if (t <= TimeEnd + 0.1f)
        {
            //Color c = color;
            float alpha;

            if (isfadein)
                alpha = Mathf.Clamp01((t - TimeStart) / (TimeEnd - TimeStart));
            else
                alpha = Mathf.Clamp01((1 - (t - TimeStart) / (TimeEnd - TimeStart)));
        
            foreach(LineRenderer lr in groupe.GetComponentsInChildren<LineRenderer>())
            {
                Color c = lr.startColor;
                c.a= alpha;
                lr.startColor = lr.endColor = c;
            }

        }
        else
        {
            if (toBeRemoved)  // Eventually destroy the arrow if planned to be
            {
                foreach (GameObject el in elements)
                    el.GetComponent<ElementManager>().reset();
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
            foreach (GameObject el in elements)
                el.GetComponent<ElementManager>().reset();
            Destroy(groupe);
            Destroy(this);
        }
           

    }


    public void DrawGroupe()  // Dessine le groupe
    {
        if (elements.Length < 1) return;

        float scale = PageRect.localScale.x; // Niveau de zoom
        color.a = 0;


        groupe = new GameObject()
        {
            name = "groupe"
        };
        groupe.transform.parent = transform;


        int size = elements.Length==1 ? 1:elements.Length - 1;


        Segments = new GameObject[size];

        if (correction)
        {
            for (int i = 0; i < size; i++)
            {
                GameObject seg =  new GameObject();
                seg.transform.parent = groupe.transform;
                LineRenderer lr = seg.AddComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Sprites/Default"));
                //lr.material = new Material(Shader.Find("Particles/Additive"));
                lr.startColor = lr.endColor = Color.red;

                if (elements.Length == 1)
                    lr.startWidth = lr.endWidth = width * 1.3f * 1.1f; // Un peu plus gros si tout seul
                else
                    lr.startWidth = lr.endWidth = width * 1.1f;
                lr.numCapVertices = 10;
                lr.positionCount = 2;
                lr.useWorldSpace = false;
                lr.sortingOrder = -2;

                Vector3 start = elements[i].transform.position;
                Vector3 end;
                if (elements.Length > 1)
                    end = elements[i + 1].transform.position;
                else
                    end = start;

                start.z = 0;
                end.z = 0;

                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
            }
        }

        for (int i = 0; i < size; i++)
        {
            Segments[i] = new GameObject();
            Segments[i].transform.parent = groupe.transform;
            LineRenderer lr = Segments[i].AddComponent<LineRenderer>();

           

            lr.material = new Material(Shader.Find("Sprites/Default"));
            //lr.material = new Material(Shader.Find("Particles/Additive"));
            if (correction) color.a = 1; // inutil de faire un fade in
            lr.startColor = lr.endColor = color;
            if (elements.Length == 1)
                lr.startWidth = lr.endWidth = width * 1.3f; // Un peu plus gros si tout seul
            else
                lr.startWidth = lr.endWidth = width;
            lr.numCapVertices = 10;
            lr.positionCount = 2;
            lr.useWorldSpace = false;
            lr.sortingOrder = -1;

            Vector3 start = elements[i].transform.position;
            Vector3 end;
            if (elements.Length > 1)
                end = elements[i + 1].transform.position;
            else
                end = start;

            start.z = 0;
            end.z = 0;

            lr.SetPosition(0, start);
            lr.SetPosition(1, end);

            //////// Les boutons pour le retirer
            GameObject image = new GameObject();

            image.AddComponent<Image>();
            image.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            image.AddComponent<Button>();
            image.GetComponent<Button>().onClick.AddListener(delegate () { Remove(0.2f); });

            image.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector3.Distance(start, end)+ width * PageRect.localScale.x, width * PageRect.localScale.x);
            image.transform.position = 0.5f * (end + start);
            float angleImage = Vector3.Angle(Vector3.right, end - start);
            if ((end - start).y < 0) angleImage = -angleImage;
            image.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angleImage);

            image.transform.SetParent(groupe.transform); //Doit être mis à la fin sinon ajouter un rectTransform ou une Image reset l'animation, pourquoi???

        }

    }

    public void SetWrong()
    {
        Vector3 position = Vector3.zero;
        foreach (GameObject el in elements) {
            position += el.transform.position;
        }
        position = position / elements.Length;


        GameObject sf = Instantiate(Resources.Load("SmallFail")) as GameObject;
        sf.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.2f) * PageRect.localScale.x;
        sf.transform.SetParent(groupe.transform);
        sf.transform.localPosition = position;
        Canvas cg = sf.AddComponent<Canvas>();
        cg.overrideSorting = true;
        cg.sortingOrder = 1;
    }

    public void SetGood()
    {
        Vector3 position = Vector3.zero;
        foreach (GameObject el in elements)
        {
            position += el.transform.position;
        }
        position = position / elements.Length;
        GameObject sf = Instantiate(Resources.Load("SmallCheck")) as GameObject;
        sf.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.2f) * PageRect.localScale.x;
        sf.transform.SetParent(groupe.transform);
        sf.transform.localPosition = position;
        Canvas cg = sf.AddComponent<Canvas>();
        cg.overrideSorting = true;
        cg.sortingOrder = 1;
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
