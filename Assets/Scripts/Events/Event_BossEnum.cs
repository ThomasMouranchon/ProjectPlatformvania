using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_BossEnum : MonoBehaviour
{
    public BossHandler bossHandler;

    public enum EnumTypeName
    {
        Boss_Position,
        Boss_Health,
        Boss_Animation
    }

    public EnumTypeName enumTypeName;
    [Space(30)]
    public BossHandler.Boss_Position enumBossPosition;
    [Space(10)]
    public BossHandler.Boss_Health enumBossHealth;
    [Space(10)]
    public BossHandler.Boss_Animation enumBossAnimation;

    public void ChangeBossEnum()
    {
        switch (enumTypeName)
        {
            default:
                bossHandler.positionEnum = enumBossPosition;
                break;
            case EnumTypeName.Boss_Health:
                bossHandler.healthEnum = enumBossHealth;
                break;
            case EnumTypeName.Boss_Animation:
                bossHandler.animationEnum = enumBossAnimation;
                break;
        }
    }
}
