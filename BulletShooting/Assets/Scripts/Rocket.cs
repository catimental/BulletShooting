using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float moveSpeed = 7f;
    public bool IsMove = false;
    protected Vector2 directionVector;
    
    // Start is called before the first frame update
    protected void Start()
    {
        directionVector = new Vector2(0, 1);

        IsMove = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (IsMove)
        {
            float moveX = directionVector.x * moveSpeed * Time.deltaTime;
            float moveY = directionVector.y * moveSpeed * Time.deltaTime;
            //transform.Translate(moveX, moveY, 0);
            transform.position += new Vector3(moveX, moveY, 0);
        }
    }

    protected virtual void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public void SetDirectionVector(Vector3 vector)
    {
        SetDirectionVector(new Vector2(vector.x, vector.y));
    }
    public void SetDirectionVector(Vector2 vector)
    {
        directionVector.x = vector.x;
        directionVector.y = vector.y;
    }

}
