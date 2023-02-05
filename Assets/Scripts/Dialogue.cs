using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // A reference to the TextMeshProUGUI component in the scene
    public TextMeshProUGUI textComponent;

    // A list of sentences that will be displayed one by one
    [TextArea(3, 10)]
    public string[] sentences;

    // A variable to keep track of the current sentence index
    private int index;

    // The duration in seconds that the dialogue text should stay on screen
    public float duration = 5f;

    // A timer used to keep track of the elapsed time
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the text component text to an empty string
        textComponent.text = string.Empty;
        // Start the dialogue
        StartDialogue(); 
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimer();
        CheckInput();
        
        if (index < sentences.Length && timer < duration)
        {
          // Increment the timer by the delta time
          //timer += Time.deltaTime;
          Debug.Log("Timer: " + timer);
        }
    }

    // This function checks the timer and deactivates the game object if the timer has reached the duration
    void CheckTimer()
    {
        if (timer >= duration)
        {
            // Deactivate the game object
            gameObject.SetActive(false);
            Debug.Log("Dialogue deactivated");
        }
    }

    // This function checks the space bar input and calls the NextLine function if the space key is pressed
    void CheckInput()
    {
        // If the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call the NextLine function
            NextLine();
        }
    }

    // This function starts the dialogue by initializing the index and setting the first sentence to the text component
    void StartDialogue()
    {
        index = 0;
        textComponent.text = sentences[index];
        // Reset the timer
        timer = 0f;
    }

    // This function is called when the next line should be displayed
    public void NextLine()
    {
        CheckTimer();
        // If there are still sentences left to be displayed
        if(index < sentences.Length - 1)
        {
            // Increment the index
            index++;
            // Update the text component with the new sentence
            textComponent.text = sentences[index];
            // Reset the timer
            timer = 0f;
        }
        else
        {
            // If there are no more sentences, deactivate the game object if the timer has reached the duration
            gameObject.SetActive(false);
            Debug.Log("Dialogue deactivated");
        }
        
    }
}
