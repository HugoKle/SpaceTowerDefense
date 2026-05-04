using UnityEngine;

public class mercury1Script : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float spinSpeed = 1;
    [SerializeField] float distance = 1;

    Rigidbody2D rb;
    float phaseOffset;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        int index = transform.GetSiblingIndex();
        int totalSiblings = transform.parent.childCount;
        float sharedOffset = transform.parent.GetComponent<OrbitParent>().sharedRandomOffset;

        phaseOffset = (Mathf.PI * 2f / totalSiblings) * index + sharedOffset;

    }

    private void FixedUpdate()
    {
        float angle = Time.fixedTime * spinSpeed + phaseOffset;
        transform.Rotate(0, 0, spinSpeed * 10 * Time.fixedDeltaTime);
        Vector2 offset = new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
        rb.MovePosition((Vector2)transform.parent.position + offset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }

}
