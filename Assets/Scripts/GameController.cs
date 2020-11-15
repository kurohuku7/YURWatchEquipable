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

    private GameObject watchSocket = null;
    private GameObject autoloadedWatch = null;

    private IEnumerator Start()
    {
        yield return WaitWatchInitialize();
        (watchSocket, autoloadedWatch) = GetWatchObjects();

        // Switch to default watch object (watch socket) to change watch type.
        autoloadedWatch.SetActive(false);
        watchSocket.SetActive(true);

        // StartCoroutine(ChangeLoop());
    }

    private IEnumerator ChangeLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            ChangeWatch(commonWatch);

            yield return new WaitForSeconds(3);
            ChangeWatch(uncommonWatch);

            yield return new WaitForSeconds(3);
            ChangeWatch(rareWatch);

            yield return new WaitForSeconds(3);
            ChangeWatch(epicWatch);

            yield return new WaitForSeconds(3);
            ChangeWatch(legendaryWatch);
        }
    }

    private IEnumerator WaitWatchInitialize()
    {
        // TODO: detect initialize event.
        yield return new WaitForSeconds(5);
        watch.ToggleWatch(true);
        yield return new WaitForSeconds(1);
    }

    private (GameObject, GameObject) GetWatchObjects()
    {
        var watchComps = watch.GetComponentsInChildren<WatchReferenceContainer>(true);
        if (watchComps.Length < 2)
        {
            Debug.LogException(new Exception("Default and autoloaded watches are not created!"));
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
        var currentMesh = watchSocket.transform.Find("Mesh").gameObject;
        var newMesh = toWatch.transform.Find("Mesh").gameObject;
        Destroy(currentMesh);
        Instantiate(newMesh, currentMesh.transform.parent);

        var goOnlineEvent = toWatch.GetComponent<WatchReferenceContainer>().GoOnlineEvent;
        var goOfflineEvent = toWatch.GetComponent<WatchReferenceContainer>().GoOfflineEvent;
        var watchReferenceContainer = watchSocket.GetComponent<WatchReferenceContainer>();
        watchReferenceContainer.GoOnlineEvent = goOnlineEvent;
        watchReferenceContainer.GoOnlineEvent = goOnlineEvent;

    }
}
