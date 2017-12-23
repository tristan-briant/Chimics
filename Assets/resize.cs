using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resize : MonoBehaviour
{
    bool one_click = false;
    float dclick_threshold = 0.25f;
    double timerdclick = 0;


    RectTransform reactionRect, parentRect;
    RectTransform pageRect;
    ScrollRect scrollRect;

    Transform pg;

    public float Zoom;

    public float ZoomMagMax = 3.0f;
    float ZoomBest;
    float ZoomTarget;

    Vector2 positionTarget=new Vector2(0,0);

    float SpeedZoom = 0.2f;

    private void Awake()
    {
        pageRect = transform.GetComponent<RectTransform>();
        Zoom = 1.0f;
        parentRect = transform.parent.parent.GetComponent<RectTransform>();
        scrollRect = transform.parent.parent.GetComponent<ScrollRect>();

        pg = transform.Find("Playground");
    }

    void Update () {

 
        Zoom = (1 - SpeedZoom) * Zoom + SpeedZoom * ZoomTarget;

        ChangeZoom(Zoom);
    }


    bool isDragging = false;
    Vector2 StartPosition=new Vector2(0,0);

    public void OnGUI()
    {
        Event e = Event.current;

        if (e.isMouse && e.type == EventType.MouseDown && e.clickCount == 2)
        {
            ZoomTarget = ZoomBest;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            ZoomTarget = ZoomTarget * 1.1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            ZoomTarget = ZoomTarget / 1.1f;
        }

        if (Input.touchCount == 2)
        {

            scrollRect.enabled = false; // On desactive le scroll car ça saute
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            ZoomTarget = ZoomTarget - 0.001f * deltaMagnitudeDiff;


        }
        else
        {
            scrollRect.enabled = true;
        }

        ZoomTarget = Mathf.Clamp(ZoomTarget, ZoomBest, ZoomBest * ZoomMagMax);



    }


    public void InitResize(Transform reaction)
    {
        reactionRect = reaction.transform.GetComponent<RectTransform>();
  
        Zoom = ZoomTarget = ZoomBest = BestFit();
        pageRect.localScale = ZoomBest * Vector2.one;

        Transform bg = transform.Find("Background");
        float s = bg.localScale.x * ZoomBest;
        bg.GetComponent<SpriteRenderer>().size = (parentRect.rect.size + new Vector2(0, 2 * 100)) / s; 

        pageRect.sizeDelta = new Vector2(reactionRect.sizeDelta.x * reactionRect.localScale.x,reactionRect.sizeDelta.y * reactionRect.localScale.y);
    }

    public void ChangeZoom(float z)
    {
        //change the zoom and check if position is ok 
        pageRect.localScale = new Vector2(z, z);

        //ScrollRect scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition, 0f, 1f);
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);

    }

    public void ReZoom() {
        ZoomTarget = ZoomBest;
    }

    public float BestFit(){
        float scale;

        if (parentRect.rect.width / parentRect.rect.height > reactionRect.rect.width / reactionRect.rect.height)
        {
            scale = parentRect.rect.height / reactionRect.rect.height;
        }
        else {
            scale = parentRect.rect.width / reactionRect.rect.width;
        }

        return scale;

    }

  
}
