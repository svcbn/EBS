using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class DummySkills
{
    
}

public class DummyFireballSkill : SkillBase, IActiveSkill
{

	public DummyFireballSkill()
    {
        Id                = 1;
        Type              = SkillType.Attack;
        Priority          = 1;
        IsRestricteMoving = false;
        Cooldown          = 5f;
        BeforeDelay       = 0.5f;
        Duration          = 2f;
        AfterDelay        = 0.5f;
		RequireMP		  = 0f;
	}

	public override void Execute()
	{ 
		base.Execute();

		Debug.Log($"DummyFireballSkill is used");
	}
}
public class DummyFireballSkill1 : SkillBase
{

	public DummyFireballSkill1()
    {
        Id                = 1;
        Type              = SkillType.Attack;
        Priority          = 1;
        IsRestricteMoving = false;
        Cooldown          = 4f;
        BeforeDelay       = 0.5f;
        Duration          = 0.5f;
        AfterDelay        = 0.5f;
		RequireMP = 0f;
	}

	public override void Execute()
	{ 
		base.Execute();

		Debug.Log($"DummyFireballSkill is used");
	}
}
public class DummyFireballSkill2 : SkillBase
{

	public DummyFireballSkill2()
    {
        Id                = 1;
        Type              = SkillType.Attack;
        Priority          = 1;
        IsRestricteMoving = false;
        Cooldown          = 5f;
        BeforeDelay       = 0.1f;
        Duration          = 0.9f;
        AfterDelay        = 0.2f;
		RequireMP		  = 0f;
	}

	public override void Execute()
	{ 
		base.Execute();

		Debug.Log($"DummyFireballSkill is used");
	}
}

public class DummyHealSkill : SkillBase, IActiveSkill
{
    
    public DummyHealSkill()
    {
        Id                = 2;
        Type              = SkillType.General;
        Priority          = 2;
        IsRestricteMoving = false;
        Cooldown          = 10f;
        BeforeDelay       = 0f;
        Duration          = 1f;
        AfterDelay        = 0f;
		RequireMP		  = 0f;
	}
}

public class DummyStunSkill : SkillBase, IActiveSkill
{
    public DummyStunSkill()
    {
        Id                = 3;
        Type              = SkillType.Attack;
        Priority          = 3;
        IsRestricteMoving = true;
        Cooldown          = 8f;
        BeforeDelay       = 0.2f;
        Duration          = 1.5f;
        AfterDelay        = 0.2f;
		RequireMP		  = 0f;
	}
}

public class DummyShieldSkill : SkillBase, IActiveSkill
{
    public DummyShieldSkill()
    {
        Id                = 4;
        Type              = SkillType.General;
        Priority          = 1;
        IsRestricteMoving = false;
        Cooldown          = 15f;
        BeforeDelay       = 0f;
        Duration          = 4f;
        AfterDelay        = 0f;
		RequireMP		  = 0f;
	}
}

public class DummyDashSkill : SkillBase, IActiveSkill
{
    public DummyDashSkill()
    {
        Id                = 5;
        Type              = SkillType.General;
        Priority          = 2;
        IsRestricteMoving = false;
        Cooldown          = 3f;
        BeforeDelay       = 0f;
        Duration          = 0.5f;
        AfterDelay        = 0f;
		RequireMP		  = 0f;
	}
}

public class DummyLightningSkill : SkillBase, IActiveSkill
{
    public DummyLightningSkill()
    {
        Id                = 6;
        Type              = SkillType.Attack;
        Priority          = 3;
        IsRestricteMoving = false;
        Cooldown          = 4f;
        BeforeDelay       = 0.1f;
        Duration          = 1.2f;
        AfterDelay        = 0.3f;
		RequireMP		  = 0f;
	}
}

public class DummyIceSkill : SkillBase, IActiveSkill
{
    public DummyIceSkill()
    {
        Id                = 7;
        Type              = SkillType.Attack;
        Priority          = 1;
        IsRestricteMoving = true;
        Cooldown          = 6f;
        BeforeDelay       = 0.4f;
        Duration          = 2.5f;
        AfterDelay        = 0.4f;
		RequireMP		  = 0f;
	}
}

public class DummyStealthSkill : SkillBase, IActiveSkill
{
    public DummyStealthSkill()
    {
        Id                = 8;
        Type              = SkillType.General;
        Priority          = 2;
        IsRestricteMoving = false;
        Cooldown          = 10f;
        BeforeDelay       = 0f;
        Duration          = 5f;
        AfterDelay        = 0f;
		RequireMP		  = 0f;
	}
}

public class DummyBarrierSkill : SkillBase, IActiveSkill
{
    public DummyBarrierSkill()
    {
        Id                = 9;
        Type              = SkillType.General;
        Priority          = 3;
        IsRestricteMoving = false;
        Cooldown          = 12f;
        BeforeDelay       = 0f;
        Duration          = 3f;
        AfterDelay        = 0f;
		RequireMP		  = 0f;
	}
}

public class DummySpeedBoostSkill : SkillBase, IActiveSkill
{
    public DummySpeedBoostSkill()
    {
        Id                = 10;
        Type              = SkillType.General;
        Priority          = 1;
        IsRestricteMoving = false;
        Cooldown          = 8f;
        BeforeDelay       = 0f;
        Duration          = 2f;
        AfterDelay        = 0f;
		RequireMP		  = 0f;
	}
}