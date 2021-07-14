using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour {

    public GameObject pipeObject;
    private List<GameObject> pipeList;
    private float time;
    private Vector3 screenPos;
    private Vector3 pipeBound;
    private GameStatus prevGameStatus;

    // Start is called before the first frame update
    void Start() {
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        pipeBound = pipeObject.GetComponent<Renderer>().bounds.extents;
        prevGameStatus = GameController.status;
        pipeList = new List<GameObject>();
        time = 0.0f;
    }

    private void clear() {
        time = 0.0f;
        foreach (GameObject pipe in pipeList) {
            Destroy(pipe);
        }
        pipeList.Clear();
    }

    // Update is called once per frame
    void Update() {
        if (GameController.status == GameStatus.RUN) {
            time += Time.deltaTime;
            if (time >= GameController.genObjectTime) {
                time = 0.0f;
                Vector3 genPos = new Vector3(screenPos.x + pipeBound.x, Random.Range(-1.0f, 2.0f), 0.0f);
                pipeList.Add(Instantiate<GameObject>(pipeObject, genPos, Quaternion.identity, transform));
            }
            for (int i = pipeList.Count-1; i >= 0; --i) {
                pipeList[i].transform.Translate(Vector3.left * GameController.flySpeed * Time.deltaTime);
                if (Camera.main.WorldToScreenPoint(pipeList[i].transform.position+Vector3.right*pipeBound.x).x < 0) {
                    Destroy(pipeList[i]);
                    pipeList.RemoveAt(i);
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
