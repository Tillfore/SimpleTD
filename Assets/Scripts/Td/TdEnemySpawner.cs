using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdEnemySpawner : MonoBehaviour {

    public static int CountEnemyAlive = 0;
    public TdWave[] waves; //记录波次信息
    public Transform START; //起点
    public float waveRate = 5; //每波间隔
    private int totalWaveCount = 0;
    private int waveCount = 1;  //记录波次
    private Coroutine coroutine;

    void Start()
    {
       coroutine = StartCoroutine(SpawnEnemy());
       waveCount = 1;
       foreach (TdWave wave in waves) totalWaveCount++;
       TdGameManager.Instance.WaveCount(waveCount, totalWaveCount);
    }

    public void Stop()
    {
        StopCoroutine(coroutine); //停止刷兵
    }

	IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(10);
        foreach (TdWave wave in waves)
        {
            for(int i = 0; i < wave.count; i++)
            {
                Instantiate(wave.enemyPrefab, START);
                CountEnemyAlive++;
                if (i < wave.count - 1)
                {
                    yield return new WaitForSeconds(wave.rate);
                }
            }
            while (CountEnemyAlive > 0)
            {
                yield return 0;
            }
            waveCount++;
            TdGameManager.Instance.WaveCount(waveCount,totalWaveCount);
            yield return new WaitForSeconds(waveRate);
        }
    }
}