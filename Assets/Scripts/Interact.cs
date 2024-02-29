using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;

public class Interact : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private IInteractable interactableInstance;
    private Subject<int> healthSubject = new Subject<int>();
    private Subject<int> coinSubject = new Subject<int>();
    private Subject<bool> deathSubject = new Subject<bool>();
    public IObservable<bool> DeathObservable => deathSubject;
    public IObservable<int> HealthObservable => healthSubject;
    public IObservable<int> CoinsObservable => coinSubject;
    public int currentHealth = 3;
    public int currentScore = 0;
    public bool death = false;
    public Color defaultColor;

    public TMP_Text coinText;
    public TMP_Text healthText;
    public GameObject deathText;
    public void TryToInteract(InputAction.CallbackContext value)
    {
        if (interactableInstance != null)
        {
            interactableInstance.OnInteract();

            Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                // Code to execute after the delay
                if (!death)
                {
                    spriteRenderer.color = defaultColor;
                }
            })
            .AddTo(this);
        }
    }

    public void SetIInstance(IInteractable interactable)
    {
        interactableInstance = interactable;
        // Debug.Log(interactableInstance);
    }

    public void ClearIInstance()
    {
        interactableInstance = null;
        // Debug.Log("Foop");
    }

    public void OnInteract()
    {

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        spriteRenderer.color = Color.red;
        if (currentHealth <= 0)
        {
            death = true;
            spriteRenderer.color = Color.gray;
            currentHealth = 0;
            deathSubject.OnNext(death); // Notify observer of the updated death status
        }
        healthSubject.OnNext(currentHealth); // Notify observers of the updated health
    }

    // Method to heal the player
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > 3) // Assuming max health is 3
        {
            currentHealth = 3;
        }
        healthSubject.OnNext(currentHealth); // Notify observers of the updated health
        spriteRenderer.color = Color.green;
    }

    public void Score(int scoreAmount)
    {
        currentScore += scoreAmount;
        coinSubject.OnNext(currentScore); // Notify observers of the updated score
        spriteRenderer.color = Color.yellow;
    }

    // Example usage
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        healthText.text = ": " + currentHealth.ToString();
        coinText.text = ": " + currentScore.ToString();

        // Subscribe to health changes and update health text
        HealthObservable.Subscribe(health =>
        {
            healthText.text = ": " + health.ToString();
        });

        // Subscribe to coin changes and update coin text
        CoinsObservable.Subscribe(coins =>
        {
            coinText.text = ": " + coins.ToString();
        });

        // Subscribe to Death status.
        DeathObservable.Subscribe(death =>
        {
            if (death)
            {
                deathText.SetActive(true);
                rb.bodyType = RigidbodyType2D.Static;
                this.enabled = false;
            }
        });
    }
}
