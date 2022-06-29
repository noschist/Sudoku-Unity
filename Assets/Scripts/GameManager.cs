using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int width = 9, height = 9, difficulty;
    [SerializeField] private List<BoardManager> boardArray = new List<BoardManager>();
    [SerializeField] private Block blockPrefab;

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
                boardArray[0] = boardArr;
                boardArr.isActive = false;
                return;
            }
        }
    }

    private void GenerateGrid()
    {
        var center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);
        var node = Instantiate(boardArray[0].boardLayout, new Vector2(3.81f, 3.961f), Quaternion.identity);
        var board = Instantiate(boardArray[0].boardPrefab, center, Quaternion.identity);
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
            {
                CrackJSON(request.downloadHandler.text);
            }
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
        for(int i = width - 1; i >= 0; i--)
        {
            for(int j = 0; j < height ; j++)
            {
                if (sudo.Response.UnsolvedSudoku[width - i -1][j] != 0)
                {
                    var block =  Instantiate(blockPrefab, new Vector2(j, i), Quaternion.identity);
                    block.transform.parent = GameObject.Find("node-gaps(Clone)").transform;
                    block.Init(sudo.Response.UnsolvedSudoku[width - i - 1][j], false);
                }
            }
        }    
    }    
}