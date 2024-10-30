using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SelectableBehaviour : MonoBehaviour
{
    protected int level;
    protected int levelMax;
    protected GameObject player;

    abstract protected void LevelUp();
}
