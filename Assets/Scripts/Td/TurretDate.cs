using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretDate {
    public GameObject turretPrefab;
    public int cost;
    public GameObject turretUpgradePrefab;
    public int costUpgraded;
    public TurretType turretType;
}

public enum TurretType
{
    LaserTurret,
    MissileTurret,
    StandardTurret
}

