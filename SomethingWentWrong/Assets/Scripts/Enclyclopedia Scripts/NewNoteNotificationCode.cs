using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNoteNotificationCode : MonoBehaviour
{
    const float SPACE_FROM_RIGHT_SCREEN_BORDER = 20f;
    const float TIME_TO_APPEAR = 1.5f;
    const float TIME_TO_DISAPPEAR = 1;

    private Vector2 coordinatesInitial;
    private Vector2 coordinatesToStay;

    private void Start()
    {
        coordinatesInitial = transform.localPosition;
        coordinatesToStay = new Vector2 (transform.parent.GetComponent<RectTransform>().rect.xMax - GetComponent<RectTransform>().rect.width / 2 - SPACE_FROM_RIGHT_SCREEN_BORDER, transform.localPosition.y);
        Debug.Log(GetComponent<RectTransform>().rect.width / 2);
        Debug.Log(transform.parent.GetComponent<RectTransform>().rect.xMax);
        Debug.Log(transform.parent.GetComponent<RectTransform>().rect.width);
        Debug.Log(transform.parent.GetComponent<RectTransform>().rect.center);
        //Debug.Log(transform.parent.GetComponent<RectTransform>().rect.xMax);
        //Debug.Log(GetComponent<RectTransform>().rect.xMin);
        //Debug.Log(transform.position.x);
        //Debug.Log(transform.localPosition.y);
        //Debug.Log(transform.position.y);
        Debug.Log(coordinatesToStay);
        Debug.Log(transform.localPosition);
        //Debug.Log(transform.position);
        StartCoroutine(ShowNewNoteNotificationEnum());
    }

    private IEnumerator ShowNewNoteNotificationEnum()
    {

        float timeElapsed = 0;
        Debug.Log("Coroutine started");
        while (Mathf.Abs(transform.localPosition.x - coordinatesToStay.x) > 0.01f)
        {
            transform.localPosition= Vector2.Lerp(coordinatesInitial, coordinatesToStay, timeElapsed);
            timeElapsed += Time.deltaTime / TIME_TO_APPEAR;
            yield return null;
        }
        Debug.Log("Coroutine 1st step ended");
        yield return new WaitForSeconds(4f);
        Debug.Log("Coroutine 2nd step ended");
        timeElapsed = 0;
        while (transform.localPosition.x != coordinatesInitial.x)
        {
            transform.localPosition = Vector2.Lerp(coordinatesToStay, coordinatesInitial, timeElapsed);
            timeElapsed += Time.deltaTime / TIME_TO_DISAPPEAR;
            yield return null;
        }
        Debug.Log("Coroutine ended");
        Destroy(gameObject);
    }
}
