using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLogic : MonoBehaviour, IInteractable
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

    public void InteractCoin()
    {
        player.Score(1);
        Debug.Log("coin");
        counter++;
        Destroy(gameObject);
    }

    public void OnInteract()
    {
        if (counter == 0)
        {
            InteractCoin();
        }
    }
}