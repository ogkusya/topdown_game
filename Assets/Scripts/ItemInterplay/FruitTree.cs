using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : InterplayObject
{
    [SerializeField] private Vector2 radius;
    [SerializeField] private ItemType[] dropFruits;

    //private int currentCountFruits = 4;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(radius.x, 0.1f, radius.y));
    }

    public override void IterplayObject(CharacterInvetory characterInvetory)
    {
        DropFruit();
    }

    private void DropFruit()
    {
        //currentCountFruits--;
        var dropPosition = transform.position;
        dropPosition += new Vector3(Random.Range(-radius.x / 2f, radius.x / 2f), 0,
            Random.Range(-radius.y / 2f, radius.y / 2f));

        int index = Random.Range(0, 10) % 2 == 0 ? 0 : 1;
        Debug.Log(index);
        var dropFruit = dropFruits[index];
        var droppedGruit = ItemSpawner.Instance.GetItemByType(dropFruit);
        droppedGruit.transform.position = transform.position;
        droppedGruit.transform.rotation = Random.rotation;
        droppedGruit.IsSelectable = false;

        var sequence = DOTween.Sequence();
        sequence.Append(droppedGruit.transform.DOJump(dropPosition, 1.7f, 1, 0.9f).SetEase(Ease.Flash));
        sequence.Join(droppedGruit.transform.DORotate(Vector3.zero, 0.9f));
        sequence.Append(droppedGruit.transform.DOShakeScale(0.3f, 0.4f));
        sequence.OnComplete(() => { droppedGruit.IsSelectable = true; });
    }
}
