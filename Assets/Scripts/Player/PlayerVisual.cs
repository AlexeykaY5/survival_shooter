using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_RUNNING = "IsRunning";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (GameInput.Instance == null) return;
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector2 aim = GameInput.Instance.GetAimDirection(transform.position);
        if(aim.x < 0)
        {
            spriteRenderer.flipX = true;
        }else if(aim.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}