using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicToGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadGame", 8.5f);
    }


    void LoadGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
