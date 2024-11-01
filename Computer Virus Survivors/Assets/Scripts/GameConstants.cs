using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    // PlayerController : Move
    private const int ThresholdFrame = 3;
    private const int DeadZoneFrame = 3;
    private const float InputUnitDecreasePerFrame = .2f;
    public const float ThresholdSec = ThresholdFrame * InputUnitDecreasePerFrame;
    public const float DeadZoneSec = DeadZoneFrame * InputUnitDecreasePerFrame;

    // PlayerStat : Default
    public const int DefaultMaxHp = 100;
    public const int DefaultCurrentHp = 100;
    public const int DefaultHealthRegenPer10 = 0;
    public const int DefaultDefencePoint = 0;
    public const int DefaultEvadeProbability = 0;
    public const int DefaultInvincibleFrame = 20;

    public const int DefaultAttackPoint = 100;
    public const int DefaultMultiProjectile = 1;
    public const int DefaultAttackSpeed = 100;
    public const int DefaultAttackRange = 100;
    public const int DefaultCritAttackProbability = 0;
    public const int DefaultCritAttackPoint = 150;

    public const int DefaultExpGainRatio = 100;
    public const int DefaultPlayerLevel = 1;
    public const int DefaultCurrentExp = 0;
    public const float DefaultExpGainRange = 0.707f;
    public const float DefaultMoveSpeed = 5f;

    // PlayerStat : Character1(custom)
    // TODO : Character1 Stat

    // PlayerStat : Exp List
    public static readonly List<int> ExpList = new List<int>()
    {
        100, 300, 500, 700, 900,
        1500, 2000, 2500, 3000, 3500,
        4500, 5500, 6500, 7500, 8500,
        10000, 12000, 14000, 16000, 18000,
        20000, 25000, 30000, 35000, 40000,
        50000, 60000, 70000, 80000, 90000,
        100000, 120000, 140000, 160000, 180000,
        200000,
    };

    // CameraController : offset
    public static readonly Vector3 CameraOffset = new Vector3(0, 10, -10);
}
