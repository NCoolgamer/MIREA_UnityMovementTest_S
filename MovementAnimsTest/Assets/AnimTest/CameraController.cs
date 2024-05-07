using UnityEngine;

namespace AnimTest
{
    /// <summary>
    /// Controls a camera target transform for cinemachine based on mouse inpuy
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        
        [SerializeField]
        private Vector3 offset;
        
        [SerializeField]
        private Vector2 upDownClamp = new Vector2(-80, 80);
        
        [SerializeField]
        private float sensitivity = 1f;
        
        private Vector2 _currentRotation;
        
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void Update()
        {
            transform.position = target.position + offset;
            
            
            _currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            _currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            _currentRotation.y = Mathf.Clamp(_currentRotation.y, upDownClamp.x, upDownClamp.y);
            
            transform.localRotation = Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0);
        }
    }
}