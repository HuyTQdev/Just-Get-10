using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteManager : MonoBehaviour
{
    public static int block, row;
    [SerializeField]
    Text rowT, blockT;
    public static DeleteManager instance;
    [SerializeField]
    Image[] rButton, bButton;
    private void Awake()
    {
        #region SINGLETON
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
    }
    private void Start()
    {
        block = PlayerPrefs.GetInt("DeleteBlock");
        row = PlayerPrefs.GetInt("DeleteRow");
    }
    public void ChangeDeleteBlockMode()
    {
        if (block == 0) return; 
        GameController.instance.deleteBlockMode = !GameController.instance.deleteBlockMode;
        GameController.instance.choseMode = GameController.instance.deleteRowMode = false;
        GameController.instance.Reset();
    }
    public void ChangeDeleteRowMode()
    {
        if (row == 0) return;
        GameController.instance.deleteRowMode = !GameController.instance.deleteRowMode;
        GameController.instance.choseMode = GameController.instance.deleteBlockMode = false;
        GameController.instance.Reset();
    }
    public void Delete()
    {
        for(int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (GameController.instance.blocks[i][j].IsDelete)
                {
                    GameController.instance.dragons[i][j].IsDestroy = true;
                    GameController.instance.dragons[i][j].Deleted();
                }
            }
        }
        GameController.instance.UpdateGrid();
        if (GameController.instance.deleteRowMode)
        {
            row--;
            GameController.instance.deleteRowMode = false;
        }else
        {
            block--;
            GameController.instance.deleteBlockMode = false;
        }
    }
    private void Update()
    {
        foreach (Image i in rButton)
        {
            if (row == 0) i.color = new Color(1, 0, 0, 1);
            else if (GameController.instance.deleteRowMode) i.color = new Color(0, 1, 0, 1);
            else i.color = Color.white;
        }
        foreach (Image i in bButton)
        {
            if (block == 0) i.color = new Color(1, 0, 0, 1);
            else if (GameController.instance.deleteBlockMode) i.color = new Color(0, 1, 0, 1);
            else i.color = Color.white;
        }
        blockT.text = block.ToString();
        rowT.text = row.ToString();
        PlayerPrefs.SetInt("DeleteBlock", block);
        PlayerPrefs.SetInt("DeleteRow", row);
    }
}
