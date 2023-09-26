
using UnityEngine;

[CreateAssetMenu(fileName = nameof(MagicMissileData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(MagicMissileData))]
public class MagicMissileData : ActiveSkillData
{
	public GuidedBulletMover missilePrefab;
    public LayerMask damageLayer;
    public float duration;
    public int missileCount;
    public float fireBallDelay;
    
    
    public float range;
    public float bezierDelta;
    public float bezierDelta2;
}
