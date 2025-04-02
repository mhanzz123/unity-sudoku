
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button EasyButton;

    public Button MiddleButton;

    public Button HardButton;

    public Button PlayerButton;

    //Jednotlive hodnoty zavisejici na obtiznostech
    public void CliclOnEazy()
    {
        SceneManager.LoadScene("Game");
        GameSettings.EazyMiddleHard_Number = 1;
    }

    public void CliclOnMiddle()
    {
        SceneManager.LoadScene("Game");
        GameSettings.EazyMiddleHard_Number = 2;
    }

    public void CliclOnHard()
    {
        SceneManager.LoadScene("Game");
        GameSettings.EazyMiddleHard_Number = 3;
    }

    public void ClickOnPlayer()
    {

        SceneManager.LoadScene("GameHistory");
    }

}
