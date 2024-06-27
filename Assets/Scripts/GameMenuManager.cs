using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager instance;

    public static bool gameIsPaused = false;
    public static bool playerIsDead = false;

    [SerializeField]
    private GameObject _pauseMenuObject;
    [SerializeField]
    private GameObject _deathMenu;
    [SerializeField]
    private Health _playerHealth;

    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _mainMenuButton;
    [SerializeField]
    private Button _exitButton;

    private void Awake()
    {
        instance = this;
        gameIsPaused = false;
        playerIsDead = false;
    }

    private void Start()
    {
        _continueButton.onClick.AddListener(() =>
        {
            gameIsPaused = false;
            UpdateMenuItems();
        });
        _mainMenuButton.onClick.AddListener(MainMenu);
        _exitButton.onClick.AddListener(ExitGame);
        _playerHealth.OnDeath.AddListener(() =>
        {
            playerIsDead = true;
            UpdateMenuItems();
        });
        UpdateMenuItems();
    }

    private void Update()
    {
        if (playerIsDead) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            UpdateMenuItems();
        }
    }

    private void UpdateMenuItems()
    {
        _deathMenu.SetActive(playerIsDead);
        _pauseMenuObject.SetActive(gameIsPaused && !playerIsDead);

        _continueButton.gameObject.SetActive(gameIsPaused);
        _mainMenuButton.gameObject.SetActive(playerIsDead || gameIsPaused);
        _exitButton.gameObject.SetActive(playerIsDead || gameIsPaused);

        Time.timeScale = playerIsDead || gameIsPaused ? 0f : 1f;
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
