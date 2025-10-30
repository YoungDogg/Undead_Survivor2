using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //public int id;
    public ItemData.ItemType itemType;  // 아이템타입
    public float damage;
    public int amount;
    public int instanceCount;
    public int pierceCount;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (itemType)
        {
            case ItemData.ItemType.Melee:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            case ItemData.ItemType.Range:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damage, int amount)
    {
        this.damage = damage * Character.Damage;
        this.amount += amount;

        if(itemType == ItemData.ItemType.Melee)
        {
            UpdateMeleeWeapon();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemType;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        //id = data.itemId;
        this.itemType = data.itemType;
        damage = data.baseDamage * Character.Damage;
        amount = data.baseCount + Character.Amount;

        
        switch (this.itemType)
        {
            case ItemData.ItemType.Melee:
                speed = 150 * Character.rotationSpeed;
                UpdateMeleeWeapon();
                break;

            case ItemData.ItemType.Range:
                speed = 0.5f * Character.fireCooldown;
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void UpdateMeleeWeapon()
    {
        for(int idx = 0; idx < amount; idx++)
        {
            Transform bullet;
            if (idx < transform.childCount)
            {
                bullet = transform.GetChild(idx);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(PoolType.Shovel).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * idx / amount;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is inifity pierceCount
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        Transform bullet = GameManager.instance.pool.Get(PoolType.Bullet).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, pierceCount, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
