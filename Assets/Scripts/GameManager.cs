using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] ragdollGrandmas;
    
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
    
    [Header("VR End Game UI Integration")]
    public EndGameUI endGameUIScript;
    public Transform playerCamera;
    public float uiDistanceFromPlayer = 2.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entranceTrigger.onTriggerEnterAction = onDoorEntranceEnter;
        entranceTrigger.onTriggerExitAction = onDoorEntranceExit;
        exitTrigger.onTriggerEnterAction = onDoorExitEnter;
        exitTrigger.onTriggerExitAction = onDoorExitExit;
        aisleTrigger.onTriggerEnterAction = onAisleEnter;
        
        for (int i = 0; i < ragdollGrandmas.Length; i++ )
        {
            var temp = ragdollGrandmas[i];
            var random = Random.Range(i, ragdollGrandmas.Length);
            ragdollGrandmas[i] = ragdollGrandmas[random];
            ragdollGrandmas[random] = temp;
        }

        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            ragdollGrandmas[i].SetActive(true);
        }
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
        // Smart VR verification check we built earlier
        bool isPlayer = other.CompareTag("Player") || 
                        other.transform.root.CompareTag("Player") || 
                        other.GetComponentInParent<OVRCameraRig>() != null;

        if (!isPlayer) return;

        Debug.Log($"[EXIT STATUS] Player verified! shoppingFinished: {shoppingFinished} | runFinished: {runFinished}");

        if (shoppingFinished && !runFinished)
        {
            runFinished = true;
            timerText.color = Color.green;

            StartCoroutine(tween(doorLeft, 0.7f, doorLeft.position, doorLeft.position + doorLeft.right * 1.3f));
            StartCoroutine(tween(doorRight, 0.7f, doorRight.position, doorRight.position - doorLeft.right * 1.3f));

            Debug.Log("[EXIT SUCCESS] Spawning EndGameUI directly in front of the player!");
            TriggerVREndGameUI();
        }
        else
        {
            Debug.LogWarning($"[EXIT BLOCKED] Is shoppingFinished true? ({shoppingFinished}) | Is runFinished false? ({!runFinished})");
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

        // if (shoppingFinished)
        // {
        //     runFinished = true;
        //     StartCoroutine(tween(doorLeft, 0.7f, doorLeft.position, doorLeft.position + doorLeft.right * 1.3f));
        //     StartCoroutine(tween(doorRight, 0.7f, doorRight.position, doorRight.position - doorLeft.right * 1.3f));
        //     timerText.color = Color.green;
        // }
    }
    
    private void TriggerVREndGameUI()
    {
        if (endGameUIScript == null || playerCamera == null)
        {
            Debug.LogError("[GAME MANAGER ERROR] Missing EndGameUI or PlayerCamera reference links in GameManager script settings!");
            return;
        }

        Vector3 headPos = playerCamera.position;
        Vector3 lookDirection = playerCamera.forward;
        
        lookDirection.y = 0;
        lookDirection.Normalize();

        Vector3 targetUIPosition = headPos + (lookDirection * uiDistanceFromPlayer);
        
        targetUIPosition.y = headPos.y; 

        endGameUIScript.transform.position = targetUIPosition;
        endGameUIScript.transform.rotation = Quaternion.LookRotation(lookDirection);

        endGameUIScript.gameObject.SetActive(true);
        endGameUIScript.OpenEndGameUI(elapsedTime, GameModeManager.SelectedMode); 
    }
    
    public void ResetGameVariablesForNewRun()
    {
        elapsedTime = 0f;
        gameStarted = false;
        runFinished = false;
        enteredStore = false;
        shoppingFinished = false;
        timerText.text = "Time: 0.00";
        timerText.color = Color.white;
        Debug.Log("[GAME MANAGER] Run status parameters reset to factory settings!");
    }
}


