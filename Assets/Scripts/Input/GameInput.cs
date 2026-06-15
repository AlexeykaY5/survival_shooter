using UnityEngine;
using UnityEngine.InputSystem;
public enum InputMode
{
    PC,
    Mobile
}

public class GameInput : MonoBehaviour
{
    [SerializeField] private FloatingJoystick leftJoystick;
    [SerializeField] private FloatingJoystick rightJoystick;
    [SerializeField] private GameObject mobileUICanvas;
    [SerializeField] private float manualThreshold = 0.33f;

    private InputMode currentMode;
    public InputMode CurrentMode => currentMode;

    public static GameInput Instance { get; private set; }

    private InputSystem_Actions action;

    private void Awake()
    {
        Instance = this;
        action = new InputSystem_Actions();
        action.Enable();
        DetectInputMode();
    }

    private void DetectInputMode()
    {
        if (Touchscreen.current != null)
        {
            currentMode = InputMode.Mobile;
            mobileUICanvas.SetActive(true);
        }
        else
        {
            currentMode = InputMode.PC;
            mobileUICanvas.SetActive(false);
        }
    }
        

    public Vector2 GetMovementVector()
    {
        if (currentMode == InputMode.PC)
        {
            return action.Player.Move.ReadValue<Vector2>();
        }
        else if (currentMode == InputMode.Mobile)
        {
            return leftJoystick.Direction;
        }
        else
        {
            return Vector2.zero;
        } 
    }

    public bool IsAttacking()
    {
        if (currentMode == InputMode.PC)
        {
            return Mouse.current.leftButton.isPressed;
        }
        else if (currentMode == InputMode.Mobile) 
        {
            return rightJoystick.IsPressed;
        }
        else
        {
            return false;
        }
    }

    public Vector2 GetAimDirection(Vector3 fromPosition)
    {
        if(currentMode == InputMode.PC)
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            return ((Vector2)mouseWorld - (Vector2)fromPosition).normalized;
        }
        else
        {
            if (rightJoystick.Direction.sqrMagnitude < manualThreshold * manualThreshold)
            {
                return TargetFinder.GetDirectionToNearestEnemy(fromPosition);
            }
            else
            {
                return rightJoystick.Direction;
            }
        }
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
        return mousePos;
    }
}