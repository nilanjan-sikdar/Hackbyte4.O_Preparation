using UnityEngine;
using UnityEngine.InputSystem;

namespace Climbing
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputCharacterController : MonoBehaviour
    {
        [Header("Input Action Names (Must match your Asset exactly)")]
        [Tooltip("Example: Type 'Move1' for Player 1, and 'Move2' for Player 2")]
        public string moveActionName = "Move1";
        public string jumpActionName = "Jump1";
        public string dropActionName = "Drop1";
        public string runActionName = "Run1";

        [HideInInspector] public Vector2 movement;
        [HideInInspector] public bool run;
        [HideInInspector] public bool jump;
        [HideInInspector] public bool drop;

        private PlayerInput playerInput;

        // Cached actions
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction dropAction;
        private InputAction runAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            if (playerInput.actions == null)
            {
                Debug.LogError($"[Input Error] No Input Action Asset assigned to the PlayerInput on {gameObject.name}!");
                return;
            }

            // Look for the specific names you typed in the Inspector
            moveAction = playerInput.actions.FindAction(moveActionName);
            jumpAction = playerInput.actions.FindAction(jumpActionName);
            dropAction = playerInput.actions.FindAction(dropActionName);
            runAction = playerInput.actions.FindAction(runActionName);

            // Print helpful warnings if a name is misspelled in the Inspector
            if (moveAction == null) Debug.LogError($"[Input Error] Cannot find action named '{moveActionName}'.");
            if (jumpAction == null) Debug.LogWarning($"[Input Warning] Cannot find action named '{jumpActionName}'.");
            if (dropAction == null) Debug.LogWarning($"[Input Warning] Cannot find action named '{dropActionName}'.");
            if (runAction == null) Debug.LogWarning($"[Input Warning] Cannot find action named '{runActionName}'.");
        }

        private void Update()
        {
            // Failsafe: Stop reading if movement isn't hooked up properly
            if (moveAction == null) return;

            movement = moveAction.ReadValue<Vector2>();

            // The "?." means "If this action exists, check if it's pressed. If not, return false."
            jump = jumpAction?.IsPressed() ?? false;
            drop = dropAction?.IsPressed() ?? false;
            run = runAction?.IsPressed() ?? false;
        }

        // The Parkour System requires this method to exist
        public void ToggleRun()
        {
            if (movement.magnitude > 0.2f && !run)
                run = true;
            else if (movement.magnitude <= 0.2f)
                run = false;
        }
    }
}