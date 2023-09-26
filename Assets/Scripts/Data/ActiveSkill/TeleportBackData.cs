using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TeleportBackData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(TeleportBackData))]
public class TeleportBackData : ActiveSkillData
{

    public ParticleSystem teleportEffect; // Prefab/Particle/Teleport_blue 1
    public ParticleSystem postEffect;     // Prefab/Particle/Level_Up_blue 1


    public int telpoDistance;

}