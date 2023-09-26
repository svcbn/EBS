using System;
using System.Collections;
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
	public uint Id;

	public string Name;

	public string SkillType;

	public float CoolDown;

	public string Description;

	public string SpriteName;

	public Sprite Sprite;

	public ActiveSkillData Data;

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		Sprite = Managers.Resource.Load<Sprite>($"Sprites/{SpriteName}");
		Data = Managers.Resource.Load<ActiveSkillData>($"Data/{Name}Data");
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