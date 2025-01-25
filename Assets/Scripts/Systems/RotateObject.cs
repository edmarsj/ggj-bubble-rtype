using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
   
    private void LateUpdate()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }
}
