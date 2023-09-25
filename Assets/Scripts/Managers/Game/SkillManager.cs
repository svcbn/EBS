using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager
{
	private readonly Dictionary<uint, ISkill> _allSkills = new();

	private SkillData _skillData;

	private Dictionary<Character, List<ISkill>> _skills = new();

	private static Dictionary<Character, List<ISkill>> _dummySkills = new(); // for test

	private Dictionary<int, ISkill> _skillCache = new();

	public void Init()
	{
		GetAllSkills();
		_skillData = Managers.Data.Load<SkillData>();
	}

	public List<ISkill> GetSkills() => _allSkills.Values.ToList();

	public List<ISkill> GeneratePool(int count)
	{
		List<ISkill> newSkillPool = new();

		while (newSkillPool.Count < count)
		{
			ISkill newSkill = new Slash();

			if (_skills.Any(pool => pool.Value.Contains(newSkill)))
			{
				// 이미 새 스킬을 누가 가지고 있음
				continue;
			}

			newSkillPool.Add(newSkill);
		}

		return newSkillPool;
	}

	public ISkill GetSkillById(int id)
	{
		if (!_skillCache.TryGetValue(id, out var skill))
		{
			skill = _skills.SelectMany(pool => pool.Value).FirstOrDefault(skill => skill.Id == id);
			if (skill == null)
			{
				// TODO: 스킬을 보유하지 않은 경우 처리
			}

			_skillCache[id] = skill;
		}

		return skill;
	}

	private void GetAllSkills()
	{
		var skillTypes = AppDomain.CurrentDomain.GetAssemblies().
			SelectMany(s => s.GetTypes()).
			Where(type => typeof(ISkill).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

		foreach (var type in skillTypes)
		{
			ISkill skill = Activator.CreateInstance(type) as ISkill;
			_allSkills.Add(skill.Id, skill);
		}
	}



	static Character _dummyChar;
	
	public static void SetDummySkills() // for test
	{
		Debug.Log("SetDummySkills");


		_dummyChar = new Character();
		_dummySkills.Add(_dummyChar, new List<ISkill>()
		{
			new DummyFireballSkill(),
			new DummyHealSkill(),
			new DummyStunSkill(),
			new DummyShieldSkill(),
			new DummyDashSkill(),
			new DummyLightningSkill(),
			new DummyIceSkill(),
			new DummyStealthSkill(),
			new DummyBarrierSkill(),
			new DummySpeedBoostSkill()
		});
	}

	public static List<ISkill> GetDummySkills()
	{
		Debug.Log("GetDummySkills");

		return _dummySkills[_dummyChar];
	}


	public static ISkill GetHighPrioritySkill()
	{
		if( _dummySkills == null ) { return null; }

		List<ISkill> skills = _dummySkills[_dummyChar];

		foreach( ISkill skill in skills)
		{
			// find high priority skill
			if (skill.Priority == 1)
			{
				return skill;
			}
		}

		return null;
	}
}
