using System.Collections;
using UnityEngine;

public class ModelBehaviour : MonoBehaviour {

    #region Public Field
    public Transform scalingMask;
    [Range(10, 50)]
    [Tooltip("The speed to rotate the object")]
    public float rotateSpeed = 25;
    [Tooltip("The rate of change of the model size")]
    public float zoomSpeed = 0.5f;
    [Tooltip("The minimum size of the model scale")]
    public float minScale = 0.3f;
    [Tooltip("The maximum size of the model scale")]
    public float maxScale = 1;
    [Tooltip("The coefficient to adjust the scale size")]
    public float scaleMultiplier = 2;
    #endregion

    #region Private Field
    /// <summary>
    /// To restore the previous scale factor.
    /// </summary>
    private float _currentScale = 0;
    #endregion

    #region Behaviour Management
    //--------------------------------------------------------------------------

    /// <summary>
    /// To make the pinch zoom.
    /// </summary>
    void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - 
                touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - 
                touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between 
            // the touches in each frame.
            float prevTouchDeltaMag = 
                (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = 
                (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Change the model size based on the change in distance 
            // between the touches.
            Vector3 newScale = new Vector3(1, 1, 1) * 
                deltaMagnitudeDiff * zoomSpeed;

            // Limit the max and min scale of the model.
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            scalingMask.localScale = newScale;
        }
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// To be able to rotate the 3D object on dragging.
    /// </summary>
    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotateSpeed;
        transform.Rotate(Vector3.up, -rotX, Space.Self);

        float rotY = Input.GetAxis("Mouse Y") * rotateSpeed;
        transform.Rotate(Vector3.right, -rotY, Space.Self);
    }

    //--------------------------------------------------------------------------

    /// <summary>
    /// This scale method can be removed if the pinch method works fine.
    /// </summary>
    public void Zoom(float scale)
    {
        scalingMask.localScale += new Vector3(scale - _currentScale,
            scale - _currentScale, scale - _currentScale) * scaleMultiplier;

        _currentScale = scale;
    }

    //--------------------------------------------------------------------------
    #endregion
}
