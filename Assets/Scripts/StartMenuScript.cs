using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

    public Image title;
    public Text score;
    public Text scoreBG;
    public GameObject ayaka;
    public GameObject groundObject;
    private GameStatus prevGameStatus;
    private static float titleTime = 1.5f;
    private float time = 0.0f;
    private Vector3 screenPos;
    private Vector3 groundBound;
    private Vector3 ayakaBound;

    // Start is called before the first frame update
    void Start() {
        prevGameStatus = GameController.status;
        score.enabled = false;
        scoreBG.enabled = false;
        //Get win size
        screenPos = new Vector3(Screen.width, Screen.height, 0);
        groundBound = Camera.main.WorldToScreenPoint(groundObject.GetComponent<Renderer>().bounds.extents) - Camera.main.WorldToScreenPoint(Vector3.zero);
        RectTransform[] rectTransList = ayaka.GetComponentsInChildren<RectTransform>();
        ayakaBound = Vector3.zero;
        foreach (RectTransform rectTrans in rectTransList) {
            ayakaBound.x = Mathf.Min(ayakaBound.x, rectTrans.position.x - rectTrans.rect.width / 2);
            ayakaBound.y = Mathf.Min(ayakaBound.y, rectTrans.position.y - rectTrans.rect.height / 2);
        }
        ayakaBound = -ayakaBound;
        ayaka.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        switch (GameController.status) {
            case GameStatus.START: {
                    if (prevGameStatus == GameStatus.OVER) {
                        time = 0.0f;
                        title.enabled = true;
                        score.enabled = false;
                        scoreBG.enabled = false;
                    } else if (prevGameStatus == GameStatus.WINOVER) {
                        time = 0.0f;
                        title.enabled = true;
                        ayaka.SetActive(false);
                    }
                    break;
                }
            case GameStatus.RUN: {
                    if (time < titleTime) {
                        time += Time.deltaTime;
                    } else {
                        if (title.enabled) {
                            title.enabled = false;
                            score.enabled = true;
                            scoreBG.enabled = true;
                        }
                        score.text = string.Format("{0:D2}", GameController.score);
                        scoreBG.text = score.text;
                    }
                    break;
                }
            case GameStatus.WIN: {
                    if (prevGameStatus == GameStatus.RUN) {
                        score.enabled = false;
                        scoreBG.enabled = false;
                        ayaka.SetActive(true);
                        Vector3 genPos = new Vector3(screenPos.x + ayakaBound.x * 2, groundBound.y * 2 + ayakaBound.y, 0.0f);
                        genPos = Camera.main.ScreenToWorldPoint(genPos);
                        genPos.z = 0.0f;
                        ayaka.transform.SetPositionAndRotation(genPos, Quaternion.identity);
                    } else {
                        if (ayaka.transform.position.x > 0) {
                            ayaka.transform.Translate(Vector3.left * GameController.flySpeed * Time.deltaTime);
                        } else {
                            GameController.winover();
                        }
                    }
                    break;
                }
        }
        prevGameStatus = GameController.status;
    }

}
