using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun instance;
    public Animator animator;
    public int maxAmmo = 10;
   
    public int currentAmmo;
    public float reloadTime = 8f;
    public bool isReloading = false;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
            currentAmmo = maxAmmo;
        
    }

    // Update is called once per frame
    

    
}
