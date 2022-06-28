using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int width = 9, height = 9, difficulty;
    //[SerializeField] private Node nodePrefab;
    [SerializeField] private List<BoardManager> boardArray = new List<BoardManager>();
    [SerializeField] private SpriteRenderer boardPrefab;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private float gap; 

    //private List<Node> nodes;

    //private BoardManager activeBoard = new BoardManager();

    //todo: if no gap, change node and block scale to 1

    private void Start()
    {
        getActiveBoard();
        GenerateGrid();
    }

    private void getActiveBoard()
    {
        foreach (BoardManager boardArr in boardArray)
        {
            if (boardArr.isActive)
            {
                //activeBoard = boardArr;
                boardArray[0] = boardArr;
                boardArr.isActive = false;
                return;
            }
        }
    }

    private void GenerateGrid()
    {
        //nodes = new List<Node>();
   
        var node = Instantiate(boardArray[0].boardLayout, new Vector2(0, 0), Quaternion.identity);
        //nodes.Add(node);
             
        var center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);

        var board = Instantiate(boardPrefab, center, Quaternion.identity);
        board.size = new Vector2(width, height);

        Camera.main.transform.position = new Vector3(center.x, center.y, -10);

        GetSudoku(difficulty);
    }



    private IEnumerator CallSudokuAPI(int diff)
    {
        string reqUri = $"https://sudoku-board.p.rapidapi.com/new-board?diff={diff}&stype=list&solu=true";
        using (UnityWebRequest request = UnityWebRequest.Get(reqUri))
        {
            request.SetRequestHeader("X-RapidAPI-Key", "16e7fadf2amshc827494108f8b58p17c6a5jsnb37b5267332c");
            request.SetRequestHeader("X-RapidAPI-Host", "sudoku-board.p.rapidapi.com");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log(request.error);
            else
                CrackJSON(request.downloadHandler.text);
        }
    }

    public void GetSudoku(int diff)
    {
        StartCoroutine(CallSudokuAPI(diff));
    }

    private void CrackJSON(string jsonString)
    {
        SpawnBlocks(JsonConvert.DeserializeObject<Root>(jsonString));
    }

    
    private void SpawnBlocks(Root sudo)
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (sudo.Response.UnsolvedSudoku[i][j] != 0)
                {
                    var block =  Instantiate(blockPrefab, new Vector2(i, j), Quaternion.identity);
                    block.Init(sudo.Response.UnsolvedSudoku[i][j], false);
                }
            }
        }    
    }    
}