
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public float timevalue = 90;
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text timer;
    public TMP_Text endScreen;
    public TMP_Text countdownText;
    public float countdownDuration = 5f;

    [HideInInspector]public int score;
    [SerializeField] private Transform endPanel;
    [SerializeField] private Transform startPanel;

    public bool isGameStart;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


    }
    private void Start()
    {
        endPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        //countdownDuration -= Time.deltaTime;
        if(CountDownTimer()) 
        {
            isGameStart = true;
            startPanel.gameObject.SetActive(false);
            countdownDuration = 120f;
        }
        if (!isGameStart) return;

        if (timevalue > 0)
        {
            timevalue -= Time.deltaTime;

        }

        DisplayTime(timevalue);
        scoreText.text = "Ball Collect: " + score.ToString();
        
    }
    bool CountDownTimer() 
    {
        countdownDuration -= Time.deltaTime;
        if(countdownDuration < 1) {
            countdownText.text = "GO ";
        }
        else
        {
            countdownText.text = Mathf.RoundToInt(countdownDuration).ToString();
        }
        
        return countdownDuration < 0;
    }
    void DisplayTime(float timeTodisplay)
    {
        if (timeTodisplay < 0)
        {
            timeTodisplay = 0;
        }
        float minute = Mathf.FloorToInt(timeTodisplay / 60);
        float seconds = Mathf.FloorToInt(timeTodisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minute, seconds);
        if (timevalue < 10)
        {

            timerText.text = string.Format( "{0:00}:{1:00}", minute, seconds);
            timerText.color = Color.red;
        }

        if (timevalue <= 0)
        {
            ShowEndPanel(" Your Score " + score);
            timerText.text = "";
        }

    }
 
    public void ShowEndPanel(string showString) 
    {
        endPanel.gameObject.SetActive(true);
        isGameStart = false;
        endScreen.text = showString;
    }

    public void RetryScene()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}
