using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Response
{
    [JsonProperty(PropertyName = "difficulty")]
    public string Difficulty { get; set; }

    [JsonProperty(PropertyName = "solution")]
    public List<List<int>> Solution { get; set; }

    [JsonProperty(PropertyName = "unsolved-sudoku")]
    public List<List<int>> UnsolvedSudoku { get; set; }
}

public class Root
{
    [JsonProperty(PropertyName = "response")]
    public Response Response { get; set; }
}

public class BlockType
{
    public int val;
    public bool isPlayer;
}