using System.Collections;
using UnityEngine;

public class VenusScript : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int attackspeed;
    [SerializeField] float attackInterval;
    [SerializeField] float range;
    [SerializeField] ParticleSystem attackEffect;
    [SerializeField] float radius;
    [SerializeField] float duration;

    bool canAttack = true;

    private void FixedUpdate()
    {
        FindTarget();
    }


    void FindTarget()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"));
        
        GameObject target = null;

        foreach (var enemy in enemiesHit)
        {
            float distanceTravelled = enemy.GetComponent<EnemyAI>().distanceTravelled;
            if (target == null || distanceTravelled > target.GetComponent<EnemyAI>().distanceTravelled)
            {
                target = enemy.gameObject;
            }
        }

        if (target != null)
        {
            Attack(target.transform);
        }
    }
    void Attack(Transform target)
    {
        if (canAttack)
        {
            ParticleSystem currentEffect = Instantiate(attackEffect);
            currentEffect.transform.position = target.position;

            var main = currentEffect.main;
            var shape = currentEffect.shape;
            main.duration = duration;
            shape.radius = radius;


            currentEffect.Play();
            canAttack = false;

            StartCoroutine(DamageEnemy(target.position));

            Invoke(nameof(ResetAttack), 10f / attackspeed);

            Destroy(currentEffect.gameObject, duration + 0.1f);
        }
    }

    IEnumerator DamageEnemy(Vector2 target)
    {
        float elapsed = duration;
        yield return null;
        while (elapsed > 0)
        {
            yield return new WaitForSeconds(attackInterval);
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(target, radius, LayerMask.GetMask("Enemy"));
            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            elapsed -= attackInterval;
        }   
    }




    void ResetAttack()
    {
        canAttack = true;
    }
}
