using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Some plant", menuName = "Creatures/Plant", order = 52)]
public class CreatureTypePlant : CreaturesBase
{
    public List<Sprite> lootSprites = new List<Sprite>();
    public List<int> lootAmount = new List<int>();
}
