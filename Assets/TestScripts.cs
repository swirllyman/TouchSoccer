using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    [ContextMenu("Test")]
    void Test()
    {
        string s = "ThiSIHSiGOIS";
        foreach (char c in s)
        {
            Debug.Log(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
