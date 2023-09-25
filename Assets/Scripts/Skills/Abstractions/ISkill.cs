using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
	/// <summary>
	/// 스킬을 소유한 캐릭터
	/// </summary>
	Character Owner { get; set; }

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

	/// <summary>
	/// 스킬 초기화 시 호출
	/// </summary>
	void Init();

	/// <summary>
	/// 스킬을 사용할 수 있는지 여부를 반환
	/// </summary>
	/// <returns>스킬을 사용할 수 있는지 여부</returns>
	bool CheckCanUse();

	/// <summary>
	/// 스킬을 실행
	/// </summary>
	void Execute();

	/// <summary>
	/// 기즈모를 그림
	/// </summary>
	/// <param name="character"></param>
	void OnDrawGizmos(Transform character);
}