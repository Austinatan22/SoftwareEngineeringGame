using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{
    public CharacterSelect[] character;

    public int CharacterCount
    {
        get
        {
            return character.Length;
        }
    }
    public CharacterSelect getCharacter(int index)
    {
        return character[index];
    }
}