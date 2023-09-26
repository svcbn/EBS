using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SkillSelector
{
	public SkillSelector(IList<SkillInfo> skills)
	{
		Skills = new ReadOnlyCollection<SkillInfo>(skills);
	}

	public event Action<SkillInfo> SkillSelected;

	public SkillSelectorInput Input { get; set; }

	public IReadOnlyList<SkillInfo> Skills { get; private set; }

	public void SelectSkill(SkillInfo skill)
	{
		SkillSelected?.Invoke(skill);
	}

	public void SelectSkill(int index)
	{
		SelectSkill(Skills[index]);
	}
}
