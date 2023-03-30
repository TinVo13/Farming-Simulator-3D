using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCRelationshipListing : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite emptyHeart, fullHeart;

    [Header("Sprites")]
    public Image portraitImage;
    public Text nameText;
    public Image[] hearts;

    public void Display(CharacterData characterData, NPCRelationshipState relationship)
    {
        portraitImage.sprite = characterData.portrait;
        nameText.text = relationship.name;

        DisplayHearts(relationship.Hearts());
    }

    void DisplayHearts(float number)
    {
        foreach(Image heart in hearts)
        {
            heart.sprite = emptyHeart;
        }

        for(int i = 0; i < number; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }
}
