using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigameController : MonoBehaviour
{
    public Image promptDisplay; // UI Image to display arrow prompts
    public Sprite leftArrow, rightArrow, upArrow, downArrow; // Arrow sprites
    public Image resultDisplay; // UI Image for success/failure
    public Sprite successSprite, failureSprite; // Success/failure images
    public int sequenceLength = 5; // Number of prompts in the sequence

    private List<KeyCode> promptSequence = new List<KeyCode>(); // Holds the sequence of prompts
    private bool isPlayerInputActive = false; // Checks if input phase is active


    public Transform fishingFloat; // Reference to the fishing float
    public float floatAmplitude = 0.1f; // How far the float moves up and down
    public float floatFrequency = 2.0f; // Speed of the up-and-down movement
    private Vector3 originalFloatPosition; // To remember the original position


    void Start()
    {
        originalFloatPosition = fishingFloat.position; // Save initial position
        StartGame();
    }


    // Generates a sequence of random arrow keys
    void GenerateSequence(int length)
    {
        promptSequence.Clear();
        for (int i = 0; i < length; i++)
        {
            KeyCode randomKey = GetRandomKey();
            promptSequence.Add(randomKey);
        }
        Debug.Log("Generated Sequence: " + string.Join(", ", promptSequence));
    }


    // Returns a random KeyCode for the arrow keys
    KeyCode GetRandomKey()
    {
        KeyCode[] keys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
        return keys[Random.Range(0, keys.Length)];
    }

    // Starts the game and shows the sequence of prompts
    void StartGame()
    {
        GenerateSequence(sequenceLength);
        StartCoroutine(ShowSequence());
    }

    // Coroutine to display each prompt one by one
    // Coroutine to display each prompt one by one
    IEnumerator ShowSequence()
    {
        isPlayerInputActive = false; // Disable player input during prompt display
        foreach (var key in promptSequence)
        {
            ShowPrompt(key);
            yield return new WaitForSeconds(1.5f); // Wait before showing the next prompt
            HidePrompt(); // Make the prompt invisible
            yield return new WaitForSeconds(0.5f); // Short pause before the next prompt
        }
        StartCoroutine(StartFloatAndInputTimer()); // Start float vibration and input timer
        isPlayerInputActive = true; // Enable player input after sequence is shown
        StartCoroutine(CheckPlayerInput());
    }



    // Updates the prompt display based on the current key
    void ShowPrompt(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.LeftArrow:
                promptDisplay.sprite = leftArrow;
                break;
            case KeyCode.RightArrow:
                promptDisplay.sprite = rightArrow;
                break;
            case KeyCode.UpArrow:
                promptDisplay.sprite = upArrow;
                break;
            case KeyCode.DownArrow:
                promptDisplay.sprite = downArrow;
                break;
        }
        promptDisplay.color = new Color(1, 1, 1, 1); // Make arrow fully visible
    }

    // Call this method to hide the arrow display
    void HidePrompt()
    {
        promptDisplay.color = new Color(1, 1, 1, 0); // Fully transparent
    }


    // Coroutine to check player input for each prompt in sequence
    IEnumerator CheckPlayerInput()
    {
        for (int i = 0; i < promptSequence.Count; i++)
        {
            bool correctKeyPressed = false;

            Debug.Log("Current expected key: " + promptSequence[i]);

            // Wait until the correct key is pressed or time runs out (handled in StartFloatAndInputTimer)
            while (!correctKeyPressed && isPlayerInputActive)
            {
                if (Input.GetKeyDown(promptSequence[i]))
                {
                    Debug.Log("Correct key pressed: " + promptSequence[i]);
                    correctKeyPressed = true; // Move to the next key
                }
                else if (Input.anyKeyDown)
                {
                    Debug.Log("Incorrect key pressed.");
                    ShowFailure();
                    StopCoroutine("VibrateFloat"); // Stop float vibration
                    isPlayerInputActive = false; // Disable player input
                    yield break; // Exit if an incorrect key is pressed
                }

                yield return null; // Wait for the next frame to check again
            }
        }

        ShowSuccess();
        StopCoroutine("VibrateFloat"); // Stop float vibration
        isPlayerInputActive = false; // Disable player input after success
    }







    // Shows the success image and makes it visible
    void ShowSuccess()
    {
        resultDisplay.sprite = successSprite; // Set to success sprite
        resultDisplay.color = new Color(1, 1, 1, 1); // Fully visible
        //Invoke("HideResult", 2.0f); // Hide after 2 seconds
    }

    // Shows the failure image and makes it visible
    void ShowFailure()
    {
        resultDisplay.sprite = failureSprite; // Set to failure sprite
        resultDisplay.color = new Color(1, 1, 1, 1); // Fully visible
        //Invoke("HideResult", 2.0f); // Hide after 2 seconds
    }

    // Hides the result display by making it fully transparent
    void HideResult()
    {
        resultDisplay.color = new Color(1, 1, 1, 0); // Fully transparent
    }




    // Coroutine to handle float vibration and input timing
    IEnumerator StartFloatAndInputTimer()
    {
        // Random delay before the float starts vibrating
        float randomDelay = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(randomDelay);

        // Start vibrating the float
        StartCoroutine(VibrateFloat());

        // Give the player 5 seconds to complete input after vibration starts
        yield return new WaitForSeconds(5.0f);

        // If the player hasn't finished the input sequence in time, trigger failure
        if (isPlayerInputActive)
        {
            Debug.Log("Time ran out!");
            ShowFailure();
            StopCoroutine("CheckPlayerInput"); // Stop input-checking coroutine if still running
            isPlayerInputActive = false; // Disable player input
        }
    }

    // Coroutine to vibrate the float
    IEnumerator VibrateFloat()
    {
        while (isPlayerInputActive)
        {
            float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            fishingFloat.position = originalFloatPosition + new Vector3(0, yOffset, 0);
            yield return null;
        }
        fishingFloat.position = originalFloatPosition; // Reset position when done
    }

}
