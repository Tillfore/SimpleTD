using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{

    public TurretDate laserTurretData;
    public TurretDate missileTurretData;
    public TurretDate standardTurretData;
    public Text moneyText;
    public int money = 500;
    public int bonus = 1;
    public Animator upgradeCanvasAnimator;
    public Animator moneyAnimator;
    public GameObject upgradeUI;
    public Button buttonUpgrade;
    public Toggle toggleNullSelected;
    private  float wages = 0;
    private TdMapCubeData td_MapCube = null;            //表示选中的MapCube实例的组件
    private TurretDate selectedTurretData;  //表示当前选择的炮台类型
    private GameObject selectedTurretGo;    //表示当前选择的炮台实例
    private bool isUpgradeUIOn = false;     //升级UI是否处于显示状态
    private int mousePriority = 0;

    
    void Start()
    {
        upgradeCanvasAnimator = upgradeUI.GetComponent<Animator>();
    }

    void Update()
    {
        wages +=  Time.deltaTime * bonus;
        if (wages > 1) {
            wages--;
            ChangeMoney(bonus);
        }
        if (Input.GetMouseButtonDown(1)) //右键取消控制
        {
            HideUpgradeUI(td_MapCube);
            toggleNullSelected.isOn = true;
        }
        if (Input.GetMouseButtonDown(0)) //确认鼠标点击
        {
            if (EventSystem.current.IsPointerOverGameObject()==false) //确认鼠标不点在UI上
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //创建射线ray为鼠标点
                RaycastHit hit; //投射信息
                /*检测射线是否投射到目标层（ray，投射信息记录，最远投射距离，目标层），
                 * 仅检测名为“MapCube”的层，该层已事先在MapCube（prefab）的Layer中添加.*/
                bool isCollider = Physics.Raycast(ray,out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    TdMapCubeData mapCube = hit.collider.GetComponent<TdMapCubeData>(); 
                    if(selectedTurretData != null && mapCube.turretGo == null && mousePriority >= 1)
                    {
                        //地板空，尝试创建
                        if (money >= selectedTurretData.cost)
                        {
                            mapCube.BuildTurret(selectedTurretData);
                            ChangeMoney(-selectedTurretData.cost);
                        }
                        else
                        {
                            //提示钱不够
                            moneyAnimator.SetTrigger("Tri_money_flicker");
                        }
                    }
                    else if (mapCube.turretGo != null) 
                    {
                        //判断点击的mapCube是否是显示了UI的那个，且UI已显示（即再次点击关闭UI）
                        if (selectedTurretGo == mapCube.turretGo && isUpgradeUIOn)
                        {
                            HideUpgradeUI(mapCube);
                            //StartCoroutine(IEnumerator HideUpgradeUI());
                        }
                        else
                        {
                            //地板不空，且UI未显示，显示升级UI
                            HideUpgradeUI(td_MapCube);
                            td_MapCube = mapCube;
                            ShowUpgradeUI(td_MapCube, !td_MapCube.isUpgraded);
                        }
                        selectedTurretGo = mapCube.turretGo;
                    }

                }
                
            }
        }
    }

    public void ChangeMoney(int change)
    {
        money += change;
        moneyText.text = "￥" + money;
    }

    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
        {
            mousePriority = 1;
            selectedTurretData = laserTurretData;
        }
    }
    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
        {
            mousePriority = 1;
            selectedTurretData = missileTurretData;
        }
    }
    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
        {
            mousePriority = 1;
            selectedTurretData = standardTurretData;
        }
    }
    public void OnNullSelected(bool On)
    {
        if (On)
        {
            mousePriority = 0;
            //selectedTurretData = null;    升级方法参考的数据错误，待解决
        }
    }

    void ShowUpgradeUI(TdMapCubeData mapCube, bool isUpgradeAble = true)
    {
        //StopCoroutine("HideUpgradeUI");
        upgradeUI.transform.position = mapCube.transform.position;
        upgradeUI.SetActive(false);
        upgradeUI.SetActive(true);
        isUpgradeUIOn = true;
        buttonUpgrade.interactable = isUpgradeAble;
        mapCube.OnUpgradeUI(true);
    }
    void HideUpgradeUI(TdMapCubeData mapCube) 
    //IEnumerator HideUpgradeUI(){……} 如果用迭代器配合yield return new WaitForSeconds()可以跟针对性的实现效果
    {
        if (mapCube == null) return;
        upgradeCanvasAnimator.SetTrigger("Hide");
        isUpgradeUIOn = false;
        mapCube.OnUpgradeUI(false);
    }
    public void OnUpgradeButtonDown()  //升级
    {
        HideUpgradeUI(td_MapCube);
        if (money >= selectedTurretData.costUpgraded)
        {
            ChangeMoney(-selectedTurretData.costUpgraded);
            td_MapCube.UpgradeTurret();
        }
        else moneyAnimator.SetTrigger("Tri_money_flicker");
    }
    public void OnDestroyButtonDown()  //回收
    {
        HideUpgradeUI(td_MapCube);
        if (td_MapCube.isUpgraded)
        {
            ChangeMoney((selectedTurretData.cost + selectedTurretData.costUpgraded) / 2);
        }
        else ChangeMoney(selectedTurretData.cost / 2);
        td_MapCube.DestroyTurret();
    }

}
