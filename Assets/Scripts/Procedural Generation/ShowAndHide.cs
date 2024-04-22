using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndHide : MonoBehaviour
{

    public GameObject factory;
    // Start is called before the first frame update
    void Start()
    {
        factory.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
