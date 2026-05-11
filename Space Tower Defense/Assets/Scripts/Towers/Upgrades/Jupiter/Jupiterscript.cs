using System.Collections;
using UnityEngine;

public class Jupiterscript : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int attackspeed;
    [SerializeField] float attackInterval;
    [SerializeField] float range;
    [SerializeField] ParticleSystem attackEffect;
    [SerializeField] float radius;
    [SerializeField] float stunDuration;
    [SerializeField] int trapHealth;
    [SerializeField] float duration;

    Rigidbody2D rb;
    bool canAttack = true;

    private void Start()
    {
        FindTarget();
    }

    void FindTarget()
    {
        bool foundPath = false;
        RaycastHit2D pathHit = new RaycastHit2D();

        Collider2D path = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Path"));

        while (!foundPath && path != null)
        { 
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Debug.DrawRay(transform.position, direction * range, Color.red, 1f);
            pathHit = Physics2D.Raycast(transform.position, direction, range, LayerMask.GetMask("Path"));
            if (pathHit.collider != null)
            {
                foundPath = true;
                break;
            }
        }
    
        if (pathHit.collider != null)
        {
            Attack(pathHit.point + (Vector2)pathHit.normal * -0.1f);
        }

        Invoke(nameof(FindTarget), 10f / attackspeed);
    }

    void Attack(Vector2 target)
    {
        ParticleSystem currentEffect = Instantiate(attackEffect);
        currentEffect.transform.position = target;

        var main = currentEffect.main;
        var shape = currentEffect.shape;
        shape.radius = radius;


        currentEffect.Play();
        canAttack = false;

        StartCoroutine(DamageEnemy(target, currentEffect));

        Destroy(currentEffect.gameObject, duration + 0.1f);

    }

    IEnumerator DamageEnemy(Vector2 target, ParticleSystem effect)
    {
        int currentTrapHealth = trapHealth;

        yield return null;
        while (currentTrapHealth > 0)
        {
            bool enemyHit = false;
           
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(target, radius, LayerMask.GetMask("Enemy"));
            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<EnemyAI>().StunEnemy(stunDuration);
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                enemyHit = true;
            }
            if (enemyHit)
            {
                currentTrapHealth -= damage;
                yield return new WaitForSeconds(attackInterval);
            }
            yield return null;
        }

        Destroy(effect.gameObject);
        
    }
}
