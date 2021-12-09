using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 2f;
    public GameObject explosion;
    //public GameObject rocket;
    public bool canShoot = false;
    float shootDelay = 0.1f;
    float shootTimer = 0;

    private Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
        ShootControl();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var tag = col.gameObject.tag;
        tag = string.Empty;
        if(tag == "Enemy" || tag == "EnemyRocket")
        {            
            Instantiate(explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound();
            GameManager.Instance.KillPlayer();

            Destroy(col.gameObject);
            //Destroy(gameObject);
            InactivePlayer();
        }
    }

    void InactivePlayer()
    {
        gameObject.SetActive(false);
        canShoot = false;
        transform.position = playerPos;
    }

    void MoveControl()
    {
        //���ӵ� ���� �̵� �����ϱ�
        float moveX = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        transform.Translate(moveX, moveY, 0);

        var viewPos = Camera.main.WorldToViewportPoint(transform.position);

        //�Էµ� ���� 0~1���̸� ������� �����������ִ� �Լ�
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y); // y��ǥ�赵 ó���Ϸ�


        var worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = worldPos;
    }

    void ShootControl()
    {
        if(Input.GetKey(KeyCode.X))
        {
            var objectManager = ObjectManager.Instance;
            if(objectManager.GetActivatedBulletCount() < 5 && shootTimer >= shootDelay)
            {

                //Instantiate(rocket, transform.position, rocket.transform.rotation);
                objectManager.GetBullet(transform.position);
                shootTimer = 0;
            }
            shootTimer += Time.deltaTime;
        }
    }
}
