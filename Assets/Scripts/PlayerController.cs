using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public DialogueManager dialogueManager;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lookDirection = new Vector2(0, -1);

    // 🔥 [추가] 지금 대화 중인지 확인하는 변수
    public bool isTalking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 🔥 [추가] 대화 중이라면? 움직임 입력을 받지 말고, 스페이스바 누르면 창 닫기!
        if (isTalking)
        {
            movement = Vector2.zero; // 멈춤
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CloseDialogue(); // 대화창 닫기 함수 호출
            }
            return; // 아래 코드는 실행 안 함 (움직임 봉인)
        }

        // --- 평소 상태 (움직임 가능) ---
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 0)
        {
            lookDirection = movement;
        }

        // 방향 전환 (오른쪽/왼쪽 보기)
        if (movement.x > 0) GetComponent<SpriteRenderer>().flipX = false;
        else if (movement.x < 0) GetComponent<SpriteRenderer>().flipX = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            NPCDialogue npc = hit.collider.GetComponent<NPCDialogue>();
            if (npc != null)
            {
                dialogueManager.ShowDialogue(npc.npcName, npc.dialogue);
                isTalking = true; // 🔥 [추가] 대화 시작 상태로 변경!
            }
        }
    }

    // 🔥 [추가] 대화창 닫는 함수
    void CloseDialogue()
    {
        dialogueManager.HideDialogue(); // 매니저에게 끄라고 시킴
        isTalking = false; // 다시 움직일 수 있게 상태 변경
    }
}