using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour {

    public GameObject slimeObject;
    private List<GameObject> slimeList;
    private static float probRate = 0.5f;
    private float time;
    private Vector3 screenPos;
    private Vector3 slimeBound;
    private GameStatus prevGameStatus;

    // Start is called before the first frame update
    void Start() {
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        slimeBound = slimeObject.GetComponentInChildren<Renderer>().bounds.extents;
        prevGameStatus = GameController.status;
        slimeList = new List<GameObject>();
        time = -GameController.genObjectTime / 2;
    }

    private void clear() {
        time = -GameController.genObjectTime / 2;
        foreach (GameObject slime in slimeList) {
            Destroy(slime);
        }
        slimeList.Clear();
    }

    // Update is called once per frame
    void Update() {
        if (GameController.status == GameStatus.RUN) {
            time += Time.deltaTime;
            if (time >= GameController.genObjectTime) {
                time = 0.0f;
                if (Random.Range(0.0f, 1.0f) <= probRate) {
                    Vector3 genPos = new Vector3(screenPos.x + slimeBound.x, 0.0f, 0.0f);
                    slimeList.Add(Instantiate<GameObject>(slimeObject, genPos, Quaternion.identity, transform));
                }
            }
            for (int i = slimeList.Count - 1; i >= 0; --i) {
                slimeList[i].transform.Translate(Vector3.left * GameController.flySpeed * Time.deltaTime);
                if (Camera.main.WorldToScreenPoint(slimeList[i].transform.position + Vector3.right * slimeBound.x).x < 0) {
                    Destroy(slimeList[i]);
                    slimeList.RemoveAt(i);
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
