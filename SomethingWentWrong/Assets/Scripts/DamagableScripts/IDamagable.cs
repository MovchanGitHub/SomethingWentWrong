using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    abstract int HP { get; set; }
    abstract int MaxHP { get; set; }
    public void GetDamage(IWeaponable weapon);
}
