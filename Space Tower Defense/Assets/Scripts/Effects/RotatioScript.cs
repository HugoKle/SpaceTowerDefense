using UnityEngine;

public class RotatioScript : MonoBehaviour
{
    [SerializeField] float spinSpeed = 1;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, spinSpeed * 10 * Time.fixedDeltaTime);
    }
}
