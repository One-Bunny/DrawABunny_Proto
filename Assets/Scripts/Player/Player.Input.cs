using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


namespace OneBunny
{
public partial class Player
{
    public enum ButtonActions // bool 값으로 입력받는 액션
    {
        Jump
    }

    public enum ValueActions //float 값으로 입력받는 액션
    {
    }


    public UnityAction<Vector2> OnMove { get; set; }

    private Dictionary<ButtonActions, InputAction> buttonActions;
    private Dictionary<ButtonActions, UnityAction<bool>> buttonEvents;

    private Dictionary<ValueActions, InputAction> valueActions;
    private Dictionary<ValueActions, UnityAction<bool>> valueEvents;


    private PlayerInputData inputActions;
    private InputAction moveInputAction;

    private void InitInputs()
    {
        buttonActions = new Dictionary<ButtonActions, InputAction>();
        buttonEvents = new Dictionary<ButtonActions, UnityAction<bool>>();

        valueActions = new Dictionary<ValueActions, InputAction>();
        valueEvents = new Dictionary<ValueActions, UnityAction<bool>>();

        inputActions = new global::PlayerInputData();
        
        moveInputAction = inputActions.Player.Move;
        
        buttonActions.Add(ButtonActions.Jump, inputActions.Player.Jump);
        inputActions.Player.Jump.started += (x) => GetAction(ButtonActions.Jump)?.Invoke(true);
        inputActions.Player.Jump.canceled += (x) => GetAction(ButtonActions.Jump)?.Invoke(false);
    }

    private void UpdateInputs()
    {
        var moveInput = moveInputAction.ReadValue<Vector2>();
        OnMove?.Invoke(moveInput);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


    private UnityAction<bool> GetAction(ButtonActions type)
    {
        if (buttonEvents.TryGetValue(type, out var action))
        {
            return action;
        }
        return null;
    }

    private UnityAction<bool> GetAction(ValueActions type)
    {
        if (valueEvents.TryGetValue(type, out var action))
        {
            return action;
        }
        return null;
    }
    
    public bool GetActionValue(ButtonActions type)
    {
        if (buttonActions.TryGetValue(type, out var input))
        {
            return input.IsPressed();
        }
        return false;
    }

    public float GetActionValue(ValueActions type)
    {
        if (valueActions.TryGetValue(type, out var input))
        {
            return input.ReadValue<float>();
        }

        return 0;
    }
    
    public void SetAction(ButtonActions type, UnityAction<bool> action, bool update = false)
    {
        if (!buttonEvents.ContainsKey(type))
        {
            buttonEvents.Add(type, action);
        }
        else
        {
            buttonEvents[type] = action;
        }

        if (update && buttonActions.TryGetValue(type, out var input))
        {
            action?.Invoke(input.IsPressed());
        }
    }

    public void ClearAction(ButtonActions type)
    {
        if (buttonEvents.ContainsKey(type))
        {
            buttonEvents.Remove(type);
        }
    }
}
}
