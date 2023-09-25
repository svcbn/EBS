public interface IActiveSkill : ISkill
{
	/// <summary>
	/// 스킬의 타입
	/// </summary>
	SkillType Type { get; }

	/// <summary>
	/// 스킬의 우선순위
	/// </summary>
	int Priority { get; }

	/// <summary>
	/// 스킬이 캐릭터의 움직임을 제약하는지 여부
	/// </summary>
	bool IsRestricteMoving { get; }

	/// <summary>
	/// 스킬의 쿨타임이 끝났는지 여부
	/// </summary>
	bool IsCoolReady { get; }

	/// <summary>
	/// 스킬이 실행 중인지 여부
	/// </summary>
	bool IsActing { get; }

	/// <summary>
	/// 스킬의 쿨타임
	/// </summary>
	float Cooldown { get; }

	/// <summary>
	/// 스킬의 선 딜레이
	/// </summary>
	float BeforeDelay { get; }

	/// <summary>
	/// 스킬의 사용 시간
	/// </summary>
	float Duration { get; }

	/// <summary>
	/// 스킬의 후 딜레이
	/// </summary>
	float AfterDelay { get; }
}