using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour
{
    protected string goodsType;
    protected int goodsScore;
    // Start is called before the first frame update

    public string GoodsType
    {
        get { return goodsType; } 
    }

    public int GoodsScore
    {
        get { return goodsScore; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        UIManager.Instance.AddScore(this.GoodsScore);
    }
}
