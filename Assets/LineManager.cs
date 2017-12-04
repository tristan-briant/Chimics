using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

    public GameObject liaison;
    public GameObject atome;

    float alpha=0;
    float TimeStart;
    float TimeEnd;
    bool isfadein=true;
    bool toBeRemoved = false;

    private void Awake()
    {
        FadeIn(0.2f);
    }
    public void Update()
    {
        float t=Time.time;
        if (t < TimeEnd) {
            for(int i=0; i < transform.childCount; i++)
            {
                LineRenderer lr = transform.GetChild(i).GetComponent<LineRenderer>();
                Color c= lr.startColor;
                if (isfadein)
                    c.a = (t - TimeStart) / (TimeEnd - TimeStart);
                else
                    c.a = 1-(t - TimeStart) / (TimeEnd - TimeStart);

                lr.startColor=c;
                lr.endColor = c;
            }
            
        }
        else if (toBeRemoved)
        {
            //Destroy(gameObject);
        }

    }

    public void FadeIn(float duration) {
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

    public void Remove(float duration=0)
    {
        /*if(duration>0) FadeOut(duration);
        toBeRemoved = true;*/
    }

}
