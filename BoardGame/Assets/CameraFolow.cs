using UnityEngine;

public class CameraFolow : MonoBehaviour
{

    public Camera mainCamera;
    protected Plane plane;
    private Vector3 startPosition;
    private Transform startTransform;
    private float maxDistance = 45;
    public Vector3 limits;
    private Quaternion startQuaternion;


    public bool rotate;
#if UNITY_ANDROID
    private void Awake()
    {
        mainCamera = Camera.main;
        startPosition = mainCamera.transform.position;
        startTransform = mainCamera.transform;
        startQuaternion = mainCamera.transform.rotation;

    }

    private void Update()
    {
        if (Input.touchCount >= 1)
            plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll
        if (Input.touchCount >= 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {                
                mainCamera.transform.Translate(Delta1, Space.World);
                Vector3 pos = mainCamera.transform.position;
                pos.x = Mathf.Clamp(mainCamera.transform.position.x, -limits.x, limits.x);
                pos.z = Mathf.Clamp(mainCamera.transform.position.z, -limits.x, limits.x);

                
                mainCamera.transform.position = pos;
            }
        }

        //Pinch
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2)/Vector3.Distance(pos1b, pos2b);

            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray
            mainCamera.transform.position = Vector3.LerpUnclamped(pos1, mainCamera.transform.position, 1 / zoom);

            Vector3 pos = new Vector3(mainCamera.transform.position.x, Mathf.Clamp(mainCamera.transform.position.y, 6f, 20f),mainCamera.transform.position.z);
            mainCamera.transform.position = pos;
            if (rotate && pos2b != pos2)
                mainCamera.transform.RotateAround(pos1, plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, plane.normal));
        }
    }

    private Vector3 PlanePosition(Vector2 screenPos)
    {
        var ray = mainCamera.ScreenPointToRay(screenPos);
        if (plane.Raycast(ray, out var enter))
            return ray.GetPoint(enter);

        return Vector3.zero;
    }

    private Vector3 PlanePositionDelta(Touch touch)
    {
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        var rayBefore = mainCamera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = mainCamera.ScreenPointToRay(touch.position);
        if (plane.Raycast(rayBefore, out var enterBefore) && plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }
    public void ResetCameraPosition()
    {
        //TODO: add smooth transition
        mainCamera.transform.position = startPosition;
        mainCamera.transform.rotation = startQuaternion;
    }

#endif
}