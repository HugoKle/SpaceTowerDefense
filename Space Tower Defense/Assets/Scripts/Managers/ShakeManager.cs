using Unity.Cinemachine;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
   public static ShakeManager Instance { get; private set; }
   

    public void ShakeCamera(float duration, float intensity)
    {
        GameObject[] cameraShakeControllers = GameObject.FindGameObjectsWithTag("Camera");

        CameraShakeController cameraShakeController = null;
        float highestPriority = -100000;

        foreach (GameObject controller in cameraShakeControllers)
        {
            CinemachineCamera cam = controller.GetComponent<CinemachineCamera>();
            CameraShakeController shaker = controller.GetComponent<CameraShakeController>();

            if (cameraShakeController == null || cam.Priority.Value > highestPriority)
            {
                highestPriority = cam.Priority.Value;
                cameraShakeController = shaker;
            }
        }



        cameraShakeController.ShakeCamera(duration, intensity);
    }
}
