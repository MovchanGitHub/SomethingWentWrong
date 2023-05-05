using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    [SerializeField] private string loreText;
    public bool isAvailable;
    [SerializeField] private GameObject lockedIcon;
    [SerializeField] private TMPro.TextMeshProUGUI recordToBeat;

    private TMPro.TextMeshProUGUI noteText;
    private GameObject notePanel;


    private void Start()
    {
        noteText = GameManager.GM.UI.Encyclopedia.ExtraInfoLorePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        notePanel = noteText.transform.parent.gameObject;

        if (GameManager.GM.UI.EndScreenScript.MaxScore >= transform.GetSiblingIndex() * 3)
            isAvailable = true;
        else
            isAvailable = false;

        recordToBeat.text = $"дней прожить: {transform.GetSiblingIndex() * 3}";

        if (isAvailable)
        {
            lockedIcon.SetActive(false);
            recordToBeat.gameObject.SetActive(false);
        }
        else
        {
            lockedIcon.SetActive(true);
            recordToBeat.gameObject.SetActive(true);
        }
    }

    public void ShowUpLoreNote()
    {
        if (isAvailable)
        {
            noteText.text = loreText;
            GameManager.GM.UI.Encyclopedia.EncyclopediaScript.coroutineToStop = StartCoroutine(GameManager.GM.UI.Encyclopedia.EncyclopediaScript.AnimateOpenCloseMultipleElement(new GameObject[] { notePanel }, new GameObject[] {GameManager.GM.UI.Encyclopedia.LoreTab }));
            //notePanel.SetActive(true);
        }
    }

    public void UnlockNote()
    {
        isAvailable = true;
        lockedIcon.SetActive(false);
        recordToBeat.gameObject.SetActive(false);
    }
}
