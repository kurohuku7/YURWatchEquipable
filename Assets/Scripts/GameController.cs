using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUR.SDK.Core.Watch;

public class GameController : MonoBehaviour
{
    public YURWatch watch;
    public GameObject commonWatch;
    public GameObject uncommonWatch;
    public GameObject rareWatch;
    public GameObject epicWatch;
    public GameObject legendaryWatch;
    public GameObject WatchContainer;

    private GameObject watchSocket = null;
    private GameObject autoloadedWatch = null;

    private IEnumerator Start()
    {
        WatchContainer.SetActive(false);

        // This program uses the default watch as changable watch (watchSocket).
        // Toggle to default watch.
        watch.ToggleWatch(true);

        // Wait YUR watch setup and toggle.
        while (watchSocket == null || autoloadedWatch == null)
        {
            yield return new WaitForSeconds(1);
            (watchSocket, autoloadedWatch) = GetWatchObjects();
        }

        // Switch to default watch object (watch socket) to change watch type.
        autoloadedWatch.SetActive(false);
        watchSocket.SetActive(true);

        // First watch is common.
        WatchContainer.SetActive(true);
        ChangeWatch(commonWatch);
    }

    private bool IsWatchInitialized()
    {
        var watchComps = watch.GetComponentsInChildren<WatchReferenceContainer>(true);
        if (watchComps.Length < 2)
        {
            return false;
        }
        return true;
    }

    private (GameObject, GameObject) GetWatchObjects()
    {
        // TODO: dupricated code avobe.
        var watchComps = watch.GetComponentsInChildren<WatchReferenceContainer>(true);
        if (watchComps.Length < 2)
        {
            // Default and autoloaded watches are not created.
            return (null, null);
        }
        foreach (var watchComp in watchComps)
        {
            if (watchComp.gameObject.activeSelf)
            {
                autoloadedWatch = watchComp.gameObject;
            }
            else
            {
                watchSocket = watchComp.gameObject;
            }
        }

        return (watchSocket, autoloadedWatch);
    }

    private void ChangeWatch(GameObject toWatch)
    {
        // Note: Equipped watch's position is set in Mesh game object under each watch.
        // Note: commonMesh prefab has no Mesh object so unpacked it in the editor and add Mesh object manually.
        var currentMesh = watchSocket.transform.Find("Mesh");
        if (currentMesh == null)
        {
            // TODO: fix search mesh by without name
            currentMesh = watchSocket.transform.Find("Mesh(Clone)");
        }
        var newMesh = toWatch.transform.Find("Mesh");
        Instantiate(newMesh.gameObject, currentMesh.parent);
        Destroy(currentMesh.gameObject);
        
        // TODO: Copy particle system of the legendary watch.

        var goOnlineEvent = toWatch.GetComponent<WatchReferenceContainer>().GoOnlineEvent;
        var goOfflineEvent = toWatch.GetComponent<WatchReferenceContainer>().GoOfflineEvent;
        var watchReferenceContainer = watchSocket.GetComponent<WatchReferenceContainer>();
        watchReferenceContainer.GoOnlineEvent = goOnlineEvent;
        watchReferenceContainer.GoOnlineEvent = goOnlineEvent;
    }
    
    public void OnWatchEnter(GameObject watch)
    {
        ChangeWatch(watch);
    }
}
