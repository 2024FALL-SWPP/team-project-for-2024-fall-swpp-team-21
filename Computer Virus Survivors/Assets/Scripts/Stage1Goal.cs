using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Stage1Goal을 Stage1Puzzle에 통합시키는 방법도 있음 */
public class Stage1Goal : Singleton<Stage1Goal>
{
    [SerializeField] private GameObject stage1Puzzle;

    //private bool isBossDead = false;
    public bool hasPiece = false;
    //private bool isGameClear = false;

    public override void Initialize()
    {
        // Maybe nothing to do
    }

    public void OnBossDead()
    {
        //isBossDead = true;
        stage1Puzzle.SetActive(true);
    }

    public void OnPieceGet()
    {
        hasPiece = true;
    }
}