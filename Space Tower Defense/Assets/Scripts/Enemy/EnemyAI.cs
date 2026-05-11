using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public float speed = 2f;
    [SerializeField] int damage = 1;

    public float distanceTravelled = 0f;
    Transform[] points;
    int currentPoint = 0;
    Rigidbody2D rb;
    bool isSlowed = false;
    bool isStunned = false;

    private void Start()
    {
        int currentWave = FindFirstObjectByType<UIScript>().GetWave();
        if (currentWave > 15)
        {
            speed += (currentWave - 15) * 0.5f;
            damage += (currentWave - 15) / 2;
        }

        points = FindFirstObjectByType<PointsList>().points;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (points == null || points.Length == 0) return;
        if (currentPoint >= points.Length) return;
        
        Vector2 dir = transform.position - points[currentPoint].position;

        float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        rb.linearVelocity = -dir.normalized * speed;
        if (dir.magnitude < 0.1f)
        {
            transform.position = points[currentPoint].position;
            currentPoint++;
            if (currentPoint >= points.Length)
            {
                FindFirstObjectByType<UIScript>().ReduceHealth(damage);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        distanceTravelled += step;
    }

    public void ReduceSpeed(float multiplier, float duration)
    {
        if (!isSlowed)
        {
            StartCoroutine(ReduceSpeedCoroutine(multiplier, duration));
        }
    }

    IEnumerator ReduceSpeedCoroutine(float multiplier, float duration)
    {
        float originalSpeed = speed;
        speed *= multiplier;
        isSlowed = true;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        isSlowed = false;
    }
    public void StunEnemy(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(Stun(duration));
        }
    }

    IEnumerator Stun(float duration)
    {
        float originalSpeed = speed;
        speed = 0f;
        isStunned = true;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        isStunned = false;
    }

}
