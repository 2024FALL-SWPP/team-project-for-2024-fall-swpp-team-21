using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SelectableBehaviour : MonoBehaviour
{
    // level과 levelMax는 SelectableData에 정의되어 있음

    protected GameObject player;

    // WeaponBehaviour에서 Sealed로 override되어 있음
    // 각 무기에서 접근 불가
    abstract protected void LevelUp();

    abstract public void GetSelectable(PlayerController player);

}
