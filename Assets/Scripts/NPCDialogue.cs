using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public string npcName;
    [TextArea]
    public string[] dialogues; // 여러 줄을 저장하기 위해 배열([])로 변경!
    public bool isItem = false;
}