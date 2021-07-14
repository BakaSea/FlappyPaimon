using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

    public GameObject groundObject;
    private List<GameObject> groundList;
    private Vector3 screenPos;
    private Vector3 groundBound;
    private float lastX;
    private GameStatus prevGameStatus;

    private void extendGround() {
        while (lastX < screenPos.x) {
            Vector3 genPos = new Vector3(lastX + groundBound.x, -screenPos.y + groundBound.y, 0.0f);
            groundList.Add(Instantiate<GameObject>(groundObject, genPos, Quaternion.identity, transform));
            lastX += groundBound.x * 2;
        }
    }

    // Start is called before the first frame update
    void Start() {
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        groundBound = groundObject.GetComponent<Renderer>().bounds.extents;
        groundList = new List<GameObject>();
        lastX = -screenPos.x;
        extendGround();
        prevGameStatus = GameController.status;
    }

    // Update is called once per frame
    void Update() {
        if (GameController.status == GameStatus.START || GameController.status == GameStatus.RUN || GameController.status == GameStatus.WIN) {
            if (GameController.status == GameStatus.START && GameController.status != prevGameStatus) {
                foreach (GameObject ground in groundList) {
                    Destroy(ground);
                }
                groundList.Clear();
                lastX = -screenPos.x;
                extendGround();
            } else {
                for (int i = groundList.Count-1; i >= 0; --i) {
                    groundList[i].transform.Translate(Vector3.left * GameController.flySpeed * Time.deltaTime);
                    if (Camera.main.WorldToScreenPoint(groundList[i].transform.position+Vector3.right*groundBound.x).x < 0) {
                        Destroy(groundList[i]);
                        groundList.RemoveAt(i);
                    }
                }
                lastX -= GameController.flySpeed * Time.deltaTime;
                extendGround();
            }
        }
        prevGameStatus = GameController.status;
    }

}
