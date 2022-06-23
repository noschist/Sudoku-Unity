using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int width = 9, height = 9;
    [SerializeField] private Node nodePrefab;
    [SerializeField] private SpriteRenderer boardPrefab;


    private void Start()
    {
        GenerateGrid();
        GetSudoku();
    }

    private void GenerateGrid()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                var node = Instantiate(nodePrefab, new Vector2(i, j), Quaternion.identity);
            }
        }

        var center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);

        var board = Instantiate(boardPrefab, center, Quaternion.identity);
        board.size = new Vector2(width, height);

        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
    }

    void GetSudoku() => StartCoroutine(GetSudokuData());

    IEnumerator GetSudokuData()
    {
        string reqUri = "https://sudoku-board.p.rapidapi.com/new-board?diff=2&stype=list&solu=true";
        using (UnityWebRequest request = UnityWebRequest.Get(reqUri))
        {
            request.SetRequestHeader("X-RapidAPI-Key", "16e7fadf2amshc827494108f8b58p17c6a5jsnb37b5267332c");
            request.SetRequestHeader("X-RapidAPI-Host", "sudoku-board.p.rapidapi.com");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log(request.error);
            else
                CreateFromJSON(request.downloadHandler.text);
        }
    }

    private void SpawnBlocks(Response sudo)
    {
        Debug.Log(sudo.response.unsolved_sudoku);
        
    }

    private void CreateFromJSON(string jsonString)
    {
        var values = JsonConvert.DeserializeObject<Response>(jsonString);
        SpawnBlocks(values);
    }
}

public struct BlockType
{
    public int val;
    public bool isPlayer;
}

public struct Response
{
    public SudokuBoard response;
    public struct SudokuBoard
    {
        public string difficulty;
        public int[][] solution;
        [JsonProperty(PropertyName = "unsolved-sudoku")] public int[][] unsolved_sudoku;
    }
}