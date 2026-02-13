using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text nameText;
    public TMP_Text talkText;
    public float textSpeed = 0.05f;

    // 🔥 [추가] 사운드 관련 변수들
    public AudioClip typingSound;    // 인스펙터에서 넣을 소리 파일
    private AudioSource audioSource; // 소리를 내보낼 스피커

    private string[] currentLines;
    private int currentLineIndex;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialogueBox.SetActive(false);

        // 🔥 [추가] 게임 시작 시 스피커(AudioSource) 부품을 자동으로 하나 달아줍니다.
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ShowDialogue(string name, string[] lines)
    {
        dialogueBox.SetActive(true);
        nameText.text = name;
        currentLines = lines;
        currentLineIndex = 0;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(currentLines[currentLineIndex]));
    }

    public void ShowNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < currentLines.Length)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeSentence(currentLines[currentLineIndex]));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        talkText.text = sentence;
        talkText.ForceMeshUpdate();

        int totalCharacters = talkText.textInfo.characterCount;
        talkText.maxVisibleCharacters = 0;

        for (int i = 0; i <= totalCharacters; i++)
        {
            talkText.maxVisibleCharacters = i;

            // 🔥 [추가] 글자가 하나씩 보일 때마다 소리 재생! (소리 파일이 있을 때만)
            if (typingSound != null && i > 0)
            {
                // PlayOneShot: 소리가 겹쳐도 끊기지 않고 자연스럽게 덧대어 재생됩니다.
                // 뒤의 0.5f는 볼륨(50%)입니다. 취향껏 1.0f 등으로 조절하세요.
                audioSource.PlayOneShot(typingSound, 0.5f);
            }

            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    public bool IsLastLine() { return currentLineIndex >= currentLines.Length - 1; }
    public bool IsTyping() { return isTyping; }

    public void SkipTyping()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        talkText.ForceMeshUpdate();
        talkText.maxVisibleCharacters = talkText.textInfo.characterCount;
        isTyping = false;
    }

    public void ShowChoiceText()
    {
        talkText.text += "\n\n<color=yellow>[아이템을 획득하시겠습니까? (Y: 네 / N: 아니요)]</color>";
        talkText.ForceMeshUpdate();
        talkText.maxVisibleCharacters = talkText.textInfo.characterCount;
    }

    public void HideDialogue()
    {
        dialogueBox.SetActive(false);
        isTyping = false;
    }
}