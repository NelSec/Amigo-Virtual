﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject flashText;

    public Text play;
    public Text reset;
    public Text instructions;
    public Text quit;
    //public Image exit;

    public Animator transition;

    private int numOfOptions = 4;

    private int selectedOption;

    int levelIndex;

    public readonly int defaultLastLevel = 1; // Set as appropriate
    private static bool loaded = false;

    void Start()
    {
        selectedOption = 2;
        play.color = new Color32(255, 255, 255, 255);
        reset.color = new Color32(0, 0, 0, 255);
        instructions.color = new Color32(0, 0, 0, 255);
        quit.color = new Color32(0, 0, 0, 255);

        InvokeRepeating("flashTheText", 0f, 0.5f);

        if (!loaded)
        {
            loaded = true;
            levelIndex = PlayerPrefs.GetInt("Last_Level", defaultLastLevel);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("Main");

        if (Input.GetKeyDown(KeyCode.RightArrow) /*|| Controller input*/)
        { //Input telling it to go up or down.
            selectedOption += 1;
            if (selectedOption > numOfOptions) //If at end of list go back to top
            {
                selectedOption = 1;
            }

            play.color = new Color32(0, 0, 0, 255);
            reset.color = new Color32(0, 0, 0, 255);
            instructions.color = new Color32(0, 0, 0, 255);
            quit.color = new Color32(0, 0, 0, 255);

            switch (selectedOption) //Set the visual indicator for which option you are on.
            {
                case 1:
                    instructions.color = new Color32(255, 255, 255, 255);
                    break;
                case 2:
                    play.color = new Color32(255, 255, 255, 255);
                    break;
                case 3:
                    reset.color = new Color32(255, 255, 255, 255);
                    break;
                case 4:
                    quit.color = new Color32(255, 255, 255, 255);
                    break;
            }

            FindObjectOfType<AudioManager>().Play("Switch");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) /*|| Controller input*/)
        { //Input telling it to go up or down.
            selectedOption -= 1;
            if (selectedOption < 1) //If at end of list go back to top
            {
                selectedOption = numOfOptions;
            }

            play.color = new Color32(0, 0, 0, 255); //Make sure all others will be black (or do any visual you want to use to indicate this)
            reset.color = new Color32(0, 0, 0, 255);
            instructions.color = new Color32(0, 0, 0, 255);
            quit.color = new Color32(0, 0, 0, 255);
            switch (selectedOption) //Set the visual indicator for which option you are on.
            {
                case 1:
                    instructions.color = new Color32(255, 255, 255, 255);
                    break;
                case 2:
                    play.color = new Color32(255, 255, 255, 255);
                    break;
                case 3:
                    reset.color = new Color32(255, 255, 255, 255);
                    break;
                case 4:
                    quit.color = new Color32(255, 255, 255, 255);
                    break;
            }

            FindObjectOfType<AudioManager>().Play("Switch");
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            Debug.Log("Picked: " + selectedOption); //For testing as the switch statment does nothing right now.

            switch (selectedOption) //Set the visual indicator for which option you are on.
            {
                case 2:
                    Play();
                    FindObjectOfType<AudioManager>().Play("Click");
                    break;
                case 3:
                    PlayerPrefs.DeleteAll();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    FindObjectOfType<AudioManager>().Play("Click");
                    break;
                case 1:
                    SceneManager.LoadScene("Instructions");
                    FindObjectOfType<AudioManager>().Play("Click");
                    break;
                case 4:
                    StartCoroutine(transitionToExit());
                    transition.SetTrigger("TriggerTransition");
                    FindObjectOfType<AudioManager>().Play("Click");
                    break;
            }
        }
    }

    void flashTheText()
    {
        if (flashText.activeInHierarchy)
            flashText.SetActive(false);
        else
            flashText.SetActive(true);
    }

    void Play()
    {
        if (!PlayerPrefs.HasKey("isFirstTime") || PlayerPrefs.GetInt("isFirstTime") != 1)
        {
            SceneManager.LoadScene("Setup");
            // Now set the value of isFirstTime to be false in the PlayerPrefs.
            PlayerPrefs.SetInt("isFirstTime", 1);
            PlayerPrefs.Save();
        }
        else
        {
            StartCoroutine(transitionAfterDelay());
            transition.SetTrigger("TriggerTransition");
            //SceneManager.LoadScene(levelIndex);
        }
    }

    IEnumerator transitionAfterDelay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator transitionToExit()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Exit");
        Application.Quit();
    }
}
