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

    Transform pg;

    public float Zoom;

    public float ZoomMagMax = 3.0f;
    float ZoomBest;
    float ZoomTarget;

    float SpeedZoom = 0.2f;

    private void Awake()
    {
        pageRect = transform.GetComponent<RectTransform>();
        Zoom = 1.0f;
        parentRect = transform.parent.parent.GetComponent<RectTransform>();

        pg = transform.Find("Playground");
    }

    void Update () {

        if (!reactionRect) return;



        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            ZoomTarget = ZoomTarget *1.1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            ZoomTarget = ZoomTarget / 1.1f;
        }

        if (Input.touchCount == 2) {
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

        if (one_click && ((Time.time - timerdclick) > dclick_threshold))
        {
            Debug.Log("single click");
            //call the SingleClick() function, not shown
            one_click = false;
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (!one_click)
            {
                //dclick = -1;
                timerdclick = Time.time;
                one_click = true;
            }

            else if (one_click && ((Time.time - timerdclick) < dclick_threshold))
            {
                Debug.Log("double click");
                ZoomTarget = ZoomBest;
                //transform.localPosition = new Vector3(0, 0, 0);
                one_click = false;
            }

        }
        
        ZoomTarget = Mathf.Clamp(ZoomTarget, ZoomBest, ZoomBest * ZoomMagMax);

        Zoom = (1- SpeedZoom) * Zoom + SpeedZoom * ZoomTarget;

        ChangeZoom(Zoom);

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

        ScrollRect scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
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
