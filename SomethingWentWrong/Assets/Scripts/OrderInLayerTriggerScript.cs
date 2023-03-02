using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerTriggerScript : MonoBehaviour
{
    private IsometricCharacterRenderer isoRenderer;

    private void Awake()
    {
        isoRenderer = GetComponentInParent<IsometricCharacterRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "level-2")
            isoRenderer.ChangeSpriteOrder(2);
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "level-2")
            isoRenderer.ChangeSpriteOrder(1);
    }
}
