using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [Header("Background Sprites")]
    public Sprite[] backgrounds;
    public string[] backgroundNames;

    [Header("Portrait Sprites")]
    public Sprite[] portraits;
    public string[] portraitNames;

    [Header("UI Elements")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject choicePanel;
    public GameObject choiceButtonPrefab;

    [Header("Character Visuals")]
    public Image characterPortrait;
    public Image backgroundImage;

    [Header("Settings")]
    public float textSpeed = 0.03f;

    [YarnCommand("background")]
    public void SetBackground(string bgName)
    {
        for (int i = 0; i < backgroundNames.Length; i++)
        {
            if (backgroundNames[i] == bgName)
            {
                backgroundImage.sprite = backgrounds[i];
                return;
            }
        }
        Debug.LogWarning("Background tidak ditemukan: " + bgName);
    }

    [YarnCommand("portrait")]
    public void SetPortrait(string portraitName)
    {
        if (portraitName == "none")
        {
            characterPortrait.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < portraitNames.Length; i++)
        {
            if (portraitNames[i] == portraitName)
            {
                characterPortrait.sprite = portraits[i];
                characterPortrait.gameObject.SetActive(true);
                return;
            }
        }
        Debug.LogWarning("Portrait tidak ditemukan: " + portraitName);
    }

    private Coroutine typingCoroutine;

    // Dipanggil Yarn saat dialog mulai
    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
    }

    // Dipanggil Yarn saat dialog selesai
    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
    }

    // Menampilkan teks dengan efek mengetik
    public void ShowLine(string characterName, string line)
    {
        characterNameText.text = characterName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line));
    }

    private IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // Menampilkan pilihan
    public void ShowChoices(string[] choices, System.Action<int> onChoiceSelected)
    {
        choicePanel.SetActive(true);

        // Bersihkan tombol lama
        foreach (Transform child in choicePanel.transform)
            Destroy(child.gameObject);

        // Buat tombol baru untuk setiap pilihan
        for (int i = 0; i < choices.Length; i++)
        {
            int index = i;
            GameObject btn = Instantiate(choiceButtonPrefab, choicePanel.transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                onChoiceSelected(index);
                choicePanel.SetActive(false);
            });
        }
    }

    // Ganti background
    public void ChangeBackground(Sprite newBackground)
    {
        backgroundImage.sprite = newBackground;
    }

    // Ganti portrait karakter
    public void ChangePortrait(Sprite newPortrait)
    {
        characterPortrait.sprite = newPortrait;
        characterPortrait.gameObject.SetActive(newPortrait != null);
    }
}