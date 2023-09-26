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

	public string Description;

	public string SpriteName;

	public Sprite Sprite;

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		Sprite = Managers.Resource.Load<Sprite>($"Sprites/{SpriteName}");
	}
}