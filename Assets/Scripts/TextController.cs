using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour
{
    private TextMeshProUGUI text;
    public GameObject textContinue;
    public GameObject player;
    public GameObject enemyShip;
    public GameObject FriendlyShip;
    public List<GameObject> PDCs;
    private int dialogueNum = 0;
    public List<string> dialogue;
    private bool begun = false;
    private int PDCcount = 0;
    private Touch touch;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Pilot, I'm seing something on the radar. Maintain course, but try to avoid getting too close.";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0); }

        if (Input.GetKeyDown(KeyCode.Return) || touch.phase == TouchPhase.Ended && !begun)
        {
            if (dialogueNum < dialogue.Count)
            {
                text.text = dialogue[dialogueNum];
                if (dialogueNum == 1)
                {
                    FriendlyShip.GetComponent<FriendlyShipController>().thrusting = false;
                }
            }
            else
            {
                player.SetActive(true);
                Camera.main.GetComponent<CameraController>().SwitchToPlayer();
                text.text = "";
                textContinue.SetActive(false);
                enemyShip.GetComponent<EnemyShipController>().started = true;
                for (int i = 0; i < PDCs.Count; i++)
                {
                    PDCs[i].GetComponent<PDC>().enabled = true;
                }
                begun = true;
            }
            dialogueNum++;
        }
        touch.phase = 0;
    }

    public void PDCdestroyed()
    {
        PDCcount++;
        if (PDCcount >= 4)
        {
            text.text = "You disabled their ship! We win! Our engines are back online; let's get out of here before more come!";
        }
    }
}
