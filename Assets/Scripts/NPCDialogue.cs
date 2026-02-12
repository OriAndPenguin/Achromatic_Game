using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    // 친구의 이름과 대사를 Inspector 창에서 적을 수 있게 만듦
    public string npcName;
    [TextArea] // 글 상자를 크게 만들어주는 마법
    public string dialogue;
}