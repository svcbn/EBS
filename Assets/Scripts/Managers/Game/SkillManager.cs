using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager
{
	private SkillData _skillData;

	private Dictionary<Character, List<ISkill>> _skills = new();

	private Dictionary<uint, Type> _skillCache = new();

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

	public SkillInfo GetInfo(uint id)
	{
		return _skillData.Skills.Find(skill => skill.Id == id);
	}

	public bool TryFindSkillTypeById(uint id, out Type type)
	{
		return _skillCache.TryGetValue(id, out type);
	}

	// Test
#if UNITY_EDITOR
	public ISkill GetSkill(uint id, Character owner)
	{
		if (!TryFindSkillTypeById(id, out var skillType))
		{
			Debug.LogError($"Undefined skill type. ID: {id}");
			return null;
		}

		var skill = owner.gameObject.AddComponent(skillType) as ISkill;
		owner.AddSkill(skill);

		return skill;
	}
#endif

	private void GetAllSkills()
	{
		var skillTypes = AppDomain.CurrentDomain.GetAssemblies().
			SelectMany(s => s.GetTypes()).
			Where(type => typeof(ISkill).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

		foreach (var type in skillTypes)
		{
			ActiveSkillData data = Managers.Resource.Load<ActiveSkillData>($"data/{type.Name}Data");
			_skillCache.Add(data.Id, type);
		} 
	}
}