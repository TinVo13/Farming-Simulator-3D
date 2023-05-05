using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class InteractableCharacter : InteractableObject
{
    public CharacterData characterData;

    NPCRelationshipState relationship;

    Quaternion defaultRotation;
    bool isTurning = false;

    private void Start()
    {
        relationship = RelationshipStats.GetRelationship(characterData);

        defaultRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
    }

    public override void PickUp()
    {
        LookAtPlayer();
        TriggerDialogue();
    }

    #region Rotation
    void LookAtPlayer()
    {
        Transform player = FindObjectOfType<SimpleSampleCharacterControl>().transform;

        Vector3 dir = player.position - transform.position;

        dir.y = 0;

        Quaternion lookRot = Quaternion.LookRotation(dir);

        StartCoroutine(LookAt(lookRot));
    }

    IEnumerator LookAt(Quaternion lookRot)
    {
        if(isTurning)
        {
            isTurning = false;
        }
        else
        {
            isTurning = true;
        }

        while(transform.rotation != lookRot)
        {
            if(!isTurning)
            {
                yield break;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 720 * Time.fixedDeltaTime);
            yield return new WaitForEndOfFrame();
        }
        isTurning = true;
    }

    void ResetRotation()
    {
        StartCoroutine(LookAt(defaultRotation));
    }
    #endregion

    #region Conversation Interactions
    void TriggerDialogue()
    {
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            GiffDialogue();
        }

        List<DialogueLine> dialogueToHave = characterData.defaultDialogue;

        System.Action onDialogueEnd = null;

        onDialogueEnd += ResetRotation;

        if (RelationshipStats.FirstMeeting(characterData))
        {
            dialogueToHave = characterData.onFirstMeet;
            onDialogueEnd += OnFirstMeeting;
        }

        if (RelationshipStats.IsFirstConversationOfTheDay(characterData))
        {
            onDialogueEnd += OnFirstConversation;
        }

        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }   

    void GiffDialogue()
    {
        if (!EligibleForGift()) return;

        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);

        List<DialogueLine> dialogueToHave = characterData.neutralGiftDialogue;

        System.Action onDialogueEnd = () =>
        {
            relationship.giftGivenToday = true;

            InventoryManager.Instance.ConsumeItem(handSlot);
        };

        onDialogueEnd += ResetRotation;

        bool isBirthday = RelationshipStats.IsBirthday(characterData);

        int pointsToAdd = 0;

        switch(RelationshipStats.GetReactionToGift(characterData, handSlot.itemData))
        {
            case RelationshipStats.GiftReaction.Like:
                dialogueToHave = characterData.likedGiftDialogue;
                pointsToAdd = 80;
                if (isBirthday) dialogueToHave = characterData.birthdayLikedGiftDialogue;

                break;
            case RelationshipStats.GiftReaction.Dislike:
                dialogueToHave = characterData.dislikedGiftDialogue;
                pointsToAdd = -20;
                if (isBirthday) dialogueToHave = characterData.birthdayDislikedGiftDialogue;
                break;
            case RelationshipStats.GiftReaction.Neutral:
                dialogueToHave = characterData.neutralGiftDialogue;
                pointsToAdd = 20;
                if (isBirthday) dialogueToHave = characterData.birthdayNeutralGiftDialogue;
                break;
        }
        if (isBirthday) pointsToAdd *= 8;

        RelationshipStats.AddFriendPoints(characterData, pointsToAdd);

        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }

    bool EligibleForGift()
    {
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "FirstMettingKey");
        if (RelationshipStats.FirstMeeting(characterData))
        {
            DialogueManager.Instance.StartDialogue
                (DialogueManager.CreateSimpleMessage(text));
            return false;
        }

        string textTodayGiven = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "TodayGivenKey");
        string textGift = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "GiftKey");

        if (RelationshipStats.GiftGivenToday(characterData))
        {
            DialogueManager.Instance.StartDialogue
                (DialogueManager.CreateSimpleMessage(textTodayGiven + $" {characterData.name} " + textGift ));
            return false;
        }

        return true;
    }


    void OnFirstMeeting()
    {
        RelationshipStats.UnlockCharacter(characterData);
        relationship = RelationshipStats.GetRelationship(characterData);
    }

    void OnFirstConversation()
    {
        Debug.Log("This is the first conversation of the day");

        RelationshipStats.AddFriendPoints(characterData, 20);

        relationship.hasTalkedToday = true;
    }
    #endregion


}
