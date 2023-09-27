using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ISkill
{
	private Character _owner;

	public uint Id { get; protected set; }

	public Character Owner
	{
		get => _owner;
		set
		{
			if (_owner != null && _owner != value)
			{
				throw new InvalidOperationException($"Owner is already set. Owner: {_owner.name}");
			}

			_owner = value;
		}
	}

	public SkillType Type { get; protected set; }

	public int Priority { get; protected set; }

	public bool IsRestrictMoving { get; protected set; }

	public float Cooldown { get; protected set; }

	public float CurrentCooldown { get; protected set; }

	public float BeforeDelay { get; protected set; }

	public float Duration { get; protected set; }

	public float AfterDelay { get; protected set; }

	public int RequireMP { get; protected set; }

	public bool IsCoolReady { get; protected set; }

	public bool IsActing { get; protected set; }

	public bool IsBeforeDelay { get; protected set; }

	public Coroutine UsingSkillCo { get; protected set; }

	protected virtual void Awake()
	{
		IsCoolReady = true;
	}

	public virtual void Init()
	{
	}

	public virtual void Execute()
	{
		//Debug.Log(GetType().Name);

		IsCoolReady = false;
		IsActing = true;

		CalculateCooltime();

		IsBeforeDelay = true;

		Invoke("ExecuteImpl", BeforeDelay);
	}	

	void ExecuteImpl()
	{
		IsBeforeDelay = false;
		UsingSkillCo = Owner.StartCoroutine(ExecuteImplCo());
	}
	
	public abstract IEnumerator ExecuteImplCo();


	public abstract bool CheckCanUse();

	protected virtual void CalculateCooltime()
	{
		Owner.StartCoroutine(CoCalculateTime(Cooldown, time => CurrentCooldown = time, () => IsCoolReady = true));
		float delay = BeforeDelay + Duration + AfterDelay;
		Owner.StartCoroutine(CoCalculateTime(delay, null, () => IsActing = false));
	}

	protected virtual bool CheckEnemyInBox(Vector2 center, Vector2 size)
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;
		
		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * center.x, center.y);

		var boxes = Physics2D.OverlapBoxAll(centerInWorld, size, 0);
		bool flag = boxes.Length != 0;
		return boxes.Any(box => box.TryGetComponent<Character>(out var character) && character != Owner);
	}

	protected virtual bool CheckEnoughMP(int _mp)
	{
		// float mp = owner의 mp 얻어오기
		// _mp와 owner의 현재 mp 비교
		return true;
	}

	private static IEnumerator CoCalculateTime(float time, Action<float> onUpdate, Action onFinish)
	{
		float timer = 0;
		while (timer < time)
		{
			yield return null;
			timer += Time.deltaTime;
			onUpdate?.Invoke(timer);
		}

		onFinish?.Invoke();
	}

	public void CancelExecute()
	{
		CancelInvoke("ExecuteImpl");
	}

	protected void PlayEffect(string effName, float duration, Vector2 offset, float sign = 1)
	{
		StartCoroutine(PlayEffectCo(effName, duration, offset, sign));
	}

	IEnumerator PlayEffectCo(string effName, float duration, Vector2 offset, float sign)
	{

		Transform parent = Owner.transform;
		GameObject effect = Managers.Resource.Instantiate("Skills/"+effName, parent ); // paraent를 character.gameObject로
		
		if(effect){
			effect.transform.localPosition = Vector3.zero;
		}else{
			Debug.LogError($"effect is null. effName :{effName}");
		}

		effect.transform.localPosition += (Vector3)offset;
		effect.transform.localScale = new Vector3(sign * Owner.transform.localScale.x, Owner.transform.localScale.y, Owner.transform.localScale.z);

		yield return new WaitForSeconds(duration); // 이펙트 재생 시간
		Managers.Resource.Release(effect);
		
	}

    protected void DebugRay(Vector2 from, Vector2 dir)
    {
        Debug.DrawLine(from + new Vector2(dir.x, dir.y) / 2, from + new Vector2(-dir.x, dir.y) / 2, Color.red, 1f);
        Debug.DrawLine(from + new Vector2(-dir.x, -dir.y) / 2, from + new Vector2(dir.x, -dir.y) / 2, Color.red, 1f);
        Debug.DrawLine(from + new Vector2(-dir.x, dir.y) / 2, from + new Vector2(-dir.x, -dir.y) / 2, Color.red, 1f);
        Debug.DrawLine(from + new Vector2(dir.x, dir.y) / 2, from + new Vector2(dir.x, -dir.y) / 2, Color.red, 1f);
    }

}