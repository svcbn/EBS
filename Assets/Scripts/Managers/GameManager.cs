using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region private Variables
    [SerializeField] private int totalRounds;
    [SerializeField] private int currentRound;

    #endregion

    #region public Variables

    #endregion

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }
}
