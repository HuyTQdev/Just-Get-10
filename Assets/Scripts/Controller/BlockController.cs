using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour
{
    Vector2 id;
    Vector3 position;
    [SerializeField]
    SpriteRenderer sr;
    [SerializeField]
    Sprite lightBlock, darkBlock, blackBlock;
    static Vector3 size;
    bool isChosen, isDelete;
    private void Awake()
    {
        size = new Vector2(0.97f, 1f);
    }
    private void Start()
    {
        isChosen = false;
    }
    private void Update()
    {
        if(isDelete) sr.color = new Color(1, 100 / 255f, 100 / 255f, 1);
        else if (isChosen) sr.color = new Color(100 / 255f, 1, 100/255f, 1);
        else sr.color = Color.white;
        if(isChosen && transform.position.y < position.y + 0.07f)
        {
            transform.Translate(0, 0.5f * Time.deltaTime, 0);
            if (transform.position.y > position.y + 0.07f) transform.position = position + new Vector3(0, 0.07f, 0);
        }
        else if(!isChosen && transform.position.y > position.y)
        {
            transform.Translate(0, - 0.5f * Time.deltaTime, 0);
            if (transform.position.y < position.y) transform.position = position;
        }
    }
    private void OnMouseDown()
    {
        if (isDelete) {
            AudioManager.instance.Play(GameManager.instance.dataDragon.dragons[GameController.instance.dragons[(int)id.x][(int)id.y].Power].soundSpawnName);
            DeleteManager.instance.Delete(); 
            return; 
        }
        if (GameController.instance.deleteBlockMode) {
            AudioManager.instance.Play("Choose");
            GameController.instance.Reset();
            isDelete = true; 
            return; 
        }
        if (GameController.instance.deleteRowMode)
        {
            AudioManager.instance.Play("Choose");
            GameController.instance.Reset();
            for (int i = 0; i < 5; i++) GameController.instance.blocks[i][(int)id.y].isDelete = true;
            return;
        }
        if (Time.timeScale == 0) return;
        if (DragonController.isMoving) return;
        if (!GameController.instance.choseMode) GameController.instance.ChoseMode(id);
        else GameController.instance.NotChoseMode(id);
    }
    public void Spawn(Vector2 value)
    {
        id = value;
        sr.sortingOrder = 5 - (int)id.y;
        position = transform.position;
        if ((id.x + id.y) % 2 == 1) sr.sprite = darkBlock;
        else sr.sprite = lightBlock;
    }
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
    public static Vector3 Size
    {
        get { return size; }
    }
    public bool IsChosen
    {
        get { return isChosen; }
        set { isChosen = value; }
    }
    public bool IsDelete
    {
        get { return isDelete; }
        set { isDelete = value; }
    }
}
