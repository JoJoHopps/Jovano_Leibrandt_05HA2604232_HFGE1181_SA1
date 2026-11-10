using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Pickup : MonoBehaviour
{
    private PlayerInputActions InputAction;
    [SerializeField]protected int value;
    protected bool inRange = false;
    protected GameObject  player;

    private void Awake()
    {
        InputAction = new PlayerInputActions();
    }
    private void OnEnable()
    {
        InputAction.Player.Enable();
        InputAction.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        InputAction.Player.Interact.performed -= OnInteract;
        InputAction.Player.Disable();  
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (inRange)
        {
            ApplyEffect(player);
        }
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inRange = true;
            player = col.gameObject;
        }
    }

    protected virtual void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inRange = false;
            player = null;
        }
    }

    protected virtual void ApplyEffect(GameObject player) { }

    protected virtual void DestroyPickup()
    {
        Destroy(gameObject);
    }
}
