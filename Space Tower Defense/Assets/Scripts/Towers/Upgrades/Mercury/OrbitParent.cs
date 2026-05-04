using UnityEngine;

public class OrbitParent : MonoBehaviour
{
    public float sharedRandomOffset;
    private void Awake()
    {
        sharedRandomOffset = Random.Range(0f, Mathf.PI * 2f);
    }
}
