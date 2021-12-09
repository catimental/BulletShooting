using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.8f); // 0.8초후 폭발이 사라짐
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
