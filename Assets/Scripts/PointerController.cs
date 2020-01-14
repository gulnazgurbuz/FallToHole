using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    public Camera _camera;
    public GameObject HoleObj;


    LayerMask placeHolder;
    private RaycastHit hit;
    private Ray rayFromCamera;


    private Vector3 difference;
    [HideInInspector] public Vector3 vecRed;
    [HideInInspector] public float left, right, up, down;

    private GameObject plane1Obj;

    private void Start()
    {
        plane1Obj = GameObject.Find("Plane1"); ;
        placeHolder = LayerMask.GetMask("PlaceHolderLayer");
        left = plane1Obj.transform.Find("LeftPoint").position.x;
        right = plane1Obj.transform.Find("RightPoint").position.x;
        down = plane1Obj.transform.Find("UpPoint").position.z;
        up = plane1Obj.transform.Find("DownPoint").position.z;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rayFromCamera = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayFromCamera, out hit, 100, placeHolder))
                difference = transform.position - hit.point;
        }

        if (Input.GetMouseButton(0))
        {
            rayFromCamera = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayFromCamera, out hit, 100, placeHolder))
            {
                //I drew beam from the camera to "placeholder"  (the child object of the camera),  and found where it was clicked.
                Debug.DrawLine(rayFromCamera.origin, hit.point, Color.white);
                vecRed = hit.point + difference;
                vecRed.Set(vecRed.x, transform.position.y, vecRed.z);

                //Then I equalized the x and z values of this object to the x and z values of the clicked point.
                transform.position = vecRed;

                //And I clamped it in a certain range.
                transform.position = new Vector3(Mathf.Clamp(vecRed.x, left, right),
                                                       transform.position.y,
                                                         Mathf.Clamp(vecRed.z, up, down));
                //I took the difference so that the position of the Hole Object did not change with each click and I kept this difference constant.
                difference = transform.position - hit.point;
                
                if (GameController.GameStatusEnum == GameStatus.STAY)
                    HoleObj.transform.position = new Vector3(transform.position.x,
                                                                 HoleObj.transform.position.y,
                                                                    transform.position.z);
                
            }
        }

        if (Physics.Raycast(transform.position, -transform.up, out hit))
            Debug.DrawLine(transform.position, hit.point, Color.blue);


    }
}



