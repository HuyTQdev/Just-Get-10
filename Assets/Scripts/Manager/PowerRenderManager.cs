using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerRenderManager : MonoBehaviour
{
    [SerializeField]
    int id;
    [SerializeField]
    Text textId;
    [SerializeField]
    Image block, dragon;
    [SerializeField]
    Sprite dSprite, lSprite, bSprite;
    // Start is called before the first frame update
    void Start()
    {
        textId.text = id.ToString();
        dragon.sprite = GameManager.instance.dataDragon.dragons[id].sprite;
        dragon.SetNativeSize();
        //dragon.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (id <= PlayerPrefs.GetInt("MaxLevel"))
        {
            block.sprite = (id % 2 == 0) ? dSprite : lSprite;
            dragon.enabled = true;
        }
        else
        {
            block.sprite = bSprite;
            dragon.enabled = false;
        }
    }
}
