using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

    public GameObject menu;

    public GameObject initial;

    private AudioSource menuMusic;
    private AudioSource playingMusic;

    private float countDown = 60;

	// Use this for initialization
	void Start () {
        menuMusic = GetComponents<AudioSource>()[1];
        playingMusic = GetComponents<AudioSource>()[0];

        menuMusic.Play();
        playingMusic.Stop();

        initial.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        countDown -= Time.deltaTime;

        if (countDown < 0)
            countDown = 0;
	}

    public void StartGame(){
        menu.SetActive(false);

        initial.SetActive(true);

        menuMusic.Stop();
        playingMusic.Play();
    }

    public void ExitGame(){
        Application.Quit();
    }
}
