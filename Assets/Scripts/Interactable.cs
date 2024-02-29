using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var trigger = gameObject.AddComponent<ObservableTrigger2DTrigger>();

        trigger.OnTriggerStay2DAsObservable()
            .Subscribe(collision =>
            {
                if (collision.gameObject.CompareTag("Player"))
                {

                }
            })
            .AddTo(gameObject);
    }

}
