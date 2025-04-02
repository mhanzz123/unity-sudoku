
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class AuthManager : MonoBehaviour
{
    public Button endButton;

    [Header("Register UI")]
    public GameObject registerPanel;
    public InputField registerUsername;
    public InputField registerPassword;
    public InputField registerConfirmPassword;
    public Button registerButton;
    public Button switchToLoginButton;
    public Text registerStatusText;
    public Text backupWordsText;

    [Header("Login UI")]
    public GameObject loginPanel;
    public InputField loginUsername;
    public InputField loginPassword;
    public Button loginButton;
    public Button switchToRegisterButton;
    public Button forgotPasswordButton;
    public Text loginStatusText;

    [Header("Reset Password UI")]
    public GameObject resetPasswordPanel;
    public InputField resetBackupWords;
    public Button resetPasswordButton;
    public Button backToLoginButton;
    public Text resetPasswordStatusText;

    private void Start()
    {
        registerButton.onClick.AddListener(RegisterUser);
        switchToLoginButton.onClick.AddListener(SwitchToLogin);

        loginButton.onClick.AddListener(LoginUser);
        switchToRegisterButton.onClick.AddListener(SwitchToRegister);
        forgotPasswordButton.onClick.AddListener(SwitchToResetPassword);

        resetPasswordButton.onClick.AddListener(ResetPassword);
        backToLoginButton.onClick.AddListener(SwitchToLogin);



        ShowLoginPanel();

        registerPassword.characterLimit = 20;
        registerPassword.contentType = InputField.ContentType.Password;
        registerConfirmPassword.characterLimit = 20;
        registerConfirmPassword.contentType = InputField.ContentType.Password;
        loginPassword.characterLimit = 20;
        loginPassword.contentType = InputField.ContentType.Password;

        registerUsername.onValidateInput += ValidateInput;
        registerPassword.onValidateInput += ValidateInput;
        registerConfirmPassword.onValidateInput += ValidateInput;
        loginUsername.onValidateInput += ValidateInput;
        loginPassword.onValidateInput += ValidateInput;
        resetBackupWords.onValidateInput += ValidateInput;
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (char.IsWhiteSpace(addedChar))
        {
            return '\0'; 
        }
        return addedChar;
    }

    private void RegisterUser()
    {
        string username = registerUsername.text;
        string password = registerPassword.text;
        string confirmPassword = registerConfirmPassword.text;

        if (string.IsNullOrEmpty(username))
        {
            registerStatusText.text = "Chyb� u�ivatelsk� jm�no";
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            registerStatusText.text = "Chyb� heslo";
            return;
        }

        if (password.Length < 4)
        {
            registerStatusText.text = "Heslo mus� m�t v�ce ne� 4 znaky";
            return;
        }

        if (password.Length > 20)
        {
            registerStatusText.text = "Heslo nem?�e b�t del�� ne� 20 znak?";
            return;
        }

        if (password != confirmPassword)
        {
            registerStatusText.text = "Hesla se neshoduj�";
            return;
        }

        if (PlayerPrefs.HasKey("input_" + username))
        {
            registerStatusText.text = "Toto jm�no je ji� zabran�";
            return;
        }

        string backupWords = GenerateBackupWords();

        UserOutput newUserOutput = new UserOutput
        {
            Username = username,
            Password = password,
            BackupWords = backupWords,
            GameHistory = new List<GameStats>() 
        };

        string userOutputJson = JsonUtility.ToJson(newUserOutput);

        PlayerPrefs.SetString("output_" + username, userOutputJson);
        PlayerPrefs.SetString("input_" + username, username);
        PlayerPrefs.SetString("output_" + backupWords, userOutputJson);
        PlayerPrefs.Save();

        registerStatusText.text = "Zaregistrov�no";
        backupWordsText.text = $"V� z�lohovac� k�d je: {backupWords}. Ulo�te si jej! Bez tohoto k�du nebude mo�n� obnova profilu!";
    }

    private void LoginUser()
    {
        string username = loginUsername.text;
        string password = loginPassword.text;

        if (PlayerPrefs.HasKey("input_" + username))
        {
            string userOutputJson = PlayerPrefs.GetString("output_" + username);
            if (!string.IsNullOrEmpty(userOutputJson))
            {
                UserOutput userOutput = JsonUtility.FromJson<UserOutput>(userOutputJson);

                if (userOutput.Password == password)
                {
                    loginStatusText.text = "OK";
                    PlayerPrefs.SetString("currentUsername", username);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("Main");
                }
                else
                {
                    loginStatusText.text = "Neplatn� heslo nebo jm�no";
                }
            }
            else
            {
                loginStatusText.text = "Chyba p?i na?�t�n� dat u�ivatele";
            }
        }
        else
        {
            loginStatusText.text = "Neplatn� heslo nebo jm�no";
        }
    }

    private void ResetPassword()
    {
        string enteredBackupWords = resetBackupWords.text;

        if (PlayerPrefs.HasKey("output_" + enteredBackupWords))
        {
            string userOutputJson = PlayerPrefs.GetString("output_" + enteredBackupWords);
            UserOutput userOutput = JsonUtility.FromJson<UserOutput>(userOutputJson);

            resetPasswordStatusText.text = $"Jm�no: {userOutput.Username}, Heslo: {userOutput.Password}";
        }
        else
        {
            resetPasswordStatusText.text = "Neplatn� z�lohovac� k�d";
        }
    }

    private string GenerateBackupWords()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new System.Random();
        return new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private void ShowRegisterPanel()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
    }

    private void ShowLoginPanel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
        resetPasswordPanel.SetActive(false);
    }

    private void ShowResetPasswordPanel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(false);
        resetPasswordPanel.SetActive(true);
    }

    private void SwitchToRegister()
    {
        ShowRegisterPanel();
    }

    private void SwitchToLogin()
    {
        ShowLoginPanel();
    }

    private void SwitchToResetPassword()
    {
        ShowResetPasswordPanel();
    }

    public void ClickOnEndButton()
    {
        Debug.Log("Application Ended");
        Application.Quit();
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