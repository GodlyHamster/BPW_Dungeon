using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager instance;

    public static bool gameIsPaused = false;

    [SerializeField]
    private GameObject _pauseMenuObject;

    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _mainMenuButton;
    [SerializeField]
    private Button _exitButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _continueButton.onClick.AddListener(() =>
        {
            gameIsPaused = false;
            UpdatePauseMenu();
        });
        _mainMenuButton.onClick.AddListener(MainMenu);
        _exitButton.onClick.AddListener(ExitGame);
        UpdatePauseMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            UpdatePauseMenu();
        }
    }

    private void UpdatePauseMenu()
    {
        if (gameIsPaused)
        {
            _pauseMenuObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            _pauseMenuObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void MainMenu()
    {
        gameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
