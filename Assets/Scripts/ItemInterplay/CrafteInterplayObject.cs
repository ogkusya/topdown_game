﻿using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CrafteInterplayObject : InterplayObject
{
    [SerializeField] private CraftConfiguration[] craftConfigurations;
    [SerializeField] private Item craftItem;
    [SerializeField] private Transform endCraftPosition;

    private ItemSpawner itemSpawner;

    public override void IterplayObject(CharacterInvetory characterInvetory)
    {
        var allObjectAttach = true;
        foreach (var craftConfiguration in craftConfigurations)
        {
            if (craftConfiguration.AttachItem != null)
            {
                continue;
            }

            var foundItem = characterInvetory.GetItemByType(craftConfiguration.ItemType);
            if (foundItem)
            {
                craftConfiguration.AttachItem = foundItem;
                foundItem.IsSelectable = false;
                foundItem.transform.position = craftConfiguration.ObjectPosition.position;
                foundItem.transform.rotation = craftConfiguration.ObjectPosition.rotation;
                foundItem.transform.DOKill();
                foundItem.transform.DOShakeScale(0.4f, 0.3f);
                return;
            }
            else
            {
                allObjectAttach = false;
            }
        }

        if (allObjectAttach)
        {
            foreach (var craftConfiguration in craftConfigurations)
            {
                var transformConfiguration = craftConfiguration.AttachItem.transform;
                var returnToPoolObject = craftConfiguration.AttachItem;
                transformConfiguration.DOScale(Vector3.zero, 0.3f)
                    .OnComplete(() => returnToPoolObject.ReturnToPool());
                craftConfiguration.AttachItem = null;
            }

            StartCoroutine(SpawnCraftableObject());
        }
    }

    private IEnumerator SpawnCraftableObject()
    {
        itemSpawner ??= ServiceLocator.GetService<ItemSpawner>();
        yield return new WaitForSeconds(0.3f);
        var newObject = itemSpawner.GetItemByType(ItemType.Cookies);
        newObject.transform.position = endCraftPosition.position;
        newObject.transform.rotation = endCraftPosition.rotation;
        newObject.transform.DOKill();
        newObject.transform.DOShakeScale(0.4f, 0.3f);
    }
}