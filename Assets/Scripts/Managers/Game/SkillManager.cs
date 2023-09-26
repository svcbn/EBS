using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager
{
	private SkillData _skillData;

	private Dictionary<Character, List<ISkill>> _skills = new();

	private static Dictionary<Character, List<IActiveSkill>> _dummySkills = new(); // for test

	private Dictionary<int, ISkill> _skillCache = new();

	public void Init()
	{
		GetAllSkills();
		_skillData = Managers.Data.Load<SkillData>();
	}

	public List<SkillInfo> GeneratePool(int count)
	{
		List<SkillInfo> newSkillPool = new();

		int totalCount = _skills.SelectMany(pool => pool.Value).Count();
		while (newSkillPool.Count < count)
		{
			if (newSkillPool.Count >= _skillData.Skills.Count - totalCount)
			{
				break;
			}

			int random = UnityEngine.Random.Range(0, _skillData.Skills.Count);
			SkillInfo newSkill = _skillData.Skills[random];
			if (_skills.Any(pool => pool.Value.Any(skill => skill.Id == newSkill.Id)))
			{
				// 이미 새 스킬을 누가 가지고 있음
				continue;
			}

			if (newSkillPool.Contains(newSkill))
			{
				// 이미 리스트에 넣은 스킬임
				continue;
			}

			newSkillPool.Add(newSkill);
		}

		return newSkillPool;
	}

	public SkillInfo GetInfo(ISkill skill)
	{
		return _skillData.Skills.FirstOrDefault(info => info.Id == skill.Id);
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
		}
	}


	static Character _dummyChar;
	
	public static void SetDummySkills() // for test
	{
		Debug.Log("SetDummySkills");


		_dummyChar = new Character();
		_dummySkills.Add(_dummyChar, new List<IActiveSkill>()
		{
			new DummyFireballSkill(),
			//new DummyHealSkill(),
			//new DummyStunSkill(),
			//new DummyShieldSkill(),
			//new DummyDashSkill(),
			//new DummyLightningSkill(),
			//new DummyIceSkill(),
			//new DummyStealthSkill(),
			//new DummyBarrierSkill(),
			//new DummySpeedBoostSkill()
		});
	}

	public static List<IActiveSkill> GetDummySkills()
	{
		Debug.Log("GetDummySkills");

		return _dummySkills[_dummyChar];
	}


	public static IActiveSkill GetHighPrioritySkill()
	{
		if( _dummySkills == null ) { return null; }

		List<IActiveSkill> skills = _dummySkills[_dummyChar];

		foreach( var skill in skills)
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
