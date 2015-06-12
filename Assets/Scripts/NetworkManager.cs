using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    public string gameName = "ROBOT_FELLA_TEST_GAME";
    public float refreshTime = 3.0f;

    HostData[] _hostData;

    private void StartServer(){
        Network.InitializeServer(32, 25000, false);
        MasterServer.RegisterHost(gameName, "Robot Fella Test Game", "3rd person multiplayer test.");
    }

    void OnServerInitialized(){
        Debug.Log("Initialized!");
    }

    void OnMasterServerEvent(MasterServerEvent masterServerEvent){
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            Debug.Log("Registered!");
    }

    IEnumerator refeshHostList(){
        Debug.Log("Refreshing...");
        MasterServer.RequestHostList(gameName);

        float start = Time.time;
        float end = Time.time + refreshTime;

        while (Time.time < end)
        {
            _hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }

        if (_hostData == null || _hostData.Length == 0)
            Debug.Log("No active servers!");
    }

    void OnGUI() {
        if (Network.isServer || Network.isClient)
            return;

        if (_hostData != null){
            for (int i = 0; i < _hostData.Length; i++){
                if (GUI.Button(new Rect(25f, 25f + (25f * i), Screen.width - 25f * 2, 20f), _hostData[i].gameName))
                    Network.Connect(_hostData[i]);
            }
        }

        if (GUI.Button(new Rect(25f, 25f, 150f, 30f), "Start Server"))
            StartServer();

        if (GUI.Button(new Rect(25f, 75f, 150f, 30f), "Join Server"))
            StartCoroutine("refeshHostList");
    }

	void Start () {
	
	}

	void Update () {
	
	}
}
