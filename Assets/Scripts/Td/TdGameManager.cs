using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TdGameManager : MonoBehaviour {

    public static TdGameManager Instance;
    public GameObject endUI;
    public Text endMessage;
    public Text waveMessage;
    public Text hpMessage;
    public int hp = 5;
    private int nowHp;
    private TdEnemySpawner enemySpawner;

    void Awake()
    {
        Instance = this;
        enemySpawner = GetComponent<TdEnemySpawner>();
        nowHp = hp;
        hpMessage.text = "剩余机会：" + nowHp + "/" + hp;
    }

    void Win()
    {
        enemySpawner.Stop();
        endUI.SetActive(true);
        endMessage.text = "<color=#FEFF4AFF>你真棒！</color>";
    }
    void Lose()
    {
        enemySpawner.Stop();
        endUI.SetActive(true);
        endMessage.text = "<color=#FF4A55FF>你真不棒！</color>";
    }

    public void TakeDamage(int damage = 1)
    {
        nowHp -= damage;
        if (nowHp > 0) hpMessage.text = "剩余机会：" + nowHp + "/" + hp;
        else Lose();
    }
    public void WaveCount(int wave,int totalWave = 4)
    {
        if (wave > totalWave) Win();
        else waveMessage.text = "当前波次：" + wave + "/" + totalWave;
    }

    public void OnButtonPlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnButtonBackMenu()
    {
        SceneManager.LoadScene(0);
    }

}
