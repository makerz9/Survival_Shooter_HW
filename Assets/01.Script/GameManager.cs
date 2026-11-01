using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    [SerializeField] private int score=0;

    public float survivalTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager.SetScoreText(0);
    }

    // Update is called once per frame
    void Update()
    {
        survivalTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(survivalTime / 60);
        int seconds = Mathf.FloorToInt(survivalTime % 60);



        uiManager.SetTimeText(minutes, seconds);


    }

    public void ScoreUp(int finalscore)
    {
        score = score + finalscore;
        uiManager.SetScoreText(score);
    }
}
