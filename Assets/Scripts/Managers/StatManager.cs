using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StatManager
{
	StatData _data;

	private int[] _currentHps = new int[2];
	private int[] _currentMps = new int[2];
	private int[] _baseMaxHps = new int[2];
	private int[] _baseMaxMps = new int[2];
	private List<int>[] _maxHpModifiers = new List<int>[2];
	private List<int>[] _maxMpModifiers = new List<int>[2];
	private int[] _finalMaxHps = new int[2];
	private int[] _finalMaxMps = new int[2];

	private int roundHpIncrease = 20;


	public List<Modifier>[] damageModifiers = new List<Modifier>[2];

	private float[] _invincibleTimers = new float[2];
	private Coroutine _invincibleCR;

	private Character[] _characters = new Character[2];
	public Character[] Characters { get => _characters; }

	public delegate void OnCharacterEvent(int actorIndex);
	public event OnCharacterEvent onBlockDamage;
	public event OnCharacterEvent onTakeDamage;

	private Coroutine _roundEndCR;

	//Skill Specific
	public bool[] isYinYangCore = new bool[2];


	public void Init()
	{
		for (int i = 0; i < 2; i++)
		{
			_maxHpModifiers[i] = new List<int>();
			_maxMpModifiers[i] = new List<int>();

			damageModifiers[i] = new List<Modifier>();
		}
		LoadData();
		HardResetStats();

		if (_invincibleCR != null) Managers.Instance.StopCoroutine(_invincibleCR);
		_invincibleCR = Managers.Instance.StartCoroutine(CR_TickInvincibleTimers());

		_characters[0] = GameObject.Find("Player 1").GetComponent<Character>();
		_characters[1] = GameObject.Find("Player 2").GetComponent<Character>();
	}

	public void SoftResetStats()
	{
		CalculateFinalHps();

		for (int i = 0; i < 2; i++)
		{
			_currentHps[i] = _finalMaxHps[i] + (GameManager.Instance.CurrentRound - 1) * roundHpIncrease;
			_currentMps[i] = _baseMaxMps[i];
		}
	}

	/// <summary>
	/// Reset all stats as if game has just started.
	/// </summary>
	public void HardResetStats()
	{
		for (int i = 0; i < 2; i++) 
		{
			_maxHpModifiers[i].Clear();
			_maxMpModifiers[i].Clear();

			_baseMaxHps[i] = _data.startingMaxHp;
			_baseMaxMps[i] = _data.startingMaxMp;
		}
	}

	public void GiveDamage(int playerIndex, int baseDamage)
	{
		if (_roundEndCR != null) return; //캐릭터 하나 죽었을시 전부 무적

		if (_invincibleTimers[playerIndex] > 0) //무적일 경우
		{
			onBlockDamage?.Invoke(playerIndex);
		}
		else
		{
			/*------최종 대미지 계산 (적용 순서상 정렬)------*/
			float totalPercentage = 0f;
			//대미지 n%증가 버프 합연산
			foreach (var modifier in damageModifiers[1 - playerIndex])
			{
				totalPercentage += modifier.percentage;
			}
			int finalDamage = Mathf.CeilToInt(baseDamage * ((100f + totalPercentage)/100f));
			//YinYanCore 효과 적용
			if (isYinYangCore[1 - playerIndex])
			{
				finalDamage *= 2;
				isYinYangCore[1 - playerIndex] = false;
				//TODO: YinYangCore 타이머 재시작 함수 호출
				//_characters[1-playerIndex].GetComponentInChildren<YinYangCore>().~~
			}

			/*------최종 대미지 계산 끝-----*/

			//대미지 적용
			GameManager.UI.ShowHealthPopup(_characters[playerIndex], -finalDamage);
			_currentHps[playerIndex] -= finalDamage;
			_currentHps[playerIndex] = Mathf.Clamp(_currentHps[playerIndex], 0, _finalMaxHps[playerIndex]);

			GameManager.Instance.SetHpUI(playerIndex, _currentHps[playerIndex]);
			onTakeDamage?.Invoke(playerIndex);

			//죽음 체크
			if (_currentHps[playerIndex] <= 0)
			{
				if (_roundEndCR == null)
				{
					_roundEndCR = Managers.Instance.StartCoroutine(CR_RoundEnd(playerIndex));
				}
			}
		}
	}

	public void GiveHeal(int playerIndex, int baseAmount)
	{
		GameManager.UI.ShowHealthPopup(_characters[playerIndex], baseAmount);
		_currentHps[playerIndex] += baseAmount;
		_currentHps[playerIndex] = Mathf.Clamp(_currentHps[playerIndex], 0, _finalMaxHps[playerIndex]);

		GameManager.Instance.SetHpUI(playerIndex, _currentHps[playerIndex]);
	}

	public void GiveManaHeal(int playerIndex, int baseAmount)
	{
		_currentMps[playerIndex] += baseAmount;
		_currentMps[playerIndex] = Mathf.Clamp(_currentMps[playerIndex], 0, _finalMaxMps[playerIndex]);
	}

	public void BeInvincible(int playerIndex, float duration)
	{
		if (_invincibleTimers[playerIndex] < duration) 
		{
			_invincibleTimers[playerIndex] = duration;
		}
	}

	IEnumerator CR_TickInvincibleTimers()
	{
		while (true)
		{
			for (int i = 0; i < 2; i++)
			{
				if (_invincibleTimers[i] > 0) _invincibleTimers[i] -= Time.deltaTime;
			}
			yield return null;
		}
	}

	IEnumerator CR_RoundEnd(int playerIndex)
	{
		//죽은 캐릭터 투명하게
		SpriteRenderer[] renderers = _characters[playerIndex].GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer renderer in renderers) renderer.enabled = false;
		//두 캐릭터 모두 모든 행동 멈추기
		foreach (Character character in _characters)
		{
			character.GetComponent<BehaviorTree>().enabled = false;
			character.GetComponent<BehaviorTree>().enabled = false;

			character.GetComponent<CharacterMovement>().PlayerInput = Vector2.zero;
			character.GetComponent<CharacterMovement>().PlayerInput = Vector2.zero;
		}

		float delay = 3f;
		GameManager.Instance.SetRoundWinner(_characters[(playerIndex + 1) % 2]);
		GameManager.Instance.ShowRoundWinnerUI(delay);

		yield return new WaitForSeconds(delay);

		foreach (SpriteRenderer renderer in renderers) renderer.enabled = true;

		GameManager.Instance.ChangeState(GameManager.GameState.RoundOver);

		Managers.Instance.StopCoroutine(_roundEndCR);
		_roundEndCR = null;
	}

	private void LoadData()
	{
		_data = Managers.Resource.Load<StatData>("Data/StatData");
	}

	private void CalculateFinalHps()
	{

		for (int i = 0; i < 2; i++)
		{
			int totalModifier = 0;
			for(int j = 0; j < _maxHpModifiers[i].Count; j++)
			{
				totalModifier += _maxHpModifiers[i][j];
			}
			_finalMaxHps[i] = _baseMaxHps[i] + totalModifier;
		}
	}


	public int GetCurrentHp(int playerIndex)
	{
		return _currentHps[playerIndex];
	}

	public int GetMaxHp(int playerIndex)
	{
		return _finalMaxHps[playerIndex];
	}

	public Modifier GetDamageModifier(int playerIndex, string modifierName) 
	{
		List<Modifier> modifiers = damageModifiers[playerIndex];
		Modifier modifier = modifiers.SingleOrDefault(modifer => modifer.modifierName == modifierName);
		if (modifier == null)
		{
			//modifier 더하기
			modifier = new Modifier()
			{
				modifierName = modifierName,
				percentage = 0
			};
			modifiers.Add(modifier);
		}
		return modifier;
	}

	public void AddMaxHp(int playerIndex, int amount)
	{
		_maxHpModifiers[playerIndex].Add(amount);
	}
}

public class Modifier
{
	public string modifierName;
	public float percentage;
}
