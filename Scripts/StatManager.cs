﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class StatManager : UdonSharpBehaviour
{
    public PlayerHealthManager playerHealthManager;
    public Image healthBar;
    
    public void Update()
    {
        healthBar.fillAmount = playerHealthManager.CurrentHealth / 100;
    }

    
}
