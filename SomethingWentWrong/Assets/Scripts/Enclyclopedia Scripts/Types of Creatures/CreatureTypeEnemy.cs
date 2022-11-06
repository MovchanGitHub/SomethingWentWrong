using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Some enemy", menuName = "Creatures/Enemy", order = 51)]
public class CreatureTypeEnemy : CreaturesBase
{
    public int healthPoints;
    public int damagePoints;
    public List<Sprite> listOfSpecialAbilities = new List<Sprite>();
}
