using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.transform.position = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
    }

    public void Init(int val, bool isPlayer)
    {
        textMesh.text = val.ToString();
        if(isPlayer)
        {
            textMesh.color = Color.white;
        }
        else
        {
            textMesh.color = new Color32(170, 107, 103, 255);
        }
    }
}
