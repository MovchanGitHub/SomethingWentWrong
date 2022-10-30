using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableScript
{
    public interface Damagable
    {
        public void doDamage(int damage);
    }
}
