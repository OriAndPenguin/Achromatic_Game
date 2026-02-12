using UnityEngine;
using TMPro; // 🚨 중요: 이게 있어야 글자를 바꿀 수 있어!

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox; // 대화창 상자 (껐다 켰다 할 거)
    public TMP_Text nameText;      // 이름 글자
    public TMP_Text talkText;      // 내용 글자

    void Start()
    {
        // 게임 시작하면 대화창을 일단 숨긴다.
        dialogueBox.SetActive(false);
    }

    // 대화창을 띄우는 함수 (나중에 이걸 호출할 거야)
    public void ShowDialogue(string name, string talk)
    {
        dialogueBox.SetActive(true); // 창 켜기
        nameText.text = name;        // 이름 넣기
        talkText.text = talk;        // 대사 넣기
    }

    // 대화창을 끄는 함수
    public void HideDialogue()
    {
        dialogueBox.SetActive(false);
    }
}