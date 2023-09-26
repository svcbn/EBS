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


	public StatManager() 
	{
		LoadData();
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

		SoftResetStats();
	}

	public void GiveDamage(int playerIndex, int baseDamage)
	{
		//최종 대미지 계산
		int finalDamage = baseDamage;

		//대미지 적용
		_currentHps[playerIndex] -= finalDamage;
		_currentHps[playerIndex] = Mathf.Clamp(_currentHps[playerIndex], 0, _finalMaxHps[playerIndex]);
		Debug.Log($"Player {playerIndex} took total {finalDamage} damage.");
		//죽음 체크
		if (_currentHps[playerIndex] <= 0)
		{
			Debug.Log($"Player {playerIndex} died.");
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

	}

	private void LoadData()
	{
		_data = Managers.Resource.Load<StatData>("Data/StatData");
	}



}
