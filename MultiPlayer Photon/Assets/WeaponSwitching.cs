using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int SelectedWeapon = 0;



    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WeaponSwitch()
    {
        int previousweapon = SelectedWeapon;
        if(SelectedWeapon >= transform.childCount - 1)
            SelectedWeapon = 0;
        else
             SelectedWeapon++;
        if(previousweapon != SelectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i==SelectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}

