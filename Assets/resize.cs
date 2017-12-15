using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resize : MonoBehaviour {

    RectTransform reactionRect, parentRect;
    RectTransform pageRect;

    Transform pg;

    public float scale;

    public float ZoomMax = 3.0f;
    float ZoomBest;

    private void Awake()
    {
        pageRect = transform.GetComponent<RectTransform>();
        scale = 1.0f;
        parentRect = transform.parent.parent.GetComponent<RectTransform>();

        pg = transform.Find("Playground");
    }

    void Update () {


        /*for (int i=0;i< pg.childCount; i++)
        {
            GameObject ch = pg.GetChild(i).gameObject;
            if (ch.activeSelf)
            {
                reactionRect=ch.transform.GetComponent<RectTransform>();
                break;
            }
        }*/

        //reactionRect = transform.Find("Playground").GetChild(0).transform.GetComponent<RectTransform>();

        if (!reactionRect) return;

        pageRect.sizeDelta = new Vector2(reactionRect.sizeDelta.x * reactionRect.localScale.x,
           reactionRect.sizeDelta.y * reactionRect.localScale.y) ;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            scale = pageRect.localScale.x + 0.1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            scale = pageRect.localScale.x - 0.1f;
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

            scale = pageRect.localScale.x;

            scale = scale - 0.001f * deltaMagnitudeDiff;


        }


        scale = Mathf.Clamp(scale, ZoomBest, ZoomBest * ZoomMax);
        pageRect.localScale = new Vector2(scale, scale);

    }

    public void InitResize(Transform reaction)
    {
        /*for (int i = 0; i < pg.childCount; i++)
        {
            GameObject ch = pg.GetChild(i).gameObject;
            if (ch.activeSelf)
            {
                reactionRect = ch.transform.GetComponent<RectTransform>();
                break;
            }
        }*/

        reactionRect = reaction.transform.GetComponent<RectTransform>();
   
        transform.localPosition = new Vector3(0, 0, 0);
        transform.parent.localPosition = new Vector3(0, 0, 0);

        ZoomBest = BestFit();
        scale = ZoomBest;
        pageRect.localScale = ZoomBest * Vector2.one;

        Transform bg = transform.Find("Background");
        float s = bg.localScale.x * ZoomBest;
        bg.GetComponent<SpriteRenderer>().size = (parentRect.rect.size + new Vector2(0, 2 * 100)) / s; //new Vector2(0, 0);
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
