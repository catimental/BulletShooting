using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyRocket : Rocket
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();
        if (IsMove)
        {
            float moveX = directionVector.x * moveSpeed * Time.deltaTime;
            float moveY = directionVector.y * moveSpeed * Time.deltaTime;
            var viewPos = Camera.main.WorldToViewportPoint(transform.position);

            //transform.Translate(moveX, moveY, 0);
            transform.position += new Vector3(moveX, moveY, 0);
        }
    }

    protected override void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
