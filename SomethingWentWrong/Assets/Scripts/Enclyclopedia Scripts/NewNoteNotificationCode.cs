using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
        StartCoroutine(ShowNewNoteNotificationEnum());
    }

    private IEnumerator ShowNewNoteNotificationEnum()
    {

        float timeElapsed = 0;
        while (Mathf.Abs(transform.localPosition.x - coordinatesToStay.x) > 0.01f)
        {
            transform.localPosition= Vector2.Lerp(coordinatesInitial, coordinatesToStay, timeElapsed);
            timeElapsed += Time.deltaTime / TIME_TO_APPEAR;
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        timeElapsed = 0;
        while (transform.localPosition.x != coordinatesInitial.x)
        {
            transform.localPosition = Vector2.Lerp(coordinatesToStay, coordinatesInitial, timeElapsed);
            timeElapsed += Time.deltaTime / TIME_TO_DISAPPEAR;
            yield return null;
        }
        Destroy(gameObject);
    }
}
