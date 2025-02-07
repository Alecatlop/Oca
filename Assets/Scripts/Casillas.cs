using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casillas : MonoBehaviour
{
    public int numcasillas;

    
    void Awake()
    {
        string casillastring = this.gameObject.name.Substring(7);
        numcasillas = int.Parse(casillastring);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
