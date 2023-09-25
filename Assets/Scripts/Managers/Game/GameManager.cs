using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region public Variables
    public static GameManager Instance;
    public enum GameState
    {
        Title,
        PickSkill,
        PreRound,
        Battle,
        PostRound,
        GameOver
    }
    public GameState State { get; private set; }

    public int CurrentRound { get; private set; } = 1;

    #endregion


    #region private Variables
    [SerializeField] private int totalRounds;

    #endregion


    #region public Method
    public void ChangeState(GameState _state)
    {
        State = _state;
        switch (_state)
        {
            case GameState.Title:
                OnTitle();
                break;
            case GameState.PickSkill:
                OnPickSkill();
                break;
            case GameState.PreRound:
                OnPreRound();
                break;
            case GameState.Battle:
                OnBattle();
                break;
            case GameState.PostRound:
                OnPostRound();
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
        }
    }

    #endregion


    #region private Method
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
        ChangeState(GameState.Title);
    }

    private void OnTitle()
    {
        // something must do at title

    }

    private void OnPickSkill()
    {
        // if round1, player1 is first

        // else, last round's winner is first
    }

	
    private void OnPreRound()
    {
        // reset something

        // do something

        // ui refresh??

        ChangeState(GameState.Battle);
    }

    private void OnBattle()
    {
        // change something

        // battle start

    }

    private void OnPostRound()
    {
        // subtract lose player's HP

        // check any player's HP is lower than zero <= maybe do it in property

        // save before round's winner

        // reset something
    }
    
    private void OnGameOver()
    {
        // winner ui??
        // idea : how about save dealing amount of each skills, for each player skills they have
        
        // go to title, or quit <= maybe uimanager should do
    }
    #endregion
}
