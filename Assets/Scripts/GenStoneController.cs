using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenStoneController : MonoBehaviour {

    public GameObject genStoneObject;
    private List<GameObject> genStoneList;
    private static float probRate = 0.3f;
    private float time;
    private Vector3 screenPos;
    private Vector3 genStoneBound;
    private GameStatus prevGameStatus;

    // Start is called before the first frame update
    void Start() {
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        genStoneBound = genStoneObject.GetComponent<Renderer>().bounds.extents;
        prevGameStatus = GameController.status;
        genStoneList = new List<GameObject>();
        time = -GameController.genObjectTime/2;
    }

    private void clear() {
        time = -GameController.genObjectTime / 2;
        foreach (GameObject genStone in genStoneList) {
            Destroy(genStone);
        }
        genStoneList.Clear();
    }

    // Update is called once per frame
    void Update() {
        if (GameController.status == GameStatus.RUN) {
            time += Time.deltaTime;
            if (time >= GameController.genObjectTime) {
                time = 0.0f;
                if (Random.Range(0.0f, 1.0f) <= probRate) {
                    Vector3 genPos = new Vector3(screenPos.x + genStoneBound.x, Random.Range(-1.0f, 2.5f), 0.0f);
                    genStoneList.Add(Instantiate<GameObject>(genStoneObject, genPos, Quaternion.identity, transform));
                }
            }
            for (int i = genStoneList.Count - 1; i >= 0; --i) {
                if (genStoneList[i]) {
                    genStoneList[i].transform.Translate(Vector3.left * GameController.flySpeed * Time.deltaTime);
                    if (Camera.main.WorldToScreenPoint(genStoneList[i].transform.position + Vector3.right * genStoneBound.x).x < 0) {
                        Destroy(genStoneList[i]);
                        genStoneList.RemoveAt(i);
                    }
                } else {
                    genStoneList.RemoveAt(i);
                }
            }
        } else if (GameController.status == GameStatus.START && prevGameStatus == GameStatus.OVER) {
            clear();
        } else if (GameController.status == GameStatus.WIN && prevGameStatus == GameStatus.RUN) {
            clear();
        }
        prevGameStatus = GameController.status;
    }

}
