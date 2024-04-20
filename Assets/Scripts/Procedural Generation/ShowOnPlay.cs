using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnPlay : MonoBehaviour
{
    public GameObject mapFactory;
    
    // Start is called before the first frame update
    void Start()
    {
        mapFactory.SetActive(true);
    }
}
