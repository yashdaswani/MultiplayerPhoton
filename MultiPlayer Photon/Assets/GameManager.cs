using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            int randomnumber = Random.Range(-18,0);
            PhotonNetwork.Instantiate(playerPrefab.name,new Vector3(randomnumber,0,0),Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
