using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponsBarScript : MonoBehaviour
{
    [SerializeField] private GameObject weapon1;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private GameObject weapon3;
    public TextMeshProUGUI ammoCount1;
    public TextMeshProUGUI ammoCount2;
    public TextMeshProUGUI ammoCount3;
    
    public void RightRotateWeapons()
    {
        (weapon1.gameObject.transform.position, 
            weapon2.gameObject.transform.position, 
            weapon3.gameObject.transform.position) = (
            weapon3.gameObject.transform.position, 
            weapon1.gameObject.transform.position, 
            weapon2.gameObject.transform.position);
    }

    public void LeftRotateWeapons()
    {
        (weapon1.gameObject.transform.position, 
            weapon2.gameObject.transform.position, 
            weapon3.gameObject.transform.position) = (
            weapon2.gameObject.transform.position, 
            weapon3.gameObject.transform.position, 
            weapon1.gameObject.transform.position);
    }

}
