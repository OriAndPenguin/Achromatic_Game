using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public DialogueManager dialogueManager;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lookDirection = new Vector2(0, -1);
    public bool isTalking = false;

    private GameObject currentTarget;
    private bool isWaitingForChoice = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool isIndoor = true; // 체크(V)하면 실내(좌우), 풀면 실외(상하좌우)

    void Update()
    {
        if (isTalking)
        {
            movement = Vector2.zero;

            if (isWaitingForChoice)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    Destroy(currentTarget);
                    CloseDialogue();
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    CloseDialogue();
                }
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 🔥 [추가] 타이핑이 덜 끝났는데 스페이스바를 누르면? -> 즉시 전체 출력 (스킵)
                if (dialogueManager.IsTyping())
                {
                    dialogueManager.SkipTyping();
                    return; // 아래 코드는 실행 안 함
                }

                // 타이핑이 다 끝난 상태에서 스페이스바를 누르면? -> 평소대로 작동
                if (dialogueManager.IsLastLine())
                {
                    NPCDialogue npc = currentTarget.GetComponent<NPCDialogue>();

                    if (npc != null && npc.isItem)
                    {
                        dialogueManager.ShowChoiceText(); // 🔥 매니저에게 선택지를 띄워달라고 요청!
                        isWaitingForChoice = true;
                    }
                    else
                    {
                        CloseDialogue();
                    }
                }
                else
                {
                    dialogueManager.ShowNextLine();
                }
            }
            return;
        }

        // 평상시 이동
        movement.x = Input.GetAxisRaw("Horizontal");
        if (isIndoor)
        {
            movement.y = 0; // 실내면 위아래 이동 금지!
        }
        else
        {
            movement.y = Input.GetAxisRaw("Vertical"); // 밖이면 다 가능!
        }

        if (movement.magnitude > 0) lookDirection = movement;

        if (movement.x > 0) GetComponent<SpriteRenderer>().flipX = false;
        else if (movement.x < 0) GetComponent<SpriteRenderer>().flipX = true;

        if (Input.GetKeyDown(KeyCode.Space)) Interact();
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
                dialogueManager.ShowDialogue(npc.npcName, npc.dialogues);
                isTalking = true;
                currentTarget = hit.collider.gameObject;
            }
        }
    }

    void CloseDialogue()
    {
        dialogueManager.HideDialogue();
        isTalking = false;
        isWaitingForChoice = false;
        currentTarget = null;
    }
}