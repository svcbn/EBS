using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillData
{
	public List<SkillInfo> Skills;
}

[Serializable]
public class SkillInfo : ISerializationCallbackReceiver
{
	// Required Data //
	public uint Id;

	public string Name;

	public string Description;

	public string SpriteName;

	// Optional Data (overwrite by serializer) //
	public string SkillType;

	public float CoolDown;

	public Sprite Sprite;

	public ActiveSkillData Data;

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		Sprite = Managers.Resource.Load<Sprite>($"Sprites/{SpriteName}");
		Data = Managers.Resource.Load<ActiveSkillData>($"Data/{SpriteName}Data");
		if (Data != null)
		{
			SkillType = "Active";
			CoolDown = Data.Cooldown;
		}
		else
		{
			SkillType = "Passive";
		}
	}
}