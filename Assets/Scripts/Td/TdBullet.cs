using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdBullet : MonoBehaviour {

    public float speed = 500;
    public float distanceArriveTarget = 1.2f; //弹道引爆距离
    public GameObject explosionEffectPrefab;    //爆炸特效
    public GameObject additionSensorPrefab;     //附加传感器
    private Transform target;
    private int damage;
    private float attackAddition;
    private TdTurretAttack.AttackType attackType;

    public void SetTarget(Transform _target,int getDamage = 10,float attackAddition = 0 ,TdTurretAttack.AttackType attackType= TdTurretAttack.AttackType.TypeStandard)
    {
        target = _target;
        damage = getDamage;
        this.attackAddition = attackAddition;
        this.attackType = attackType;
    }

    void Update () {
        if (target == null)
        {
            DestroyBullet();
            return;
        }
        transform.LookAt(target.position); //面向攻击对象
        transform.Translate(Vector3.forward * speed /10 * Time.deltaTime); //飞向目标
        Vector3 dir = target.position - transform.position;
        if (dir.magnitude <= distanceArriveTarget)
        {
            BulletHit();
            DestroyBullet();
        }
	}

    void BulletHit()
    {
        switch (attackType)
        {
            case (TdTurretAttack.AttackType.TypeStandard):
                target.GetComponent<TdEnemy>().TakeDamage(damage);
                break;
            case (TdTurretAttack.AttackType.TypeLaser):
                break;
            case (TdTurretAttack.AttackType.TypeMissile):
                target.GetComponent<TdEnemy>().TakeDamage(damage*(1-attackAddition));
                Vector3 pos = target.position;
                pos.y = 0.5f;
                GameObject sputter = Instantiate(additionSensorPrefab, pos,target.rotation);
                sputter.GetComponent<TdBulletAddition>().Sputtering(damage,attackAddition);
                break;
        }
    }

    void DestroyBullet()
    {
        GameObject effect = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(effect, 1);
    }
    /*弹道抵达目标(无效）
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<TdEnemy>().TakeDamage(damage);
            Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    */
}
