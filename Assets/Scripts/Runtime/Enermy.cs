using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy : MonoBehaviour
{
    protected string enermyType;
    // Start is called before the first frame update
    public Enermy()
    {
        this.tag = "Enermy";
    }
    public string EnermyType
    {
        get { return this.enermyType; }
    }
}
