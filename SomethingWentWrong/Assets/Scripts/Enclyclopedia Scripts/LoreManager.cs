using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    [SerializeField] private string loreText;
    public bool isAvailable;
    [SerializeField] private GameObject lockedIcon;
    [SerializeField] private GameObject unlockedIcon;

    private TMPro.TextMeshProUGUI noteText;
    private GameObject notePanel;

    private void Awake()
    {
        if (isAvailable)
        {
            lockedIcon.SetActive(false);
            unlockedIcon.SetActive(true);
        }
        else
        {
            lockedIcon.SetActive(true);
            unlockedIcon.SetActive(false);
        }
    }

    private void Start()
    {
        noteText = GameManager.GM.UI.Encyclopedia.ExtraInfoLorePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        notePanel = noteText.transform.parent.gameObject;
    }

    public void ShowUpLoreNote()
    {
        if (isAvailable)
        {
            noteText.text = loreText;
            GameManager.GM.UI.Encyclopedia.EncyclopediaScript.coroutineToStop = StartCoroutine(GameManager.GM.UI.Encyclopedia.EncyclopediaScript.AnimateOpeningElement(notePanel));
            //notePanel.SetActive(true);
        }
    }

    public void UnlockNote()
    {
        isAvailable = true;
        lockedIcon.SetActive(false);
        unlockedIcon.SetActive(true);
    }
}
