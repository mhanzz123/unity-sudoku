using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static AuthManager;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

//objekty pouzivane v unity poli

public class Game : MonoBehaviour
{
    public GameObject MainPanel;

    public GameObject SudokuFieldPanel;

    public GameObject FieldPrefab;

    public GameObject ControlPanel;

    public GameObject ControlPrefab;

    public Button InfoButton;

    public Button BackButton;

    public Text TimerText;

    public GameObject WinPanel;

    public GameObject LossPanel;

    public Text LossMessage;

    // Startuje chod 
    void Start()
    {

        CreatePrefabs();
        CreateControlPrefabs();
        CreateSudokuObject();
        WinPanel.SetActive(false);
        LossPanel.SetActive(false);

    }


    private void SaveGameStats(float timeTaken, int mistakes, bool solvedCorrectly)
    {
        string currentUsername = PlayerPrefs.GetString("currentUsername");
        string userOutputJson = PlayerPrefs.GetString("output_" + currentUsername);

        if (!string.IsNullOrEmpty(userOutputJson))
        {
            try
            {
                UserOutput userOutput = JsonUtility.FromJson<UserOutput>(userOutputJson);

                string difficulty = GetDifficulty();  

                GameStats gameStats = new GameStats
                {
                    Timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                    TimeTaken = timeTaken,
                    Mistakes = mistakes,
                    SolvedCorrectly = solvedCorrectly,
                    Difficulty = difficulty  
                };

                if (userOutput.GameHistory == null)
                {
                    userOutput.GameHistory = new List<GameStats>();
                }

                userOutput.GameHistory.Add(gameStats);
                string updatedUserOutputJson = JsonUtility.ToJson(userOutput);
                PlayerPrefs.SetString("output_" + currentUsername, updatedUserOutputJson);
                PlayerPrefs.Save();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing or saving JSON for user {currentUsername}: {ex.Message}");
            }
        }
    }

    private string GetDifficulty()
    {
        switch (GameSettings.EazyMiddleHard_Number)
        {
            case 1: return "L";
            case 2: return "S";
            case 3: return "T";
            default: return "Neznámá";
        }
    }

    private string GetBackupWords(string username)
    {
        string userOutputJson = PlayerPrefs.GetString("output_" + username);
        if (!string.IsNullOrEmpty(userOutputJson))
        {
            try
            {
                UserOutput userOutput = JsonUtility.FromJson<UserOutput>(userOutputJson);
                return userOutput.BackupWords;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing JSON for user {username}: {ex.Message}");
                return null;
            }
        }
        return null;
    }

    public void ClickOn_Finish() //Co se stane kdyz klikneme na tlacitko dokoncit
    {
        _isTimerRunning = false;
        int mistakes = 0;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                FieldPrefabObject fieldObject = PrefabDictionary[new Tuple<int, int>(row, col)];

                if (fieldObject.IsChangeAble) //Zjisti jestli jsou cisla vyplnena spravne tim ze vyhodnoti jestli je schopen
                                              //zmenit finalni pole cisel
                {
                    if (_finalObject.Values[row, col] == fieldObject.Number)
                    {

                        fieldObject.ChangeColorToGreen();


                    }
                    else
                    {

                        fieldObject.ChangeColorToRed();
                        mistakes++;


                    }


                }


            }

        }

        bool solvedCorrectly = (mistakes == 0);
        SaveGameStats(_elapsedTime, mistakes, solvedCorrectly);


        if (solvedCorrectly)
        {
            LossPanel.SetActive(false);
            WinPanel.SetActive(true);
        }
        else
        {
            LossPanel.SetActive(true);
            WinPanel.SetActive(false);
            LossMessage.text = $"Máš {mistakes} chyb.";
        }


    }





    public void ClickOnBackButton()//Co se stane kdyz klikneme na tlacitko zpatky
    {
        SceneManager.LoadScene("Main");//Nacte scenu Main(obtiznosti)
    }

    private SudokuObject _gameObject;
    private SudokuObject _finalObject;

    private void CreateSudokuObject()//vytvari celek cisel
    {
        SudokuGenerator.CreateSudokuObject(out SudokuObject finalObject, out SudokuObject gameObject);
        _gameObject = gameObject;
        _finalObject = finalObject;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                var currentValue = _gameObject.Values[row, col];
                if (currentValue != 0)
                {
                    FieldPrefabObject fieldObject = PrefabDictionary[new Tuple<int, int>(row, col)];
                    fieldObject.SetNumber(currentValue);
                    fieldObject.IsChangeAble = false;
                    fieldObject.ChangeColor(new Color(0.8f, 0.8f, 0.8f)); 
                }
            }
        }
    }

    private bool InfoButtonActive = false;

    public void ClickOn_infoButton()//co se stane pri kliknuti na 'i'
    {
        Debug.Log($"Click on InfoButton");
        if (InfoButtonActive)
        {
            InfoButtonActive = false;
            InfoButton.GetComponent<Image>().color = new Color(0.44f, 0f, 0, 6f);
        }
        else
        {
            InfoButtonActive = true;
            InfoButton.GetComponent<Image>().color = new Color(0.01f, 0.43f, 0.08f);
        }
    }

    private Dictionary<Tuple<int, int>, FieldPrefabObject> PrefabDictionary =
        new Dictionary<Tuple<int, int>, FieldPrefabObject>();

    private void CreatePrefabs()//vytvari bile policka
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                GameObject instance = GameObject.Instantiate(FieldPrefab, SudokuFieldPanel.transform);
                FieldPrefabObject fieldPrefabObject = new FieldPrefabObject(instance, row, column);
                PrefabDictionary.Add(new Tuple<int, int>(row, column), fieldPrefabObject);
                instance.GetComponent<Button>().onClick.AddListener(() => OnClickFieldPrefab(fieldPrefabObject));
            }
        }
    }

    private void CreateControlPrefabs()//vytvari panel cisel ze kterych muzeme volit na vyplneni pole cisel
    {
        for (int i = 1; i < 10; i++)
        {
            GameObject instance = GameObject.Instantiate(ControlPrefab, ControlPanel.transform);
            instance.GetComponentInChildren<Text>().text = i.ToString();
            ControlPrefabObject controlPrefabObject = new ControlPrefabObject { Number = i };
            instance.GetComponent<Button>().onClick.AddListener(() => ClickOnCtrlPrefab(controlPrefabObject));
        }
    }

    private void ClickOnCtrlPrefab(ControlPrefabObject controlPrefabObject)//Co se stane kdyz klikneme na vyberove cisla
    {
        if (_currentHoweredFieldPrefab != null)
        {
            if (InfoButtonActive)
            {
                _currentHoweredFieldPrefab.SetSmallNumber(controlPrefabObject.Number);
            }
            else
            {
                _currentHoweredFieldPrefab.SetNumber(controlPrefabObject.Number);
                HighlightSameNumbers(controlPrefabObject.Number);
            }
        }
    }


    private void HighlightSameNumbers(int number)
    {
        foreach (var field in PrefabDictionary.Values)
        {
            if (field.Number == number)
            {
                field.ChangeColor(new Color(0.5f, 0.8f, 1f)); 
            }
            else if (field.IsChangeAble)
            {
                field.ChangeColor(Color.white); 
            }
            else
            {
                field.ChangeColor(new Color(0.8f, 0.8f, 0.8f)); 
            }
        }
    }

    private FieldPrefabObject _currentHoweredFieldPrefab;

    public int Score { get; private set; }

    private void OnClickFieldPrefab(FieldPrefabObject fieldPrefabObject) //co se stane kdyz klikneme na policko v hracim poli
    {
        Debug.Log($"Clicked on Prefab Row: {fieldPrefabObject.Row}, Column: {fieldPrefabObject.Column}");
        if (fieldPrefabObject.IsChangeAble)
        {
            if (_currentHoweredFieldPrefab != null)
            {
                _currentHoweredFieldPrefab.UnsetHowerMode();
            }
            _currentHoweredFieldPrefab = fieldPrefabObject;
            fieldPrefabObject.SetHowerMode();
        }


    }
    private bool _isTimerRunning = true;



    private float _elapsedTime = 0f; 
    void Update()
    {
       
        if (_isTimerRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(_elapsedTime);
        TimerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
}