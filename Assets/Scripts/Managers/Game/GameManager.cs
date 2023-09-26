using System;
using System.Collections.Generic;
using System.Linq;
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
        RoundOver,
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
			if (value < 0)
			{
				_isPlayer1Defeat = true;
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
				_isPlayer1Defeat = false;
				ChangeState(GameState.GameOver);
			}
		}
	}

	public static SkillManager Skill => Instance._skill;
	#endregion


	#region private Variables
	private Character _winner;
	
	[SerializeField]
	private SkillSelectorInput _player1Input;
	
	[SerializeField]
	private SkillSelectorInput _player2Input;

	private SkillSelector _selector;
	private GameUIManager _ui = new();
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
	private bool _isPlayer1Defeat = false;
	private bool _isPlayer1Pick = false;
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
            case GameState.RoundOver:
                OnRoundOver();
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
        }
    }

	public void SetWinner(Character winner)
	{
		_winner = winner;
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

		PreparePlayer();
        ChangeState(GameState.Title);
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_ui.ShowMenu();
		}
		
		//_ui.UpdateTimer();
	}

	public void StartGame()
	{
		CurrentRound = 1;
		ChangeState(GameState.PickSkill);
	}

	private void PreparePlayer()
	{
		player1 = GameObject.Find("Player 1").GetOrAddComponent<Character>();
		player2 = GameObject.Find("Player 2").GetOrAddComponent<Character>();
		
		_ui.SetSkillPresenter(player1);
		_ui.SetSkillPresenter(player2);

		_ui.ShowSkillList(player1, player2);
	}

	private void OnTitle()
    {
        // something must do at title
        InitPlayerStartingPoint();
		_ui.ShowTitle(StartGame);
    }

    private void OnPickSkill()
    {
	    InitPlayerStartingPoint();
	    
	    // TODO: 라운드별 승자 처리
	    Character winner = _winner;
		// if round1, player1 is first
		// else, last round's winner is first
		_currentPicker = CurrentRound == 1 ? player1 : winner;
		_isPlayer1Pick = winner == player1;

		_pickCountIndex = 0;
		_pickCount = _pickCountList[_pickCountIndex];

		var skillPool = _skill.GeneratePool(_skillPickCount);
		_selector = new(skillPool)
		{
			Input = _isPlayer1Pick ? _player1Input : _player2Input
		};
		_selector.SkillSelected += PickSkill;
		_ui.ShowSkillSelector(_selector);
	}

	private Character GetWinner()
	{
		// TODO: 승자 처리
		return player1;
	}

	private void PickSkill(SkillInfo skillInfo)
	{
		if (!_skill.TryFindSkillTypeById(skillInfo.Id, out var skillType))
		{
			Debug.LogError($"Undefined skill type. ID: {skillInfo.Id}, Name: {skillInfo.Name}");
			return;
		}

		if (_currentPicker.gameObject.AddComponent(skillType) is not ISkill skill)
		{
			Debug.LogError($"Can't add skill to {_currentPicker}. Id: {skillInfo.Id}, Name: {skillInfo.Name}");
			return;
		}
		
		skill.Init();
		_currentPicker.AddSkill(skill);
		if (--_pickCount > 0 && _selector.CanSelect)
		{
			return;
		}

		_currentPicker = _isPlayer1Pick ? player2 : player1;
		_selector.Input = _isPlayer1Pick ? _player2Input : _player1Input;
		_isPlayer1Pick = !_isPlayer1Pick;
			
		if (_pickCountIndex < _pickCountList.Count - 1 && _selector.CanSelect)
		{
			_pickCount = _pickCountList[++_pickCountIndex];
		}
		else
		{
			// pick end
			_ui.HideSkillSelector();
			_selector.SkillSelected -= PickSkill;
			_selector = null;
			ChangeState(GameState.PreRound);
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
		StartBattle();
    }

    private void OnRoundOver()
    {
	    InitPlayerStartingPoint();
		CalculateRoundDamage();

		// reset something

		CurrentRound++;

		ChangeState(GameState.PickSkill);
    }
    
    private void OnGameOver()
    {
        // winner ui??
		if (_isPlayer1Defeat)
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

	private void InitPlayerStartingPoint()
	{
		player1.gameObject.SetActive(true);
		player2.gameObject.SetActive(true);
		
		player1.BehaviorTree.enabled = false;
		player2.BehaviorTree.enabled = false;

		player1.GetComponent<CharacterMovement>().PlayerInput = Vector2.zero;
		player2.GetComponent<CharacterMovement>().PlayerInput = Vector2.zero;
		
		player1.transform.position = spawnPoints[0].position;
		player2.transform.position = spawnPoints[1].position;

		// maybe ui guide

		// turn on ai
		Managers.Stat.SoftResetStats();
	}
	
	private void StartBattle()
	{
		player1.BehaviorTree.enabled = true;
		player2.BehaviorTree.enabled = true;
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

	private void OnValidate()
	{
		if (spawnPoints?.Any() is not true)
		{
			Debug.LogWarning($"{nameof(spawnPoints)} is not assigned.");
		}
	}
	#endregion
}
