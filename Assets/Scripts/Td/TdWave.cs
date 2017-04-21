using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//保存每一波敌人生成所需要的属性
[System.Serializable] //声明TdWave可以被序列化
public class TdWave{
    public GameObject enemyPrefab;  //生成单位
    public int count;   //生成数量
    public float rate;  //生成间隔

}
