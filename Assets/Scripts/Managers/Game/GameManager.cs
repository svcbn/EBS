using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
	//public Player Player1 { get; private set; }
	//public Player Player2 { get; private set; }
	public bool IsPlayer1Win { get; set; } = true;

	public int Player1HP
	{
		get
		{
			return _player1HP;
		}
		private set
		{
			_player1HP = value;
			if(value < 0)
			{
				isPlayer1Defeat = true;
				ChangeState(GameState.GameOver);
			}
		}
	}
	public int Player2HP
	{
		get
		{
			return _player2HP;
		}
		private set
		{
			_player2HP = value;
			if(value < 0)
			{
				isPlayer1Defeat = false;
				ChangeState(GameState.GameOver);
			}
		}
	}

	#endregion


	#region private Variables
	private Character _currentPicker;
	[SerializeField]
	private List<int> _pickCountList;
	private int _pickCount;
	private int _pickCountIndex;

	private SkillManager _skill = new();
	private int _skillPickCount = 9;

	[SerializeField] private int totalRounds;
	private Character player1;
	private Character player2;
	[SerializeField] private Transform[] spawnPoints = new Transform[2];
	private int _player1HP;
	private int _player2HP;
	private int[] roundDamage = {0, 0, 4, 8, 12, 20, 30, 30, 30, 30};
	private bool isPlayer1Defeat = false;
	private bool _isPlayer1Pick = false;

	private KeyCode[] _registeredKeys =
	{ KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space,
		KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.Return };

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

		_skill.Init();

        ChangeState(GameState.Title);
		
		PreparePlayer();
    }

	private void Update()
	{
		//InputCheck();
	}

	//private void InputCheck()
	//{
	//	var keys = _registeredKeys.Where(key => Input.GetKeyDown(key));
	//	keys.ToList().ForEach(HandleInput);
	//}

	//private void HandleInput(KeyCode key)
	//{
	//	switch (key)
	//	{
	//		case KeyCode.W:
	//			break;
	//		case KeyCode.A:
	//			break;
	//		case KeyCode.S:
	//			break;
	//		case KeyCode.D:
	//			break;
	//		case KeyCode.Space:
	//			break;

	//		case KeyCode.UpArrow:
	//			break;
	//		case KeyCode.LeftArrow:
	//			break;
	//		case KeyCode.DownArrow:
	//			break;
	//		case KeyCode.RightArrow:
	//			break;
	//		case KeyCode.Return:
	//			break;
	//	}
	//}

	private void PreparePlayer()
	{
		player1 = Managers.Resource.Instantiate("Character/Player 1").GetOrAddComponent<Character>();
		player2 = Managers.Resource.Instantiate("Character/Player 1").GetOrAddComponent<Character>();

		Managers.Resource.Release(player1.gameObject);
		Managers.Resource.Release(player2.gameObject);
	}

	private void OnTitle()
    {
        // something must do at title
		
    }

    private void OnPickSkill()
    {
		_pickCountIndex = 0;
		_pickCount = _pickCountList[_pickCountIndex];
		var skillPool = _skill.GeneratePool(_skillPickCount);
		SkillSelector selector = new(skillPool);
		_selectorUI = Managers.UI.ShowPopupUI<UISkillSelector>();
		selector.SkillSelected += PickSkill;
		_selectorUI.SetSelector(selector);
		// if round1, player1 is first
		Character winner = player1;
		// else, last round's winner is first
		_currentPicker = CurrentRound == 1 ? player1 : winner;
	}

	private UISkillSelector _selectorUI;

	private void PickSkill(SkillInfo skillInfo)
	{
		ISkill skill = _skill.GetSkillById(skillInfo.Id);
		_currentPicker.AddSkill(skill);
		if (--_pickCount <= 0)
		{
			_currentPicker = _isPlayer1Pick ? player2 : player1;
			_isPlayer1Pick = !_isPlayer1Pick;
			
			if (_pickCountIndex < _pickCountList.Count - 1)
			{
				_pickCount = _pickCountList[++_pickCountIndex];
			}
			else
			{
				// pick end
				Managers.UI.ClosePopupUI(_selectorUI);
				ChangeState(GameState.PreRound);
			}
		}
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

		PrepareBattle();
    }

    private void OnPostRound()
    {
		CalculateRoundDamage();

		// reset something

		CurrentRound++;

		ChangeState(GameState.PickSkill);
    }
    
    private void OnGameOver()
    {
        // winner ui??
		if (isPlayer1Defeat)
		{
			// player2 win
		}
		else
		{
			// player1 win
		}
        // idea : how about save dealing amount of each skills, for each player skills they have
        
        // go to title, or quit <= maybe uimanager should do
    }

	private void PrepareBattle()
	{
		player1 = Managers.Resource.Instantiate("Character/Player 1").GetOrAddComponent<Character>();
		player2 = Managers.Resource.Instantiate("Character/Player 1").GetOrAddComponent<Character>();
		player1.transform.position = spawnPoints[0].position;
		player2.transform.position = spawnPoints[1].position;

		// maybe ui guide

		// turn on ai

	}

	private void CalculateRoundDamage()
	{
		if (IsPlayer1Win)
		{
			Player2HP -= roundDamage[CurrentRound]; // + 남은 체력 비례 데미지;
		}
		else
		{
			Player1HP -= roundDamage[CurrentRound]; // + 남은 체력 비례 데미지;
		}
	}
    #endregion
}
