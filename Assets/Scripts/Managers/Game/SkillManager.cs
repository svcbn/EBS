using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager
{
	private Dictionary<Character, List<ISkill>> _skills = new();

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
}
