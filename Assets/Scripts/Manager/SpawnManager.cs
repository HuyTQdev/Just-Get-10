using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject Block, board, Dragon;
    public static SpawnManager instance;
    public GameObject[][] blocks, dragons;
    int numBlock;
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
        blocks = new GameObject[numBlock][];
        for (int i = 0; i < numBlock; i++) blocks[i] = new GameObject[numBlock];
        dragons = new GameObject[numBlock][];
        for (int i = 0; i < numBlock; i++) dragons[i] = new GameObject[numBlock];
    }
    void Start()
    {
        MaxLevelManager.maxLevel = 1;
        for (int i = numBlock - 1; i >= 0; i--)
        {
            for (int j = numBlock - 1; j >= 0; j--)
            {
                Vector2 position = board.transform.position
                    + new Vector3(BlockController.Size.x, 0, 0) * (i - 2)
                    + new Vector3(0, BlockController.Size.y, 0) * (j - 2);
                blocks[i][j] = Instantiate(Block, position, Quaternion.identity);
                blocks[i][j].name = $"Block {i} {j}";
                dragons[i][j] = Instantiate(Dragon, position, Quaternion.identity);
                dragons[i][j].name = $"Dragon {i} {j}";
                dragons[i][j].transform.SetParent(blocks[i][j].transform);
            }
        }
        GameController.instance.StartGame();
    }
}
