using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUR.SDK.Core.Watch;

public class WatchInteractor : MonoBehaviour
{
    public GameController gameController;

    private void OnCollisionEnter(Collision collision)
    {
        // Watch has WatchReferenceContainer component
        var watch = collision.gameObject.GetComponent<WatchReferenceContainer>();
        if (watch != null)
        {
            gameController.OnWatchEnter(watch.gameObject);
        }
    }
}
