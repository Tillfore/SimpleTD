using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdTurretAttack : MonoBehaviour {

    public int attackDamage = 10;   //攻击力 每次或每秒
    public float attackRateTime = 1; //攻击间隔(秒)
    public float attackAddition = 0; //附加效果 如减速、溅射
    private float timer = 0; //攻击间隔计时
    public AttackType attackType;
    public Transform head;
    public GameObject bulletPrefab; //弹道
    public Transform firePosition; //弹道起点
    public LineRenderer laserRenderer; //镭射弹道
    public GameObject laserEffect;
    private List<GameObject> enemys = new List<GameObject>();
    //利用碰撞捕捉敌人
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }

    /* void Start()
     {
         timer = attackRateTime;
     }*/
    void Update()
    {
        if (enemys.Count > 0 && enemys[0] != null)
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }
        timer += Time.deltaTime;
        if (timer < attackRateTime) return;
        switch (attackType)
        {
            case (AttackType.TypeStandard): Attack(); break;
            case (AttackType.TypeLaser): AttackLaser();break;
            case (AttackType.TypeMissile): AttackMissile();break;
        }
    }

    public enum AttackType
    {
        TypeStandard,
        TypeLaser,
        TypeMissile
    }

    void Attack()
    {
        if (enemys.Count <= 0) return;
        if (enemys[0] == null)
        {
            UpdateEnemys();
        }
        else
        {
            if (enemys.Count <= 0) return;
            timer = 0;
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<TdBullet>().SetTarget(enemys[0].transform,attackDamage,attackType:attackType);
        }
    }
    void AttackLaser()
    {
        if (enemys.Count <= 0)
        {
            laserRenderer.enabled = false;
            laserEffect.SetActive(false);
        }
        else if (enemys[0] == null)
        {
            UpdateEnemys();
        }
        else
        {
            if (enemys.Count <= 0) return;
            laserRenderer.enabled = true;
            laserRenderer.SetPositions(new Vector3[] { firePosition.position, enemys[0].transform.position });
            enemys[0].GetComponent<TdEnemy>().TakeDamage(attackDamage * Time.deltaTime,attackAddition);
            laserEffect.SetActive(true);
            laserEffect.transform.position = enemys[0].transform.position;
            Vector3 pos = transform.position;
            pos.y = enemys[0].transform.position.y;
            laserEffect.transform.LookAt(pos);
        }
    }
    void AttackMissile()
    {
        if (enemys.Count <= 0) return;
        if (enemys[0] == null)
        {
            UpdateEnemys();
        }
        else
        {
            if (enemys.Count <= 0) return;
            timer = 0;
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<TdBullet>().SetTarget(enemys[0].transform, attackDamage,attackAddition,attackType);
        }
    }

    void UpdateEnemys()
    {
        List<int> emptyIndex = new List<int>();
        for(int index = 0; index < enemys.Count; index++) //获取空指针的索引
        {
            if (enemys[index] == null)
            {
                emptyIndex.Add(index);
            }
        }
        for(int i = 0; i < emptyIndex.Count; i++) //根据索引从enemys中删除空指针
        {
            enemys.RemoveAt(emptyIndex[i]-i); //**注意**：因为每依次删除空指针时，索引自动前移，需要-i校正
        }
    }

}
