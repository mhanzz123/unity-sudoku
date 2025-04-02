using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AuthManager;

public class GameHistoryManager : MonoBehaviour
{
    public Text dateText;
    public Text timeText;
    public Text difficultyText;
    public Text mistakesText;
    public Text solvedText;
    public Button backButton;
    public Button logoutButton;

    void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
        logoutButton.onClick.AddListener(OnLogoutButtonClick);
        DisplayGameStats();
    }

    private void DisplayGameStats()
    {
        string currentUsername = PlayerPrefs.GetString("currentUsername");
        string userOutputJson = PlayerPrefs.GetString("output_" + currentUsername);

        if (!string.IsNullOrEmpty(userOutputJson))
        {
            try
            {
                UserOutput userOutput = JsonUtility.FromJson<UserOutput>(userOutputJson);

                if (userOutput.GameHistory != null)
                {


                    for (int i = userOutput.GameHistory.Count - 1; i >= 0; i--)
                    {
                        var gameStats = userOutput.GameHistory[i];
                        string solvedStatus = gameStats.SolvedCorrectly ? "Ano" : "Ne";
                        TimeSpan timeSpan = TimeSpan.FromSeconds(gameStats.TimeTaken);

                        dateText.text += $"{gameStats.Timestamp}\n";
                        timeText.text += $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}\n";
                        difficultyText.text += $"{gameStats.Difficulty}\n";
                        mistakesText.text += $"{gameStats.Mistakes}\n";
                        solvedText.text += $"{solvedStatus}\n";
                    }
                }
                else
                {
                    dateText.text = "Žádná historie není dostupná";
                    timeText.text = "";
                    difficultyText.text = "";
                    mistakesText.text = "";
                    solvedText.text = "";
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing JSON for user {currentUsername}: {ex.Message}");
                dateText.text = "Error v na?ítání";
                timeText.text = "";
                difficultyText.text = "";
                mistakesText.text = "";
                solvedText.text = "";
            }
        }
        else
        {
            dateText.text = "Žádná historie není dostupná";
            timeText.text = "";
            difficultyText.text = "";
            mistakesText.text = "";
            solvedText.text = "";
        }
    }

    private void OnBackButtonClick()
    {
        SceneManager.LoadScene("Main");
    }

    private void OnLogoutButtonClick()
    {
        PlayerPrefs.SetString("currentUsername", string.Empty);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Login");
    }

    [Serializable]
    public class UserOutput
    {
        public string Username;
        public string Password;
        public string BackupWords;
        public List<GameStats> GameHistory;
    }

    [Serializable]
    public class GameStats
    {
        public string Timestamp;
        public float TimeTaken;
        public int Mistakes;
        public bool SolvedCorrectly;
        public string Difficulty;
    }
}