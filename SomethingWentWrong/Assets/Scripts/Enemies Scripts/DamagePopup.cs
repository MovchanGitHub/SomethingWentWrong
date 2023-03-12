using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    [SerializeField] private float disappearTimer;
    private float actualDisapearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();

    }

    public void setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;

        moveVector = new Vector3(Random.Range(0f, 1f), 1) * 15f;
        actualDisapearTimer = disappearTimer;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;

        if (actualDisapearTimer > disappearTimer/2)
        {
            float scaleIncreaseAmmount = 1f;
            transform.localScale += Vector3.one * scaleIncreaseAmmount * Time.deltaTime;
        }
        else 
        {
            float scaleDecreaseAmmount = 1f;
            transform.localScale -= Vector3.one * scaleDecreaseAmmount * Time.deltaTime;
        }

        moveVector -= moveVector * 10f * Time.deltaTime;

        actualDisapearTimer -= Time.deltaTime;
        if (actualDisapearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
