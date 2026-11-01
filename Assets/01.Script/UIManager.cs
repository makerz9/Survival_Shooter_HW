using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI surviveTime;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI txtPlayerHP;
    public Image imgPlayerHP;

    public GameObject gameOverUI;

    private void OnEnable()
    {
        //SetAmmoText(0, 0);
        SetActiveGameOverUI(false);

        txtPlayerHP.gameObject.SetActive(true);
        

    }

    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = $"{magAmmo} / {remainAmmo}";
    }

    public void SetHpText(float curHP, float maxHP)
    {
        txtPlayerHP.text = $"{curHP} / {maxHP}";
        imgPlayerHP.fillAmount = curHP / maxHP;
    }


    public void SetTimeText(int minutes, int seconds)
    {
        surviveTime.text = $"SurvivalTime: {minutes:00}:{seconds:00}";
    }

    public void SetScoreText(int score)
    {
        Debug.Log("ddddd" + score);
        scoreText.text = $"Score: {score}";
    }


    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }
}
