using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject cloudObject;
    private List<GameObject> cloudList;
    private static float minGenTime = 0.5f;
    private static float maxGenTime = 2.0f;
    private float time;
    private Vector3 screenPos;
    private Vector3 cloudBound;
    private GameStatus prevGameStatus;

    void Start() {
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        cloudBound = cloudObject.GetComponent<Renderer>().bounds.extents;
        prevGameStatus = GameController.status;
        cloudList = new List<GameObject>();
        time = Random.Range(minGenTime, maxGenTime);
    }

    // Update is called once per frame
    void Update() {
        if (GameController.status == GameStatus.START || GameController.status == GameStatus.RUN
            || GameController.status == GameStatus.WIN || GameController.status == GameStatus.WINOVER) {
            if (GameController.status == GameStatus.START && GameController.status != prevGameStatus) {
                time = Random.Range(minGenTime, maxGenTime);
                foreach (GameObject cloud in cloudList) {
                    Destroy(cloud);
                }
                cloudList.Clear();
            }
            time -= Time.deltaTime;
            if (time <= 0.0f) {
                time = Random.Range(minGenTime, maxGenTime);
                Vector3 genPos = new Vector3(screenPos.x + cloudBound.x, Random.Range(-1.2f, 3.0f), 0.0f);
                cloudList.Add(Instantiate<GameObject>(cloudObject, genPos, Quaternion.identity, transform));
            }
            for (int i = cloudList.Count - 1; i >= 0; --i) {
                cloudList[i].transform.Translate(Vector3.left * GameController.flySpeed * 2 * Time.deltaTime);
                if (Camera.main.WorldToScreenPoint(cloudList[i].transform.position + Vector3.right * cloudBound.x).x < 0) {
                    Destroy(cloudList[i]);
                    cloudList.RemoveAt(i);
                }
            }
        }
        prevGameStatus = GameController.status;
    }

}
