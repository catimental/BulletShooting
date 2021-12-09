using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    public GameObject rocketPrefab;
    public BossHPSlider BossHpSlider;
    private List<GameObject> bullets = new List<GameObject>();//ÃÑ¾Ë

    private void Awake()
    {
        if (ObjectManager.Instance == null)
        {
            ObjectManager.Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearBullets()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].SetActive(false);
        }
    }

    void CreateBullets(int bulletCount)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(rocketPrefab) as GameObject;

            bullet.transform.parent = transform;
            bullet.SetActive(false);

            bullets.Add(bullet);
        }
    }

    public int GetActivatedBulletCount()
    {
        return bullets.Where(bullet => bullet.activeSelf)
            .Count();
;    }

    public GameObject GetBullet(Vector3 pos)
    {
        GameObject reqBullet = null;
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeSelf)
            {
                reqBullet = bullets[i];
                break;
            }
        }

        if (reqBullet == null)
        {
            GameObject newBullet = Instantiate(rocketPrefab) as GameObject;
            newBullet.transform.parent = transform;

            bullets.Add(newBullet);
            reqBullet = newBullet;
        }

        reqBullet.SetActive(true);

        reqBullet.transform.position = pos;

        return reqBullet;
    }
}
