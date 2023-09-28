using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update

    public  GameObject boundSprite;
    float horizontalScreen, verticalScreen;
    void Start()
    {
        float orthoSize = boundSprite.GetComponent<SpriteRenderer>().bounds.size.x * Screen.height / Screen.width * 0.5f;
        Camera.main.orthographicSize = orthoSize;

        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        horizontalScreen = edgeVector.x * 2;
        verticalScreen = edgeVector.y * 2;
        boundSprite.transform.localScale = new Vector3(boundSprite.transform.localScale.x, verticalScreen/ boundSprite.GetComponent<SpriteRenderer>().bounds.size.y * boundSprite.transform.localScale.y, 1);
    }
}
