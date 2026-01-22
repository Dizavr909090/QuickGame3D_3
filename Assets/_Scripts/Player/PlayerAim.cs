using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField]
    Camera _viewCamera;
    [SerializeField] 
    private LayerMask _groundLayerMask;

    private void Awake()
    {
        if (_viewCamera == null) _viewCamera = Camera.main;
    }

    private void Update()
    {
        SetAim();
    }

    private void SetAim()
    {
        Ray ray = _viewCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundLayerMask))
        {
            Vector3 point = hitInfo.point;
            Debug.DrawLine(ray.origin, point, Color.red);
            RotateTowardsAimPoint(point);
        }
    }

    private void RotateTowardsAimPoint(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
        
    }
}