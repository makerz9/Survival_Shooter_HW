using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    private Camera mainCamera;
    private Plane groundPlane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointToGo = ray.GetPoint(rayDistance);
            Vector3 targetDirection = pointToGo - transform.position;
            targetDirection.y = 0;

            if(targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            }
        }


    }


}
