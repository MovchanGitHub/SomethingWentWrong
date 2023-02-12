using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public int HP { get; set; }
    public void GetDamage(IWeaponable weapon);
}
