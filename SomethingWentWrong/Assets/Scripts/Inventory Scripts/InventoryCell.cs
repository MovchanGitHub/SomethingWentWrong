using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    public ItemsBase item;
    public int amount;
    public GameObject icon;

    
    private void Start()
    {
        icon.GetComponent<Image>().sprite = item.image;
    }
    
}
