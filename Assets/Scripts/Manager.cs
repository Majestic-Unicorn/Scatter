using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Manager : MonoBehaviour {

    public GameObject menuCanvas;
    public GameObject playCanvas;

    public GameObject initial;

    private AudioSource menuMusic;
    private AudioSource playingMusic;

    public GameObject gem;

    private float countDown = 60;

    public GameObject text;

	// Use this for initialization
	void Start () {
        menuMusic = GetComponents<AudioSource>()[1];
        playingMusic = GetComponents<AudioSource>()[0];

        menuMusic.Play();
        playingMusic.Stop();

        initial.SetActive(false);

        playCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (countDown != 0)
            countDown -= Time.deltaTime;

        if (countDown < 0){
            countDown = 0;
            EndGame();
        }

        text.GetComponent<Text>().text = Mathf.Round(countDown).ToString();
	}

    private void EndGame() {

    }

    public void StartGame(){
        menuCanvas.SetActive(false);

        initial.SetActive(true);

        menuMusic.Stop();
        playingMusic.Play();

        playCanvas.SetActive(true);

        countDown = 60;
    }

    public void ExitGame(){
        Application.Quit();
    }
}
