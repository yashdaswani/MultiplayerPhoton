using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    private int isUserLoggedin;

    public TMP_InputField username;
    public TMP_InputField roomname;
    public TMP_InputField MaxNo;
    public GameObject PlayerNamePanel;
    public GameObject RoomCreatePanel;
    public GameObject LobbyPanel;
    public GameObject ConnectingPanel;
    public GameObject RoomListPanel;
    public Dictionary<string, RoomInfo> roomListdata;
    public GameObject RoomListPrefab;
    public GameObject roomListParent;

    public Dictionary<string, GameObject> roomListgameObject;
    public Dictionary<int, GameObject> playerListgameObject;

    [Header("Room")]
    public GameObject InsideRoomPanel;
    public GameObject playerListItemprefab;
    public GameObject playerListItemprefabParent;
    public GameObject PlayButton;

    #region
    private void Start()
    {
        
        roomListdata = new Dictionary<string, RoomInfo>();
        roomListgameObject = new Dictionary<string, GameObject>();
        PhotonNetwork.AutomaticallySyncScene = true;
        isUserLoggedin = PlayerPrefs.GetInt("isUserLoggedin",0);
        if(isUserLoggedin == 0)
        {
            ActivateMyPanel(PlayerNamePanel.name);
        }
        else
        {
            string username = PlayerPrefs.GetString("username");
            PhotonNetwork.LocalPlayer.NickName = username;
            PhotonNetwork.ConnectUsingSettings();
            ActivateMyPanel(ConnectingPanel.name);
        }
    }
    #endregion


    #region UIMethods
    public void OnLoginClick()
    {
        string name = username.text;
        

        if(!string.IsNullOrEmpty(name) )
        {
            PlayerPrefs.SetString("username", name);
            PhotonNetwork.LocalPlayer.NickName = name;
            PhotonNetwork.ConnectUsingSettings();
            ActivateMyPanel(ConnectingPanel.name);
            PlayerPrefs.SetInt("isUserLoggedin", 1);
        }
        else
        {
             name = "Bot";
            PlayerPrefs.SetString("username", name);
            PhotonNetwork.LocalPlayer.NickName = name;
            PhotonNetwork.ConnectUsingSettings();
            ActivateMyPanel(ConnectingPanel.name);
        }
    }

    public void OnCreateRoomClick()
    {
        string roomName = roomname.text;
        if(string.IsNullOrEmpty(roomName) )
        {
            roomName = roomName + Random.Range(1, 1000);
        }
            
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(MaxNo.text);
        PhotonNetwork.CreateRoom(roomName,roomOptions);
        
    }

    public void onclickCancel()
    {
        ActivateMyPanel(LobbyPanel.name);
    }
    
    public void RoomListclick()
    {
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivateMyPanel(RoomListPanel.name);
    }

    public void BackFromRoomList()
    {
        if(PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        ActivateMyPanel(LobbyPanel.name);
    }
    
    public void BackFromPlayerList()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        ActivateMyPanel(LobbyPanel.name);
    }




    #endregion

    #region PhotonCallbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log( PhotonNetwork.LocalPlayer.NickName + " : Connected to photon");
        ActivateMyPanel(LobbyPanel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + "is created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " : room joined");
        ActivateMyPanel(InsideRoomPanel.name);

        if(playerListgameObject == null)
        {
            playerListgameObject = new Dictionary<int, GameObject>();
        }

        if(PhotonNetwork.IsMasterClient)
        {
            PlayButton.SetActive(true);
        }
        else
        {
            PlayButton.SetActive(false);
        }

        foreach(Player p in PhotonNetwork.PlayerList)
        {
            GameObject playerlistitem = Instantiate(playerListItemprefab);
            playerlistitem.transform.SetParent(playerListItemprefabParent.transform);
            playerlistitem.transform.localScale = Vector3.one;

            playerlistitem.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = p.NickName;
            if(p.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerlistitem.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                playerlistitem.transform.GetChild(1).gameObject.SetActive(false);
            }

            playerListgameObject.Add(p.ActorNumber, playerlistitem);

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerlistitem = Instantiate(playerListItemprefab);
        playerlistitem.transform.SetParent(playerListItemprefabParent.transform);
        playerlistitem.transform.localScale = Vector3.one;

        playerlistitem.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = newPlayer.NickName;
        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerlistitem.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            playerlistitem.transform.GetChild(1).gameObject.SetActive(false);
        }

        playerListgameObject.Add(newPlayer.ActorNumber, playerlistitem);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListgameObject[otherPlayer.ActorNumber]);
        playerListgameObject.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.IsMasterClient)
        {
            PlayButton.SetActive(true);
        }
        else
        {
            PlayButton.SetActive(false);
        }
    }


    public void onclickPlayButton()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene(1);
            
            
        }
    }

    void LoadGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }


    public override void OnLeftRoom()
    {
        ActivateMyPanel(LobbyPanel.name);
        foreach(GameObject obj in playerListgameObject.Values)
        {
            Destroy(obj);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearListRoomList();


        foreach(RoomInfo roomInfo in roomList)
        {
            Debug.Log("Room : " +roomInfo.Name);
            if(!roomInfo.IsOpen || !roomInfo.IsVisible || roomInfo.RemovedFromList)
            {
                if(roomListdata.ContainsKey(roomInfo.Name))
                {
                    roomListdata.Remove(roomInfo.Name);
                }
            }
            else
            {
                if(roomListdata.ContainsKey(roomInfo.Name))
                {
                    roomListdata[roomInfo.Name] = roomInfo;
                }
                else
                {
                    roomListdata.Add(roomInfo.Name, roomInfo);
                }
            }
           
        }

        foreach(RoomInfo roomItem in roomListdata.Values)
        {
            GameObject roomListItemObject = Instantiate(RoomListPrefab);
            roomListItemObject.transform.SetParent(roomListParent.transform);
            roomListItemObject.transform.localScale = Vector3.one;
            roomListItemObject.transform.localPosition = Vector3.zero;
            roomListItemObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = roomItem.Name;
            roomListItemObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = roomItem.PlayerCount + "/" + roomItem.MaxPlayers;
            roomListItemObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => RoomJoinFromList(roomItem.Name));
            roomListgameObject.Add(roomItem.Name, roomListItemObject);
        }
    }

    public override void OnLeftLobby()
    {
        ClearListRoomList();
        roomListdata.Clear();
    }


    #endregion


    #region Public_Method

    public void ActivateMyPanel(string panelname)
    {
        LobbyPanel.SetActive(panelname.Equals(LobbyPanel.name));
        PlayerNamePanel.SetActive(panelname.Equals(PlayerNamePanel.name));
        RoomCreatePanel.SetActive(panelname.Equals(RoomCreatePanel.name));
        ConnectingPanel.SetActive(panelname.Equals(ConnectingPanel.name));
        RoomListPanel.SetActive(panelname.Equals(RoomListPanel.name));
        InsideRoomPanel.SetActive(panelname.Equals(InsideRoomPanel.name));
    }

    public void RoomJoinFromList(string roomName)
    {
        if(PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
            PhotonNetwork.JoinRoom(roomName);
    }


    public void ClearListRoomList()
    {
        if(roomListgameObject.Count > 0)
        {
            foreach (var v in roomListgameObject.Values)
            {
                Destroy(v);
            }

            roomListgameObject.Clear();
        }
        
    }

    #endregion


}
