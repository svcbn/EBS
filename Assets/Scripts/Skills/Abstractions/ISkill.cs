public interface ISkill
{
	/// <summary>
	/// 스킬의 아이디
	/// </summary>
	uint Id { get; }

	/// <summary>
	/// 스킬을 소유한 캐릭터
	/// </summary>
	Character Owner { get; set; }

	/// <summary>
	/// 스킬의 쿨타임
	/// </summary>
	float Cooldown { get; }

	/// <summary>
	/// 스킬의 현재 쿨타임
	/// </summary>
	float CurrentCooldown { get; }

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
}