using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNoteNotificationFastEncOpen : MonoBehaviour
{
    public CreaturesBase openedCreature;
    public Coroutine fastCoroutine;

    public void GoToNewCreature() => fastCoroutine = StartCoroutine(GameManager.GM.UI.Encyclopedia.EncyclopediaScript.GoToNewCreatureCoroutine(openedCreature));

    //private void FixedUpdate()
    //{
    //    Debug.Log(fastCoroutine);
    //}
}
