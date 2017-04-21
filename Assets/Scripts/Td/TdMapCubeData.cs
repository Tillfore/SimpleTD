using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TdMapCubeData : MonoBehaviour {
    [HideInInspector]           //使下方public变量在引擎界面隐藏
    public GameObject turretGo; //保存当前MapCube身上的炮台
    public GameObject buildEffect; //建造特效
    [HideInInspector]
    public bool isUpgraded = false; //身上的炮台是否升级过
    private TurretDate turretData;
    private new Renderer renderer; //渲染器 控制MapCube颜色
    private int mousePriority = 0; //鼠标事件优先级

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void BuildTurret(TurretDate turretData)
    {
        this.turretData = turretData;
        turretGo = Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        GameObject effect = Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1);
    }

    public void UpgradeTurret()
    {
        if (isUpgraded) return;
        Destroy(turretGo);
        isUpgraded = true;
        turretGo = Instantiate(turretData.turretUpgradePrefab, transform.position, Quaternion.identity);
        GameObject effect = Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1);
    }

    public void DestroyTurret()
    {
        if (turretGo == null) return;
        Destroy(turretGo);
        GameObject effect = Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1);
        isUpgraded = false;
        turretGo = null;
        turretData = null;
    }

    private void OnMouseEnter()
    {
        if(mousePriority <= 0 && turretGo == null && EventSystem.current.IsPointerOverGameObject() == false) //MapCube上无炮台且鼠标未停在UI上
        {
            renderer.material.color = Color.yellow;
        }
    }
    private void OnMouseExit()
    {
        if (mousePriority <= 0)
        {
            renderer.material.color = Color.white;
        }
    }
    public void OnUpgradeUI(bool isOn)
    {
        if (mousePriority > 3) return;
        if (isOn)
        {
            renderer.material.color = Color.blue;
            mousePriority = 3;
        }
        else
        {
            renderer.material.color = Color.white;
            mousePriority = 0;
        }
    }
}
