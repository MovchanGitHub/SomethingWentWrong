using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour, Damagable
{
    [SerializeField] private int hp;
    [SerializeField] private GameObject drop;
    [SerializeField] private float spread = 2f;
    [SerializeField] private float dropSpeed = 5f;

    public void doDamage(int damage)
    {
        hp -= damage;
        DropItem();
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        Vector3 pos = transform.position;
        pos.x += spread * UnityEngine.Random.value - spread / 2;
        pos.y += spread * UnityEngine.Random.value - spread / 2;
        GameObject dropObject = Instantiate(drop);
        dropObject.transform.position = transform.position;
        StartCoroutine(MoveDropRoutine(dropObject.transform, pos, dropSpeed));
    }

    private IEnumerator MoveDropRoutine(Transform transform, Vector3 to, float speed)
    {
        Vector3 from = transform.position;
        float distance = Vector3.Distance(from, to);
        float rate = speed / distance;

        for (float t = 0; t < 1; t += rate * Time.deltaTime)
        {
            transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        transform.position = to;
    }
}
