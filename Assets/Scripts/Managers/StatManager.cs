using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private float[] _invincibleTimers = new float[2];
	private Coroutine _invincibleCR;

	private Character[] _characters = new Character[2];
	public Character[] Characters { get => _characters; }

	public delegate void OnCharacterEvent(int actorIndex);
	public event OnCharacterEvent onBlockDamage;
	public event OnCharacterEvent onTakeDamage;


	public void Init()
	{
		for (int i = 0; i < 2; i++)
		{
			_maxHpModifiers[i] = new List<int>();
			_maxMpModifiers[i] = new List<int>();
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
		for (int i = 0; i < 2; i++)
		{
			_currentHps[i] = _baseMaxHps[i];
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

		CalculateFinalHps();
		SoftResetStats();
	}

	public void GiveDamage(int playerIndex, int baseDamage)
	{
		if (_invincibleTimers[playerIndex] > 0) //무적일 경우
		{
			onBlockDamage?.Invoke(playerIndex);
		}
		else
		{
			//최종 대미지 계산
			int finalDamage = baseDamage;

			//대미지 적용
			_currentHps[playerIndex] -= finalDamage;
			_currentHps[playerIndex] = Mathf.Clamp(_currentHps[playerIndex], 0, _finalMaxHps[playerIndex]);
			if(playerIndex == 0)
			{
				GameManager.Instance.player1RoundHPUI.value = _currentHps[playerIndex];
			}
			else
			{
				GameManager.Instance.player2RoundHPUI.value = _currentHps[playerIndex];
			}
			Debug.Log($"Player {playerIndex} took total {finalDamage} damage.");
			onTakeDamage?.Invoke(playerIndex);
			//죽음 체크
			if (_currentHps[playerIndex] <= 0)
			{
				GameManager.Instance.SetRoundWinner(_characters[(playerIndex + 1) % 2]);
				GameManager.Instance.ChangeState(GameManager.GameState.RoundOver);
				Debug.Log($"Player {playerIndex} died.");
			}
		}
	}

	public void GiveHeal(int playerIndex, int baseAmount)
	{
		_currentHps[playerIndex] += baseAmount;
		_currentHps[playerIndex] = Mathf.Clamp(_currentHps[playerIndex], 0, _finalMaxHps[playerIndex]);
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

	private void LoadData()
	{
		_data = Managers.Resource.Load<StatData>("Data/StatData");
	}

	private void CalculateFinalHps()
	{
		for (int i = 0; i < 2; i++)
		{
			int totalLowModifier = 0;
			for(int j = 0; j < _maxHpModifiers[i].Count; j++)
			{
				totalLowModifier += _maxHpModifiers[i][j];
			}
			_finalMaxHps[i] = _baseMaxHps[i] + totalLowModifier;
		}
	}

}
