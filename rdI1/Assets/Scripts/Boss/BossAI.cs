using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public GameObject badJokePrefab;
    public GameObject goodJokePrefab;
    public GameObject hintPrefab; // 提示图片预制体

    public float initialSpeed = 3.0f;
    public float maxSpeed = 10.0f;
    public float speedIncreaseInterval = 100.0f;
    private float speedIncreaseTimer = 0;
    private float currentSpeed;

    public float attackInterval = 2.0f;
    public float minAttackInterval = 0.5f;
    public float intervalDecreaseRate = 0.5f;

    public Transform playerTransform;
    public Transform attackPositionsParent;

    private Transform[] attackPositions;
    private float timer;

    void Start()
    {
        attackPositions = new Transform[attackPositionsParent.childCount];
        for (int i = 0; i < attackPositionsParent.childCount; i++)
        {
            attackPositions[i] = attackPositionsParent.GetChild(i);
        }

        currentSpeed = initialSpeed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        speedIncreaseTimer += Time.deltaTime;

        if (speedIncreaseTimer >= speedIncreaseInterval && currentSpeed < maxSpeed)
        {
            currentSpeed += 1.0f;
            speedIncreaseTimer = 0;
        }

        if (attackInterval > minAttackInterval)
        {
            attackInterval -= intervalDecreaseRate * Time.deltaTime;
            attackInterval = Mathf.Max(attackInterval, minAttackInterval);
        }

        if (timer >= attackInterval)
        {
            StartCoroutine(PrepareAndAttack());
            timer = 0;
        }
    }

    IEnumerator PrepareAndAttack()
    {
        int randomIndex = Random.Range(0, attackPositions.Length);
        Transform chosenPosition = attackPositions[randomIndex];

        // 先显示提示图片
        GameObject hint = Instantiate(hintPrefab, chosenPosition.position, Quaternion.identity);
        yield return new WaitForSeconds(1f); // 等待时间，可以调整

        Destroy(hint); // 销毁提示图片

        // 发射笑话
        Vector2 attackDirection = (playerTransform.position - chosenPosition.position).normalized;
        GameObject jokePrefab = Random.Range(0f, 1f) < 0.7f ? badJokePrefab : goodJokePrefab;
        GameObject joke = Instantiate(jokePrefab, chosenPosition.position, Quaternion.identity);
        Rigidbody2D rb = joke.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * currentSpeed; // 调整笑话的速度
    }
}