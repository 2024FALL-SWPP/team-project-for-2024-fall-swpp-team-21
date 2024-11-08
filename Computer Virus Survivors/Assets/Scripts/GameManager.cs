using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private GameObject player;

    public float gameTime = 0;

    public GameObject Player
    {
        get { return player; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        gameTime += Time.deltaTime;
    }
}
