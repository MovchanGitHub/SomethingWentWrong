using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponsBarScript : MonoBehaviour
{
    [SerializeField] private int bombsCount;
    [SerializeField] private int crystalCount;
    [SerializeField] private int sunflowersCount;
    [SerializeField] private GameObject weapon1;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private GameObject weapon3;
    [SerializeField] private TextMeshProUGUI ammoCount1;
    [SerializeField] private TextMeshProUGUI ammoCount2;
    [SerializeField] private TextMeshProUGUI ammoCount3;
    

    private GameManager _gameManager;
    

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
