using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Rocket")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound();
            //Destroy(col.gameObject);
            GameManager.Instance.IncrementKilledCount();
            col.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void MoveControl()
    {
        
        float yMove = moveSpeed * Time.deltaTime;
        transform.Translate(0, -yMove, 0);
    }
}
