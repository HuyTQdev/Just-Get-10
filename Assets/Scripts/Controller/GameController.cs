using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public BlockController[][] blocks;
    public DragonController[][] dragons;
    int numBlock, maxDistance, tplt;
    public static GameController instance;
    public bool choseMode, isUp, coolDown, deleteBlockMode, deleteRowMode, haveUpdate;
    public float[] timeMoveEachDistance;
    public float timeMoveDragon;
    [SerializeField]
    GameObject effect;
    Animator effectAnimator;
    Vector2 chosenPosition;
    Queue<Vector2> q;
    private void Awake()
    {
        #region SINGLETON
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        numBlock = 5;
        q = new Queue<Vector2>();
        blocks = new BlockController[numBlock][];
        for (int i = 0; i < numBlock; i++) blocks[i] = new BlockController[numBlock];
        dragons = new DragonController[numBlock][];
        for (int i = 0; i < numBlock; i++) dragons[i] = new DragonController[numBlock];
        timeMoveDragon = 0f;
        timeMoveEachDistance = new float[10];
        timeMoveEachDistance[0] = 0.1f;
        for (int i = 1; i < 10; i++) timeMoveEachDistance[i] = timeMoveEachDistance[i - 1] + 0.04f;
        effectAnimator = effect.GetComponent<Animator>();
    }
    
    public void StartGame()
    {
        Time.timeScale = 1;
        for (int i = numBlock - 1; i >= 0; i--)
        {
            for (int j = numBlock - 1; j >= 0; j--)
            {
                blocks[i][j] = SpawnManager.instance.blocks[i][j].GetComponent<BlockController>();
                blocks[i][j].Spawn(new Vector2(i, j));
                dragons[i][j] = SpawnManager.instance.dragons[i][j].GetComponent<DragonController>();
                dragons[i][j].Spawn(new Vector2(i, j), 0);
            }
        }
        choseMode = deleteBlockMode = deleteRowMode = isUp = coolDown = haveUpdate = false;
        UpdateGrid();
        timeMoveDragon = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeMoveDragon > 0f)
        {
            timeMoveDragon -= Time.deltaTime;
            if (timeMoveDragon <= timeMoveEachDistance[0] && !isUp)
            {
                int i = (int)chosenPosition.x, j = (int)chosenPosition.y;
                int power = dragons[i][j].Power + 1;
                Destroy(SpawnManager.instance.dragons[i][j]);
                SpawnManager.instance.dragons[i][j] = Instantiate(GameManager.instance.dataDragon.dragons[power].dragon,
                        blocks[i][j].Position, Quaternion.identity);
                dragons[i][j] = SpawnManager.instance.dragons[i][j].GetComponent<DragonController>();
                SpawnManager.instance.dragons[i][j].GetComponent<Animator>().Play(GameManager.instance.dataDragon.dragons[power].idleAnimName);
                dragons[i][j].Spawn(chosenPosition, power);
                isUp = true;
                effect.transform.position = dragons[i][j].transform.position;
                effect.transform.SetParent(dragons[i][j].transform);
                effectAnimator.Play(GameManager.instance.dataDragon.dragons[power].effectSpawnName);
                AudioManager.instance.Play(GameManager.instance.dataDragon.dragons[power].soundSpawnName);
                ScoreManager.instance.Increascore(tplt * (power - 1));
            }
            else if (timeMoveDragon <= 0)
            {
                DragonController.isMoving = false;
                effect.transform.SetParent(null);
                UpdateGrid();
            }
            else
            {
                coolDown = false;
            }
        }
    }
    void Check(int x, int y, int px, int py)
    {
        if (x < 0 || y < 0 || x >= 5 || y >= 5) return;
        if (blocks[x][y].IsChosen) return;
        if (dragons[x][y].Power == dragons[px][py].Power)
        {
            tplt++;
            choseMode = true;
            q.Enqueue(new Vector2(x, y));
            dragons[x][y].Distance = dragons[px][py].Distance + 1;
            dragons[x][y].Parent = dragons[px][py].Position;
            if (dragons[x][y].Distance > maxDistance) maxDistance = dragons[x][y].Distance;
        }
    }
    public void ChoseMode(Vector2 v)
    {
        if (coolDown) return;
        for (int i = numBlock - 1; i >= 0; i--)
            for (int j = numBlock - 1; j >= 0; j--) if (!choseMode && dragons[i][j].Position.y < dragons[i][j].transform.position.y) return;

        if (!blocks[(int)v.x][(int)v.y].IsChosen) AudioManager.instance.Play("Choose");
        Reset();
        maxDistance = 0;
        q.Enqueue(v);
        dragons[(int)v.x][(int)v.y].Distance = 0;
        while (q.Count > 0)
        {
            Vector2 temp = q.Dequeue();
            int x = (int)temp.x, y = (int)temp.y;
            Check(x - 1, y, x, y);
            Check(x + 1, y, x, y);
            Check(x, y - 1, x, y);
            Check(x, y + 1, x, y);
            if (choseMode) dragons[x][y].IsChosen = blocks[x][y].IsChosen = true;
        }
    }
    public void NotChoseMode(Vector2 v)
    {
        if (blocks[(int)v.x][(int)v.y].IsChosen)
        {
            tplt = 1;
            chosenPosition = v;
            ChoseMode(v);
            DragonController.isMoving = true;
            timeMoveDragon = timeMoveEachDistance[maxDistance];
            isUp = coolDown = false;
        }
        else Reset();
        choseMode = false;
    }
    public void Reset()
    {
        for (int i = numBlock - 1; i >= 0; i--)
        {
            for (int j = numBlock - 1; j >= 0; j--)
            {
                dragons[i][j].IsChosen = blocks[i][j].IsChosen = blocks[i][j].IsDelete = false;
                dragons[i][j].transform.SetParent(blocks[i][j].transform);
                dragons[i][j].Position = blocks[i][j].Position;
            }
        }
    }
    public void UpdateGrid()
    {
        bool gameLose = true;
        string s = "";
        for (int i = 0; i < numBlock; i++)
        {
            float distance = 0;
            for (int j = 0; j < numBlock; j++)
            {
                if (dragons[i][j].IsDestroy)
                {
                    for (int k = j + 1; k < numBlock; k++)
                    {
                        if (!dragons[i][k].IsDestroy)
                        {
                            int power = dragons[i][k].Power;
                            Destroy(SpawnManager.instance.dragons[i][j]);
                            SpawnManager.instance.dragons[i][j] = Instantiate(GameManager.instance.dataDragon.dragons[power].dragon,
                                dragons[i][k].gameObject.transform.position, Quaternion.identity);
                            dragons[i][j] = SpawnManager.instance.dragons[i][j].GetComponent<DragonController>();
                            dragons[i][j].Spawn(new Vector2(i, j), power);
                            dragons[i][k].IsDestroy = true;
                            break;
                        }
                    }
                    if(dragons[i][j].IsDestroy)
                    {
                        if (distance == 0) distance = (numBlock - j) * BlockController.Size.y;
                        int power;
                        if(!haveUpdate && PlayerPrefs.HasKey("SaveGame") && PlayerPrefs.GetString("SaveGame") != "-1") power = "0123456789ABCDEF".IndexOf(char.ToUpper(PlayerPrefs.GetString("SaveGame")[i * numBlock + j]));
                        else power = DragonController.SpawnPower();
                        Destroy(SpawnManager.instance.dragons[i][j]);
                        SpawnManager.instance.dragons[i][j] = Instantiate(GameManager.instance.dataDragon.dragons[power].dragon,
                                blocks[i][j].Position + new Vector3(0, distance, 0), Quaternion.identity);
                        dragons[i][j] = SpawnManager.instance.dragons[i][j].GetComponent<DragonController>();
                        dragons[i][j].Spawn(new Vector2(i, j), power);
                    }
                }
                if ((i > 0 && dragons[i][j].Power == dragons[i - 1][j].Power) || (j > 0 && dragons[i][j].Power == dragons[i][j - 1].Power)) gameLose = false;
                s += dragons[i][j].Power.ToString("X");
            }
        }
        if (!haveUpdate && PlayerPrefs.GetString("SaveGame") == "-1") PlayerPrefs.SetString("SaveGame", "-1");
        else PlayerPrefs.SetString("SaveGame", s);
        if (gameLose)
        {
            PlayerPrefs.SetString("SaveGame", "-1");
            UI_Controller.instance.OpenGameOverPanel();
        }
        Reset();
        haveUpdate = true;
    }
}
