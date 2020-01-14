using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameStatus GameStatusEnum;

    private Transform part1Obstacles, part2Obstacles, transitionObstacles;
    private Transform hole, door, pointer, plane1Obj, plane2Obj;

    [SerializeField] GameObject success, fail;

    private HoleController holeCont;
    private PointerController pointCont;

    private bool setPos = true;
    private bool isDoorOpen = false;

    [HideInInspector] public int obstacleCounter;
    private int obstacleCounterStart;

    private Image successBar;

    private void Start()
    {
        GameStatusEnum = GameStatus.START;

        part1Obstacles = GameObject.Find("Part1").transform;
        part2Obstacles = GameObject.Find("Part2").transform;
        transitionObstacles = GameObject.Find("TransitionObstacles").transform;

        door = GameObject.Find("Door").transform;

        plane2Obj = GameObject.Find("Plane2").transform;
        plane1Obj = GameObject.Find("Plane1").transform;

        pointer = GameObject.Find("Pointer").transform;
        pointCont = pointer.GetComponent<PointerController>();

        hole = GameObject.Find("HoleObj").transform;
        holeCont = hole.GetComponent<HoleController>();
        holeCont.DoorOpenValue = 1;

        obstacleCounterStart = part1Obstacles.childCount + part2Obstacles.childCount + transitionObstacles.childCount;

        successBar = GameObject.Find("Inner").transform.GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        ObstacleContol();
        switch (GameStatusEnum)
        {
            case GameStatus.START:
                GameStatusEnum = GameStatus.STAY;
                break;
            case GameStatus.STAY:

                break;
            case GameStatus.TRANSITION:
                if (holeCont.DoorOpenValue < 0.1)
                {
                    holeCont.MoveByTransition(plane2Obj);
                    if (setPos)
                    {
                        SetClampPositions();
                        setPos = false;
                    }
                }
                else
                {
                    holeCont.TranslateHole(plane1Obj);
                    DoorOpen(holeCont.DoorOpenValue);
                }
                break;
            case GameStatus.FAIL:
                fail.SetActive(true);
                break;
            case GameStatus.SUCCESS:
                success.SetActive(true);
                break;
            case GameStatus.EMPTY:
                break;
            default:
                break;
        }


    }
    private void DoorOpen(float Value)
    {
        door.position = Vector3.MoveTowards(door.position, new Vector3(door.position.x, -1, door.position.z), 1 / (Value * 100));
    }
    private void SetClampPositions()
    {
        pointCont.left = plane2Obj.Find("LeftPoint").position.x;
        pointCont.right = plane2Obj.Find("RightPoint").position.x;
        pointCont.down = plane2Obj.Find("UpPoint").position.z;
        pointCont.up = plane2Obj.Find("DownPoint").position.z;
    }
    private void ObstacleContol()
    {
        if (part1Obstacles.transform.childCount == 0 && !isDoorOpen)
        {
            GameStatusEnum = GameStatus.TRANSITION;
            isDoorOpen = true;
        }

        successBar.fillAmount = Mathf.Lerp(successBar.fillAmount, (float)obstacleCounter / obstacleCounterStart, 0.1f);

        obstacleCounter = obstacleCounterStart - (part1Obstacles.childCount + part2Obstacles.childCount + transitionObstacles.childCount);

        if (obstacleCounter == obstacleCounterStart)
        {
            GameStatusEnum = GameStatus.SUCCESS;
        }
    }
}
public enum GameStatus
{
    START, STAY, TRANSITION, FAIL, SUCCESS, EMPTY
}