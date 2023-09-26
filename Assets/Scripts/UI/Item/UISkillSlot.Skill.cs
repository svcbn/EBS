using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

partial class UISkillSlot
{
	private void CheckCooldown()
	{
		var image = Get<Image>((int)Elements.CooldownIndicator);
		image.fillAmount = _skill.CurrentCooldown / _skill.Cooldown;
	}
}
