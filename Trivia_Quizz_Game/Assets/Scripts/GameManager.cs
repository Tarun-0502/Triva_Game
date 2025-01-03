using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Specialized;
using System.IO;
using DG.Tweening;

#region USER_DEFINED_CLASS

[System.Serializable]
public class Input_text
{
    public string Question, optionA, optionB, optionC, optionD;
    public string Answer;
}

#endregion

public class GameManager : MonoBehaviour
{
    #region SINGLETON_REFERENCES

    public static GameManager Instance;
    void Awake()
    {
        Instance = this;
    }

    #endregion

    #region REFRERENCES

    #region HIDE_IN_INSPECTOR

    [SerializeField, HideInInspector] Input_text input_Text = new Input_text();

    [SerializeField, HideInInspector] string Clicked_Answer;

    [SerializeField, HideInInspector] List<int> options = new List<int>();

    [SerializeField] const string fileName = "Assets/Resources/levels.txt";

    #endregion

    [SerializeField] TextMeshProUGUI Quest;

    [SerializeField] Transform Options_Panel;

    [SerializeField] int Level_No;

    [SerializeField] Transform Level_Complete_Screen, Level_Fail_Screen;

    #region Timer Variables

    [SerializeField] private float timeRemaining = 60f; // Time in seconds to count down from
    private bool timerIsRunning = false; // To check if the timer is active
    [SerializeField] private TextMeshProUGUI timerText; // Reference to the UI Text (TextMeshPro)

    #endregion

    #endregion

    #region METHODS

    private void Start()
    {
        Reset_();
        Load(Level_No);
        StartTimer();// Start the timer when the game starts
    }

    #region LOAD_ON_START

    private void Load(int level)
    {
        LoadLevelData(fileName,"Level-"+level);
        Load_Text();
        Button_Click_AddListeners();
    }

    private void LoadLevelData(string filePath, string levelName)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Look for the level name in the file
                    if (line.Trim().Equals(levelName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        // Read the next lines for question and options
                        input_Text.Question = reader.ReadLine()?.Trim();
                        input_Text.optionA = reader.ReadLine()?.Trim();
                        input_Text.optionB = reader.ReadLine()?.Trim();
                        input_Text.optionC = reader.ReadLine()?.Trim();
                        input_Text.optionD = reader.ReadLine()?.Trim();
                        input_Text.Answer = reader.ReadLine()?.Trim();

                        break; // Exit after finding the desired level
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading file: {e.Message}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unexpected error: {e.Message}");
        }
    }

    private void Button_Click_AddListeners()
    {
        Options_Panel.GetChild(0).GetComponent<Button>().onClick.AddListener(()=>Check_Answer(0));
        Options_Panel.GetChild(1).GetComponent<Button>().onClick.AddListener(()=>Check_Answer(1));
        Options_Panel.GetChild(2).GetComponent<Button>().onClick.AddListener(()=>Check_Answer(2));
        Options_Panel.GetChild(3).GetComponent<Button>().onClick.AddListener(()=>Check_Answer(3));
    }

    private void Load_Text()
    {
        Quest.text = input_Text.Question;
        Options_Panel.GetChild(0).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = input_Text.optionA;
        Options_Panel.GetChild(1).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = input_Text.optionB;
        Options_Panel.GetChild(2).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = input_Text.optionC;
        Options_Panel.GetChild(3).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = input_Text.optionD;
    }

    #endregion

    #region POWER_UPS

    public void Chance_50()
    {
        options.Clear();
        for (int i = 0; i < Options_Panel.childCount; i++)
        {
            Clicked_Answer = Options_Panel.GetChild(i).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text;
            if (Clicked_Answer.Contains(input_Text.Answer))
            {
                Options_Panel.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Options_Panel.GetChild(i).gameObject.SetActive(false);
                options.Add(i);
            }
        }
        Options_Panel.GetChild(Random.Range(0,options.Count)).gameObject.SetActive(true);
    }

    public void Hint()
    {
        for(int i = 0; i < Options_Panel.childCount; i++)
        {
            Options_Panel.GetChild(i).GetComponent<Button>().enabled = false;
            Clicked_Answer = Options_Panel.GetChild(i).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text;
            if (Clicked_Answer.Contains(input_Text.Answer))
            {
                Options_Panel.GetChild(i).GetComponent<Image>().color = Color.green;
                Options_Panel.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Options_Panel.GetChild(i).gameObject.SetActive(false);
            }
        }
        Level_Complete();
    }

    public void Change_Question()
    {
        Reset_();
        Load(Level_No + 1);
    }

    #endregion

    #region TIME

    #region Start and Stop Timer

    // Start the timer
    public void StartTimer()
    {
        timerIsRunning = true;
    }

    // Stop the timer
    public void StopTimer()
    {
        timerIsRunning = false;
    }

    #endregion

    #region Update Timer

    void Time_Update()
    {
        // Only update the timer if it's running
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                // Decrease time remaining
                timeRemaining -= Time.deltaTime;

                // Format the remaining time to show minutes:seconds (e.g., 02:30)
                DisplayTime(timeRemaining);
            }
            else
            {
                // Time is up
                timeRemaining = 0;
                StopTimer();

                // Trigger any event when the timer ends (e.g., show message, load next level, etc.)
                OnTimerEnd();
            }
        }
    }

    #endregion

    #region Display Time

    // Display the time in minutes:seconds format
    private void DisplayTime(float timeToDisplay)
    {
        // Convert the time into minutes and seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the UI text
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    #endregion

    #region Timer End Event

    // Event that happens when the timer reaches 0
    private void OnTimerEnd()
    {
        // For example, show a message, stop gameplay, or transition to another screen
        Debug.Log("Time's up!");

        Level_Fail();

        // You can also trigger an event, call a function, or load another scene
    }

    #endregion

    #endregion

    private void Check_Answer(int button)
    {
         Clicked_Answer = Options_Panel.GetChild(button).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text;

        if (Clicked_Answer.Contains(input_Text.Answer))
        {
            Options_Panel.GetChild(button).GetComponent<Image>().color = Color.green;
            for (int i = 0; i < Options_Panel.childCount; i++)
            {
                Options_Panel.GetChild(i).GetComponent<Button>().enabled = false;
                if (i!=button)
                {
                    Options_Panel.GetChild(i).gameObject.SetActive(false);
                }
            }
            Level_Complete();
        }
        else
        {
            Options_Panel.GetChild(button).GetComponent<Image>().color = Color.red;
            for (int i = 0; i < Options_Panel.childCount; i++)
            {
                Options_Panel.GetChild(i).GetComponent<Button>().enabled = false;
                Clicked_Answer = Options_Panel.GetChild(i).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text;
                if (i!=button)
                {
                    if (Clicked_Answer.Contains(input_Text.Answer))
                    {
                        Options_Panel.GetChild(i).GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        Options_Panel.GetChild(i).GetComponent<Image>().color = Color.red;
                        Options_Panel.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            Level_Fail();
        }
    }

    private void Reset_()
    {
       for (int i = 0;i < Options_Panel.childCount;i++)
       {
            Options_Panel.GetChild(i).gameObject.SetActive(true);  
            Options_Panel.GetChild(i).GetComponent<Button>().enabled=true;
       }
        timeRemaining = 25f;
    }

    #region LEVEl_COMPLETE_FAIL

    private void Level_Fail()
    {
        Level_Fail_Screen.transform.localScale = Vector3.zero;
        Level_Fail_Screen.gameObject.SetActive(true);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Level_Fail_Screen.transform.DOScale(Vector3.one, 0.35f);
        });
    }

    private void Level_Complete()
    {
        Level_Complete_Screen.transform.localScale = Vector3.zero;
        Level_Complete_Screen.gameObject.SetActive(true);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Level_Complete_Screen.transform.DOScale(Vector3.one, 0.35f);
        });
    }

    #endregion

    private void Update()
    {
        Time_Update();
    }

    #endregion
}
