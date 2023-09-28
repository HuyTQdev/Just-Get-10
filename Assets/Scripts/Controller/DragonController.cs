using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    SpriteRenderer[] srs;
    int power, distance;
    bool isChosen, isDestroy;
    public Vector3 position, v, parent;
    public static bool isMoving;
    Vector2 id;
    Animator animator;
    float coolDown;
    private void Awake()
    {
        coolDown = Random.Range(2f, 15f);
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        isMoving = false;
    }
    void Update()
    {
        coolDown -= Time.deltaTime;
        if(coolDown <= 0)
        {
            coolDown = Random.Range(2f, 15f);
            if(power >= 2 && power <= 5) animator.Play(GameManager.instance.dataDragon.dragons[power].idleAnimName);
                
        }

        if(transform.position.y <= 2 && !isDestroy)
        {
            foreach(SpriteRenderer sr in srs)sr.color = new Color(255, 255, 255, 255);
        }else foreach (SpriteRenderer sr in srs) sr.color = new Color(0, 0, 0, 0);
        if (isMoving && isChosen)
        {
            if (distance != 0 && GameController.instance.timeMoveDragon > GameController.instance.timeMoveEachDistance[distance - 1]
                && GameController.instance.timeMoveDragon <= GameController.instance.timeMoveEachDistance[distance])
            {
                foreach (SpriteRenderer sr in srs) sr.color += new Color(0, 0, 0, -1 * Time.deltaTime);
                transform.Translate(v * Time.deltaTime);
            }
            else if (distance != 0 && GameController.instance.timeMoveDragon <= GameController.instance.timeMoveEachDistance[distance - 1])
            {
                isChosen = false;
                Deleted();
            }
        }
        else if (!isMoving && !isChosen && transform.position.y > position.y)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Max((-8f * Time.deltaTime) + transform.position.y, Position.y), 0);
        }
    }
    public void Deleted()
    {
        foreach (SpriteRenderer sr in srs) sr.color = new Color(0, 0, 0, 0);
        isDestroy = true;
    }
    public static int SpawnPower()
    {

        return Random.Range(1,  Mathf.Max(4, Mathf.Min(5, MaxLevelManager.maxLevel + 1)));
    }
    public void Spawn(Vector2 value, int Power)
    {
        id = value;
        power = Power;
        if (Power == 0) power = SpawnPower();
        position = GameController.instance.blocks[(int)id.x][(int)id.y].Position;
        isDestroy = (Power == 0);
        if (MaxLevelManager.maxLevel < power) MaxLevelManager.maxLevel = power;
    }
    public int Power
    {
        get { return power; }
        set { power = value; }
    }
    public int Distance
    {
        get { return distance; }
        set { distance = value; }
    }
    public bool IsChosen
    {
        get { return isChosen; }
        set { isChosen = value; }
    }
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
    public bool IsDestroy
    {
        get { return isDestroy; }
        set { isDestroy = value; }
    }
    public Vector3 Parent
    {
        get { return parent; }
        set { 
            parent = value; 
            v = (parent - position) / (GameController.instance.timeMoveEachDistance[distance] - GameController.instance.timeMoveEachDistance[distance - 1]);
        }
    }
}
