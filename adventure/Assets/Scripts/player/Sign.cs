using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    [SerializeField] private bool canPress;
    private Iinteractable targetItem;
    private void Awake()
    {
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }
    private void OnEnable()
    {
        playerInput.Gameplay.Confirm.started += OnConfirm;        
    }
    private void OnDisable()
    {
        canPress = false;
    }
    private void Update()
    {
    }
    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if(canPress)
        {
            targetItem.TriggerAction();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<Iinteractable>();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = false;
        }
    }
}
