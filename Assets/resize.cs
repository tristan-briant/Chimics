using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resize : MonoBehaviour {

    RectTransform reactionRect;
    RectTransform rect;

    public float w,h;

    private void Start()
    {
        rect = transform.GetComponent<RectTransform>();
    }

    void Update () {
        Transform pg = transform.Find("Playground");

        for (int i=0;i< pg.childCount; i++)
        {
            GameObject ch = pg.GetChild(i).gameObject;
            if (ch.activeSelf)
            {
                reactionRect=ch.transform.GetComponent<RectTransform>();
            }
        }

        //reactionRect = transform.Find("Playground").GetChild(0).transform.GetComponent<RectTransform>();


        rect.sizeDelta = new Vector2(reactionRect.sizeDelta.x * reactionRect.localScale.x,
           reactionRect.sizeDelta.y * reactionRect.localScale.y) ;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            rect.localScale = rect.localScale + 0.1f*Vector3.one;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            rect.localScale = rect.localScale - 0.1f * Vector3.one;
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

            float scale = rect.localScale.x;

            scale = Mathf.Clamp(scale - 0.001f * deltaMagnitudeDiff, 0.3f, 3.0f);
            rect.localScale = new Vector2(scale,scale);


            
        }


        w = reactionRect.sizeDelta.x;
        h = reactionRect.sizeDelta.y;

    }
}
