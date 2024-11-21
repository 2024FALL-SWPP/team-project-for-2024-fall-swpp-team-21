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

    // PlayerStat : Character1(custom)
    // TODO : Character1 Stat

    // CameraController : offset
    public static readonly Vector3 CameraOffset = new Vector3(0, 10, -10);
}
