using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartLogic : MonoBehaviour, IInteractable
{
    private int counter;
    private Interact player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Interact>();
            player.SetIInstance(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.ClearIInstance();
        }
    }

    public void InteractHeart()
    {
        player.Heal(1);
        Debug.Log("Heart");
        counter++;
        Destroy(gameObject);
    }

    public void OnInteract()
    {
        if (counter == 0)
        {
            InteractHeart();
        }
    }
}

