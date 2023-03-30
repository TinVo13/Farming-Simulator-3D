using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RelationshipListingManager : ListingManager<NPCRelationshipState>
{
    List<CharacterData> characters;

    protected override void DisplayListing(NPCRelationshipState relationship, GameObject listingGameObject)
    {
        if (characters == null)
        {
            LoadAllCharacters();
        }

        CharacterData characterData = GetCharacterDataFromString(relationship.name);
        listingGameObject.GetComponent<NPCRelationshipListing>().Display(characterData, relationship);
    }

    public CharacterData GetCharacterDataFromString(string name)
    {
        return characters.Find(i => i.name == name);
    }

    void LoadAllCharacters()
    {
        CharacterData[] characterDatabase = Resources.LoadAll<CharacterData>("Characters");
        characters = characterDatabase.ToList();
    }
}
