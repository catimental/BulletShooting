using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Boss : Enemy
{
    private int maxBossHP = 100;
    private BossHPSlider hpSlider;
    public EnemyRocket enemyBullet;
    private int bossHp;
    private int directionX = -1;

    private int phase = 1;

    private float shootTime = 0;
    private float shootDelay = 1f;

    private float circularShooTime = 0;
    private float circularShootDelay = 5f;

    private float linearShootTime = 0;
    private float linearShootDelay = 15f;

    private bool isInvincibility = false;

    private bool isLastPattern = false;
    // Start is called before the first frame update
    void Start()
    {
        shootDelay = 3f;
        bossHp = maxBossHP;
        hpSlider = ObjectManager.Instance.BossHpSlider;
        UpdateSlider();
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == 4 && isInvincibility)
        {
            if (!isLastPattern)
            {
                isLastPattern = true;
                StartCoroutine(StartLastPhasePattern());
            }
        }
        else
        {
            if (shootTime >= shootDelay)
            {
                CreateTargettingBullet();

                shootTime = 0;
            }
            shootTime += Time.deltaTime;

            if (linearShootTime > linearShootDelay)
            {
                StartCoroutine(CreateLinearTargettingBullet());
                //StartCoroutine(CreateFillAllBullet());
                linearShootTime = 0;
            }

            linearShootTime += Time.deltaTime;
            if (phase >= 2)
            {
                if (circularShooTime >= circularShootDelay)
                {
                    CreateCircularBullets();
                    circularShooTime = 0;
                }
                circularShooTime += Time.deltaTime;
            }
        }

        if (!isInvincibility)
        {
            MoveControl();
        }
    }

    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Rocket")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound();
            if (!isInvincibility)
            {
                bossHp -= 10;
                UpdateSlider();



                col.gameObject.SetActive(false);
                float hpPercent = (float)bossHp / (float)maxBossHP * 100;
                if (hpPercent <= 50 && phase == 1)
                {
                    moveSpeed *= 2;
                    shootDelay /= 2;
                    phase = 2;
                }
                else if (hpPercent <= 20 && phase == 2)
                {
                    moveSpeed *= 2;
                    shootDelay /= 2;
                    phase = 3;
                }
                else if (hpPercent <= 0)
                {
                    if (phase == 3)
                    {
                        phase = 4;
                        bossHp = 10;
                        UpdateSlider();

                        isInvincibility = true;

                        var normalizedPosition = PositionHelper.Instance.GetNormalizedVector(transform.position);
                        var position = PositionHelper.Instance.GetPositionFromNormalizedVector(0.5f, normalizedPosition.y);
                        transform.position = position;
                    }
                    else
                    {
                        OnDead();
                    }
                }
            }
            
        }

    }
    private void OnDead()
    {
        hpSlider.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void UpdateSlider()
    {
        hpSlider.SetSliderValue((float)bossHp/(float)maxBossHP);
    }

    private void CreateCircularBullets()
    {
        var baseRadius = 100f;
        var baseBulletSpeed = 1.0f;
        var nestingCount = 3;
        var radiusGap = 20f;
        var bulletSpeedGap = 0.75f;
        var radAdjust = 25f;
        switch (phase)
        {
            case 2:
                nestingCount = 4;
                break;
            case 3:
                nestingCount = 5;
                break;
        }
        for (var i = 0; i < nestingCount; i++)
        {
            CreateCircularBullet(baseRadius, baseBulletSpeed, i* radAdjust);
            baseRadius -= radiusGap;
            baseBulletSpeed += bulletSpeedGap;
        }
    }

    IEnumerator StartLastPhasePattern()
    {
        var pivotPoint = Camera.main.WorldToViewportPoint(transform.position);
        pivotPoint.x = 0;
        Debug.Log("lastPhase Started");
        for (var i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
            {
                pivotPoint.x = 0f;
            }
            else
            {
                pivotPoint.x = 0.1f;
            }
            yield return CreateLinearPromptBullet(pivotPoint);

            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3f);
        yield return CreateFillAllBullet();
        yield return new WaitForSeconds(3f);
        isInvincibility = false;
    }

    IEnumerator CreateFillAllBullet()
    {
        var enemyBullets = new List<EnemyRocket>();
        var xMaxCount = 8;
        var yMaxCount = 8;
        var xCount = 0;
        var yCount = 0;
        while (yCount < yMaxCount)
        {
            xCount = 0;
            while (xCount < xMaxCount)
            {
                var pivotPosition = Camera.main.WorldToScreenPoint(transform.position);
                //pivotPosition.y = ((float)yCount / (float)yMaxCount) * 1.0f;
                pivotPosition.y = 1f - ((float) yCount / (float) yMaxCount);
                pivotPosition.x = ((float)xCount / (float)xMaxCount) * 1.0f;
                
                var worldPos = Camera.main.ViewportToWorldPoint(pivotPosition);
                var bullet = Instantiate(enemyBullet, worldPos, enemyBullet.transform.rotation);

                enemyBullets.Add(bullet);
                SoundManager.Instance.PlayLinearBulletSound();
                yield return new WaitForSeconds(0.05f);

                xCount++;
            }

            yCount++;
        }


        yield return new WaitForSeconds(1.0f);
        var random = new Random();
        foreach (var bullet in enemyBullets)
        {
            var position = new Vector2(
                (float) (random.NextDouble()),
                (float) (random.NextDouble())
            );
            position.x = Math.Min(Math.Max(0.1f, position.x), 1.0f);
            position.y = Math.Min(Math.Max(0.1f, position.y), 1.0f);

            position.x *= random.Next(0, 1 + 1) == 0 ? -1f : 1f;
            position.y *= random.Next(0, 1 + 1) == 0 ? -1f : 1f;

            bullet.SetDirectionVector(position);
            bullet.moveSpeed = 0.5f;
            bullet.IsMove = true;
        }
    }

    IEnumerator CreateLinearPromptBullet(Vector3 viewPoint)
    {
        Debug.Log("linePrompt!");
        var enemyBullets = new List<EnemyRocket>();
        var count = 0;
        var maxCount = 10;
        while (count < maxCount)
        {
            var pivotPosition = new Vector3(viewPoint.x, viewPoint.y, viewPoint.z);
            pivotPosition.y = 0.9f;
            pivotPosition.x = ((float) count / (float) maxCount) + viewPoint.x;
            var worldPos = Camera.main.ViewportToWorldPoint(pivotPosition);
            var bullet = Instantiate(enemyBullet, worldPos, enemyBullet.transform.rotation);
            enemyBullets.Add(bullet);
            count++;
        }

        yield return new WaitForSeconds(1.5f);
        foreach (var bullet in enemyBullets)
        {
            bullet.SetDirectionVector(new Vector2(0, -1));
            bullet.moveSpeed = 15;
            bullet.IsMove = true;
        }
        CameraShake.Instance.Shake();
        SoundManager.Instance.PlaySound();
    }

    IEnumerator CreateLinearTargettingBullet()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var enemyBullets = new List<EnemyRocket>();
        int count = 0;
        int maxCount = 0;
        float delay = 0f;
        switch (phase)
        {
            case 1:
                maxCount = 5;
                delay = 0.6f;
                break;
            case 2:
                maxCount = 10;
                delay = 0.4f;
                break;
            case 3:
                maxCount = 15;
                delay = 0.2f;
                break;
        }
        while (count < maxCount)
        {
            var pivotPosition = Camera.main.WorldToScreenPoint(transform.position);
            pivotPosition.y = 0.9f;
            pivotPosition.x = ((float) count / (float) maxCount) * 1.0f;
            var worldPos = Camera.main.ViewportToWorldPoint(pivotPosition);
            var bullet = Instantiate(enemyBullet, worldPos, enemyBullet.transform.rotation);

            enemyBullets.Add(bullet);
            SoundManager.Instance.PlayLinearBulletSound();
            yield return new WaitForSeconds(0.1f);
            
            count++;
        }

        yield return new WaitForSeconds(1.0f);
        foreach (var bullet in enemyBullets)
        {
            var from = player.transform.position;
            bullet.SetDirectionVector((from - bullet.transform.position).normalized);
            //bullet.SetDirectionVector(new Vector2(0, -1));
            bullet.moveSpeed = 7;
            bullet.IsMove = true;
            yield return new WaitForSeconds(delay);
        }
    }
    private void CreateCircularBullet(float radius, float movementSpeed, float radAdjust)
    {
        var bulletPositions = new List<Vector3>();
        var position = Camera.main.WorldToScreenPoint(transform.position);
        //position.y -= 40;

        float drad = (float)(Math.PI  / 360 * 30);
        
        for (float rad = radAdjust; rad < (2 * Math.PI) + radAdjust; rad += drad)
        {
            bulletPositions.Add(new Vector3(
                (float)Math.Round((position.x + radius * Math.Cos(rad)), 0),
                (float)Math.Round((position.y + radius * Math.Sin(rad)), 0),
                0
            ));
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        var from = transform.position;
        var to = player.transform.position;
        var direction = (to - from).normalized;

        foreach (var bulletPosition in bulletPositions)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(bulletPosition);
            worldPos.z = 0;
            var bullet = Instantiate(enemyBullet, worldPos, enemyBullet.transform.rotation);
            //bullet.SetDirectionVector(new Vector2(direction.x, direction.y));
            //bullet.SetDirectionVector(new Vector2(0, -1));
            bullet.SetDirectionVector((from - worldPos).normalized);
            bullet.moveSpeed = movementSpeed;
            bullet.IsMove = true;
        }
    }
    
    private void CreateTargettingBullet()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var from = transform.position;
        var to = player.transform.position;
        var direction = (to - from).normalized;
        var bullet = Instantiate(enemyBullet, transform.position, enemyBullet.transform.rotation);
        bullet.SetDirectionVector(new Vector2(direction.x, direction.y));
        bullet.moveSpeed = 1f;
        bullet.IsMove = true;
    }
    private void MoveControl()
    {
        var normalizedPosition = PositionHelper.Instance.GetNormalizedVector(transform.position);
        if (normalizedPosition.x <= 0 && directionX == -1)
        {
            directionX = 1;
        }
        else if (normalizedPosition.x >= 1 && directionX == 1)
        {
            directionX = -1;
        }
        float xMove = directionX * moveSpeed * Time.deltaTime;
        transform.Translate(xMove, 0, 0);
    }
}
