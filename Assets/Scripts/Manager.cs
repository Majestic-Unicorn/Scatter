using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Manager : MonoBehaviour {

    public GameObject menuCanvas;
    public GameObject playCanvas;
    public GameObject pauseCanvas;
    public GameObject endCanvas;

    public GameObject initial;

    private AudioSource menuMusic;
    private AudioSource playingMusic;

    public GameObject gem;

    private float countDown = 60;

    private bool playing = false;

    public GameObject text;

    private bool easing = false;

    private GameObject camera;

    private bool escPressed = false;

    private bool paused = false;

	// Use this for initialization
	void Start () {
        menuMusic = GetComponents<AudioSource>()[1];
        playingMusic = GetComponents<AudioSource>()[0];

        menuMusic.Play();
        playingMusic.Stop();

        initial.SetActive(false);

        playCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);

        camera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        if (playing){
            if (countDown != 0)
                countDown -= Time.deltaTime;

            if (countDown < 0)
            {
                countDown = 0;
                EndGame();
            }

            text.GetComponent<Text>().text = Mathf.Round(countDown).ToString();
        }

        if (!escPressed && Input.GetKeyDown(KeyCode.Escape) && playCanvas.activeSelf){
            escPressed = true;

            if (!paused) {
                paused = true;
                pauseCanvas.SetActive(true);
                playing = false;

                foreach (PlayerController player in initial.GetComponentsInChildren<PlayerController>()){
                    player.Freeze();
                }
            }
            else if (paused){
                paused = false;
                pauseCanvas.SetActive(false);
                playing = true;

                foreach (PlayerController player in initial.GetComponentsInChildren<PlayerController>()){
                    player.Unfreeze();
                }
            }
        }
        else if (escPressed && !Input.GetKeyDown(KeyCode.Escape)) {
            escPressed = false;
        }
	}

    void FixedUpdate() {
        if (easing){
            Vector3 target = gem.transform.position - new Vector3(0, -50, 50);

            camera.transform.position = Vector3.Lerp(camera.transform.position, target, 0.1f);

            camera.transform.LookAt(gem.transform.position);
        }
    }

    private void EndGame() {
        foreach (PlayerController player in initial.GetComponentsInChildren<PlayerController>()){
            player.Freeze();
        }

        camera.GetComponent<MultiCamera>().enabled = false;

        easing = true;

        playCanvas.SetActive(false);
        
        menuMusic.Play();
        playingMusic.Stop();

        playing = false;

        endCanvas.SetActive(true);
    }

    public void ResetGame(){
        easing = false;

        gem.transform.parent = null;

        camera.GetComponent<MultiCamera>().enabled = true;

        endCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        foreach (PlayerController player in initial.GetComponentsInChildren<PlayerController>()){
                //player.transform.gameObject.SetActive(true);
                player.Unfreeze();
                player.Reset();
        }

        gem.GetComponent<Gem>().Reset();

        StartGame();
    }

    public void StartGame(){
        playing = true;

        menuCanvas.SetActive(false);

        initial.SetActive(true);

        menuMusic.Stop();
        playingMusic.Play();

        playCanvas.SetActive(true);

        countDown = 10;
    }

    public void ExitGame(){
        Application.Quit();
    }
}
