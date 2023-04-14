using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject buttetEffectPrefab;

    [Header("Health")]
    public float startHealth = 100;
    public float health;
    public Image healthSlider;
    private Animator animator;
    public AudioSource fireSound;
    

    private void Start()
    {
        health = startHealth;
        healthSlider.fillAmount = health / startHealth;
        animator = GetComponent<Animator>();
    }


    public void Fire()
    {

        if (Gun.instance.isReloading)
            return;

        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if(Gun.instance.currentAmmo <=0f)
        {
            StartCoroutine(Reload());
            return;
        }
        Gun.instance.currentAmmo--;
        if (Physics.Raycast(ray, out hit,100))
        {
            
           //Debug.Log(hit.collider.gameObject.name);
            photonView.RPC("CreateEffect",RpcTarget.All,hit.point);
            
            if(hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("DamageHealth",RpcTarget.AllBuffered,10f);

            }

        }
    }


   IEnumerator Reload()
    {
        Gun.instance.isReloading = true;
        Gun.instance.animator.SetBool("Reload", true);
        yield return new WaitForSeconds(Gun.instance.reloadTime);
        Gun.instance.animator.SetBool("Reload", false);
        Gun.instance.currentAmmo = Gun.instance.maxAmmo;
        Gun.instance.isReloading = false;
    }



    [PunRPC]
    public void DamageHealth(float damage , PhotonMessageInfo photonMessageInfo)
    {
        health -= damage;
        healthSlider.fillAmount = health / startHealth;
        if(health <= 0)
        {
            //die
            Die();
            Debug.Log(photonMessageInfo.Sender.NickName + "to" + photonMessageInfo.photonView.Owner.NickName);
        }
    }

    public void Die()
    {
        animator.SetBool("IsDeath", true);
        StartCoroutine(Respawn());
    }
    

    IEnumerator Respawn()
    {
        GameObject restext = GameObject.Find("CountDown_respwan");
        float respwan_time = 8f;
        while(respwan_time > 0f)
        {
            yield return new WaitForSeconds(1f);
            respwan_time -= 1;
            restext.GetComponent<TMP_Text>().text =  "Player Respawing in : " + respwan_time.ToString("0.00");
            
            GetComponent<PlayerController>().enabled = false;
        }
        animator.SetBool("IsDeath", false);
        restext.GetComponent<TMP_Text>().text = "";
        float randomPoint = Random.Range(-18, 0);
        transform.position = new Vector3 (randomPoint, 0, randomPoint);
        GetComponent<PlayerController>().enabled = true;

        //Health 100
        photonView.RPC("ReGainHealth", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void ReGainHealth()
    {
        health = startHealth;
        healthSlider.fillAmount = health / startHealth;
    }


    [PunRPC]
    public void CreateEffect(Vector3 position)
    {
        GameObject go = Instantiate(buttetEffectPrefab, position , Quaternion.identity);
        fireSound.Play();
        Destroy(go, 1f);
    }
}
