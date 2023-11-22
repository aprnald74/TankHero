using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class Tower : MonoBehaviour
{
    private float towerHp;
    public float TowerHp { get { return towerHp; } set { towerHp = value;  }}
    public float towerMaxHp;
    public Slider TowerHpSlider;

    public GameObject parent;

    private void Start()
    {
        towerHp = towerMaxHp;
    }

    private void Update()
    {
        TowerHpSlider.value = towerHp / towerMaxHp;

        if (towerHp <= 0) {
            parent.GetComponent<Player>().TurretDie();
        } else {
            parent.GetComponent<Player>().TurretLive();
        }
    }
}
