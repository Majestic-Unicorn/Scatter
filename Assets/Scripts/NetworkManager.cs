using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    public string gameName = "ROBOT_FELLA_TEST_GAME";
    public float refreshTime = 3.0f;

    private const string _chars = "0123456789ABCDEF";

    HostData[] _hostData;

    private string _randomName(int len){
        char[] buffer = new char[len];

        for (int i = 0; i < len; i++){
            buffer[i] = _chars[Random.Range(0, _chars.Length - 1)];
        }

        return new string(buffer);
    }

    private void StartServer(){
        Network.InitializeServer(32, 25000, false);
        MasterServer.RegisterHost(gameName, _randomName(64));
    }

    IEnumerator refeshHostList(){
        Debug.Log("Refreshing...");
        MasterServer.RequestHostList(gameName);

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

            if (_hostData.Length != 0)
                return;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 150f / 2, 25f, 150f, 30f), "Start Server"))
            StartServer();

        if (GUI.Button(new Rect(Screen.width / 2 - 150f / 2, 75f, 150f, 30f), "Join Server"))
            StartCoroutine("refeshHostList");
    }


    void OnServerInitialized()
    {
        Debug.Log("Initialized!");

        Network.Instantiate(Resources.Load("Player"), new Vector3(2.49f, 8.04f, 3.55f), Quaternion.identity, 0);
    }

    void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            Debug.Log("Registered!");
    }

    void OnConnectedToServer(){
        Network.Instantiate(Resources.Load("Player"), new Vector3(2.49f, 8.04f, 3.55f), Quaternion.identity, 0);
    }
    /*void OnDisconnectedFromServer(NetworkDisconnection info){
        
    }
    void OnFailedToConnect(NetworkConnectionError error){

    }
    void OnPlayerConnected(NetworkPlayer player)
    {

    }*/
    void OnPlayerDisconnected(NetworkPlayer player){
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }
    /*void OnFailedToConnectToMasterServer(NetworkConnectionError info){

    }
    void OnNetworkInstantiate(NetworkMessageInfo info){

    }*/

    void OnApplicationQuit(){
        if (Network.isServer){
            Network.Disconnect(200);
            MasterServer.UnregisterHost();
        }

        if (Network.isClient)
            Network.Disconnect();
    }
}
