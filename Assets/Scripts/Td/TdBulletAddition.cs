using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdBulletAddition : MonoBehaviour {

    public List<GameObject> enemys = new List<GameObject>();
    private bool isOn = true;

    void OnTriggerEnter(Collider col)
    {
        if (!isOn) return;
        if (col.tag == "Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }

    //溅射伤害
    public void Sputtering(int damage,float sputterValue)
    {
        damage = (int)(damage * sputterValue);
        StartCoroutine(SputteringWaitCollision(damage));
    }
    IEnumerator SputteringWaitCollision(int damage)
    {
        yield return new WaitForSeconds(0.05f);
        if (enemys.Count > 0)
        {
            foreach (GameObject enemy in enemys)
            {
                if (enemy != null) enemy.transform.GetComponent<TdEnemy>().TakeDamage(damage);
            }
        }
        Destroy(gameObject, 0.5f);
    }

}
