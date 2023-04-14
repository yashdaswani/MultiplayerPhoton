using Photon.Voice.PUN.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Animator anim;
    private bool isScopped = false;
    public GameObject WeaponCamera;
    public GameObject Overlay;
    public Camera Camera;
    public float scopedFov = 15f;
    private float normalFov;

    public void Scoping()
    {
        isScopped = !isScopped;
        anim.SetBool("Scoped", isScopped);
        if (isScopped )
        {
            StartCoroutine(Scopped());
        }
        else
        {
            unscopped();
        }
    }

    void unscopped()
    {
        Overlay.SetActive(false);
        WeaponCamera.SetActive(true);
        Camera.fieldOfView = normalFov;
    }

    IEnumerator Scopped()
    {
        yield return new WaitForSeconds(0.15f);
        Overlay.SetActive(true);
        WeaponCamera.SetActive(false);

        normalFov = Camera.fieldOfView;
        Camera.fieldOfView = scopedFov;
    }


}
