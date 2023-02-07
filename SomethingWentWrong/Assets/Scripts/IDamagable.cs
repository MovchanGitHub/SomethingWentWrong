using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float HP();
    public void GetDamage(float damage);
}
