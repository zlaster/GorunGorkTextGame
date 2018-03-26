﻿using UnityEngine;

/// <summary>
/// Base de la creación de NPCs. Para poner todos sus valores y después crear un GameObject a partir de esto.
/// </summary>
public abstract class NPCTemplate : ScriptableObject {

    public string npcName = "Lukashenko";
    public string[] keyword;
    public int npcLevel = 0;
    public string npcGender = "macho";
    public Race npcRace;
    public Job npcJob;
    [TextArea] public string npcDescription;
}