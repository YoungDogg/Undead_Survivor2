using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        foreach(Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // make a copy of item array
        List<int> itemCopyList = new List<int>();
        for(int i = 0; i < items.Length; i++)
        {
            itemCopyList.Add(i);
        }

        int[] ran = new int[3];

        for(int i = 0; i < ran.Length; i++)
        {
            int pickedIdx = Random.Range(0, itemCopyList.Count);
            ran[i] = itemCopyList[pickedIdx];
            itemCopyList.RemoveAt(pickedIdx);
        }

        // if items reach max level, replace it to consumables.
        for(int index=0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];
            int maxLevel = ranItem.data.damages.Length;

            // Replace the max item to consumable item.
            if (ranItem.level == maxLevel)
            {
                // consumable items are from item 4
                items[Random.Range(4, items.Length)].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
                
        }

        
    }
}
