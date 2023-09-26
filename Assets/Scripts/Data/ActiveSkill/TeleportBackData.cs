using UnityEngine;

[CreateAssetMenu(fileName = nameof(TeleportBackData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(TeleportBackData))]
public class TeleportBackData : ActiveSkillData
{

    public ParticleSystem beforeEffect; // Prefab/Particle/Teleport_blue 1
    public ParticleSystem afterEffect;  // Prefab/Particle/Level_Up_blue 1


    public int telpoDistance;

}