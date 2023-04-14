using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject JoysticCanvas;
    public GameObject CameraHolder;
    private Animator animator;
    public Button FireButton;
    private Shooting shooting;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shooting = GetComponent<Shooting>();
        if(photonView.IsMine)
        {
            SetActiveLocalPlayer();
            GetComponent<RigidbodyFirstPersonController>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
            FireButton.onClick.AddListener(() => shooting.Fire());
            JoysticCanvas.SetActive(true);
            CameraHolder.SetActive(true);
            animator.SetBool("Soldier", true);
        }
        else
        {
            SetActiveRemotePlayer();
            GetComponent<RigidbodyFirstPersonController>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            JoysticCanvas.SetActive(false);
            CameraHolder.SetActive(false);
            animator.SetBool("Soldier", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveLocalPlayer()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).gameObject.SetActive(true);

       

    } 
    
    public void SetActiveRemotePlayer()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);

        
    }

}
