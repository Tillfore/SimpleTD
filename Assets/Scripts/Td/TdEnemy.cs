using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TdEnemy : MonoBehaviour {

    public int hp = 100;
    public float speed = 100;
    public int attack = 1;
    public int bonus = 25;
    private float speedCorrect = 1;
    private Transform[] positions;
    public GameObject dieEffect;
    float currentHp; //计算伤害的实际血量
    //public Slider hpSlider;
    private Slider hpSlider;
    private int totalHp;
    private int index = 0;
	// Use this for initialization
	void Start () {
        currentHp = hp;
        positions = TdWaypoints.positions;
        totalHp = hp;
        hpSlider = GetComponentInChildren<Slider>();//从prefab的子内搜索
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        speedCorrect = 1;
	}
    //收到攻击伤害方法
    public void TakeDamage(float damage,float deceleration = 0)
    {
        if (currentHp <= 0) return;
        currentHp -= damage;
        speedCorrect *= (1-deceleration);
        hpSlider.value = currentHp / totalHp;
        if (hp != (int)currentHp) hp = (int)currentHp;
        if (hp <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GameObject effect = Instantiate(dieEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(effect, 1);
        GameObject gamePlayer=GameObject.Find("GameManager");
        gamePlayer.GetComponent<BuildManager>().ChangeMoney(bonus);
    }
    //移动方法
    private void Move()
    {
        float nowSpeed = speed * speedCorrect / 10;
        if (index > positions.Length - 1)
        {
            ReachDestination();
            return;
        }
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * nowSpeed);
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f) index++;
    }
    //到终点后摧毁
    void ReachDestination()
    {
        Destroy(gameObject);
        TdGameManager.Instance.TakeDamage(attack);
    }
    void OnDestroy()
    {
        TdEnemySpawner.CountEnemyAlive--;
    }


}
