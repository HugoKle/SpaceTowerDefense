using UnityEngine;

public class rangeScript : MonoBehaviour
{
   public void HideVisual()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    public void ShowVisual()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
