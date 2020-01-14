using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTriggerController : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ObstacleTag")
        {
            other.attachedRigidbody.isKinematic = false;
        }
        if (other.tag == "FailObstacleTag"&& GameController.GameStatusEnum == GameStatus.STAY)
        {
            other.attachedRigidbody.isKinematic = false;
            GameController.GameStatusEnum = GameStatus.FAIL;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FailObstacleTag" && GameController.GameStatusEnum == GameStatus.STAY)
        {
            StartCoroutine(DestroyObstacles(other.gameObject));
        }
        if (other.tag == "ObstacleTag")
        {
            StartCoroutine(DestroyObstacles(other.gameObject));
        }
    }

    IEnumerator DestroyObstacles(GameObject obj)
    {
        yield return new WaitForSeconds(1);
        Destroy(obj);
    }
}
