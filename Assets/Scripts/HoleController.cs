using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    [HideInInspector] public float DoorOpenValue = 1;

    public void TranslateHole(Transform plane1)
    { 
        //The code that allows the hole object to align with the door after the end of Part 1.
        DoorOpenValue = Vector3.Distance(new Vector3(transform.position.x, 1, 1), new Vector3(plane1.position.x, 1, 1));
        Vector3 toDoor = new Vector3(plane1.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, toDoor, 0.01f);
    }

    public void MoveByTransition(Transform plane2)
    {
        //Code running down the hallway after finishing part1.
        Vector3 toPlane2 = new Vector3(transform.position.x, transform.position.y, plane2.position.z);
        transform.position = Vector3.MoveTowards(transform.position, toPlane2, 0.2f);

        Vector3 toCamPlane2 = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,plane2.position.z-2);
        Camera.main.transform.position=Vector3.MoveTowards(Camera.main.transform.position, toCamPlane2, 0.2f);

        if (Vector3.Distance(transform.position, toPlane2) < 0.1f && 
            Vector3.Distance(Camera.main.transform.position, toCamPlane2) < 0.1f)
            GameController.GameStatusEnum = GameStatus.START;
    }
}
