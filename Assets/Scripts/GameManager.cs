using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool shoppingFinished;
    public Trigger entranceTrigger;
    public Trigger exitTrigger;
    public Trigger aisleTrigger;

    public Transform doorLeft;
    public Transform doorRight;

    bool enteredStore;
    bool doorsOpen;

    bool gameStarted;
    bool runFinished;

	float elapsedTime;
	public TMP_Text timerText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entranceTrigger.onTriggerEnterAction = onDoorEntranceEnter;
        entranceTrigger.onTriggerExitAction = onDoorEntranceExit;
        exitTrigger.onTriggerEnterAction = onDoorExitEnter;
        exitTrigger.onTriggerExitAction = onDoorExitExit;
        aisleTrigger.onTriggerEnterAction = onAisleEnter;
    }

    void Update()
    {
        if (gameStarted && !runFinished)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = $"Time: {elapsedTime:F2}";
        }
    }

    IEnumerator tween(Transform target, float duration, Vector3 from, Vector3 to)
    {
        float t = 0.0f;
        
        while (target.position != to)
        {
            target.position = Vector3.Lerp(from, to, t);
            
            t += Time.deltaTime / duration;
            yield return null;
        }
    }

    void onDoorEntranceEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!enteredStore)
        {
            enteredStore = true;
            doorsOpen = true;
            //dodaj mozda kvadratni tween il tak nes
            StartCoroutine(tween(doorLeft, 0.7f, doorLeft.position, doorLeft.position + doorLeft.right * 1.3f));
            StartCoroutine(tween(doorRight, 0.7f, doorRight.position, doorRight.position - doorLeft.right * 1.3f));
        }
    }
    
    void onDoorExitEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
    }
    
    void onDoorEntranceExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
    }
    
    void onDoorExitExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (doorsOpen)
        {
            doorsOpen = false;
        }
    }

    void onAisleEnter(Collider other)
    {
        if (!gameStarted)
        {
            gameStarted = true;
            doorsOpen = false;
            StartCoroutine(tween(doorLeft, 0.7f, doorLeft.position, doorLeft.position - doorLeft.right * 1.3f));
            StartCoroutine(tween(doorRight, 0.7f, doorRight.position, doorRight.position + doorLeft.right * 1.3f));
        }

        if (shoppingFinished)
        {
            runFinished = true;
            StartCoroutine(tween(doorLeft, 0.7f, doorLeft.position, doorLeft.position + doorLeft.right * 1.3f));
            StartCoroutine(tween(doorRight, 0.7f, doorRight.position, doorRight.position - doorLeft.right * 1.3f));
            timerText.color = Color.green;
        }
    }
}


