using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    SingleAttack, //e.g. a fireball
    MultiAttack, //e.g. a sustained burst of icicle shards
    TerrainAttack, //e.g. ridges of stone/ice pop out from the ground, or a staff slam into the ground, and the earth shard/boulders fly out

    Status, // Buffs, poison, etc.
    None
}

[CreateAssetMenu(menuName = "Spell/Magic Spell")]
public class MagicSpell : ScriptableObject
{
    public SpellType spellType;
    public SpellAnim anim;
    public SpellEff effect;
    public List<GameObject> spellObjectPrefabs;
}
