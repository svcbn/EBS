using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SkillSelector
{
	private int _selectedCount = 0;
	
	public SkillSelector(IList<SkillInfo> skills)
	{
		Skills = new ReadOnlyCollection<SkillInfo>(skills);
	}

	public Action<SkillInfo> SkillSelected;

	public SkillSelectorInput Input { get; set; }

	public IReadOnlyList<SkillInfo> Skills { get; private set; }

	public bool CanSelect => _selectedCount < Skills.Count;

	public void SelectSkill(SkillInfo skill)
	{
		_selectedCount++;
		SkillSelected?.Invoke(skill);
	}

	public void SelectSkill(int index)
	{
		SelectSkill(Skills[index]);
	}
}
