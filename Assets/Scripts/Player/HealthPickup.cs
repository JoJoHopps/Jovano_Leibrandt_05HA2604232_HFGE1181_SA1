using UnityEngine;

public class HealthPickup : Pickup
{
    protected override void ApplyEffect(GameObject player)
    {
        player.GetComponent<HealthHandler>().HealHandler(value);
        DestroyPickup();
        Debug.Log("Heal");
    }
}
