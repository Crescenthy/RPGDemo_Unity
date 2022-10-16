using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public event Action<int, int> UpdataHealthBarOnAttack;

    public CharacterDate_SO templateData;
    public CharacterDate_SO characterDate;
    public AttackData_SO attackData;
    public AttackData_SO baseAttackData;
    public RuntimeAnimatorController baseAnimator;

    [Header("Weapon")]
    public Transform weaponSlot;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if (templateData != null)
            characterDate = Instantiate(templateData);

        //baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
        //baseAttackData = Instantiate(attackData);
    }


    #region Read from Data_SO
    public int MaxHealth
    {
        get { if (characterDate != null) return characterDate.maxHealth; else return 0; }
        set { characterDate.maxHealth = value; }
    }

    public int CurrentHealth
    {
        get { if (characterDate != null) return characterDate.currentHealth; else return 0; }
        set { characterDate.currentHealth = value; }
    }

    public int BaseDefence
    {
        get { if (characterDate != null) return characterDate.baseDefence; else return 0; }
        set { characterDate.baseDefence = value; }
    }

    public int CurrentDefence
    {
        get { if (characterDate != null) return characterDate.currentDefence; else return 0; }
        set { characterDate.currentDefence = value; }
    }
    #endregion


    #region Character Combat
    public void TakeDamage(CharacterStates attacker, CharacterStates defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence, 0);
        defender.CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }

        //update UI
        UpdataHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //经验update
        if (CurrentHealth <= 0)
            attacker.characterDate.UpdateExp(characterDate.killPoint);
    }

    public void TakeDamage(int damge, CharacterStates denfender)
    {
        int currentDamage = Mathf.Max(damge - denfender.CurrentDefence, 0);
        denfender.CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);

        //update UI
        UpdataHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //经验update
        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterDate.UpdateExp(characterDate.killPoint);
    }

    private int CurrentDamage()
    {
        float coreDamge = UnityEngine.Random.Range(attackData.minDamge, attackData.maxDamge);
        if (isCritical)
        {
            coreDamge *= attackData.criticalMultiplier;
        }
        return (int)coreDamge;
    }



    #endregion


    #region Equip Weapon
    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }

    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab != null)
        {
            Instantiate(weapon.weaponPrefab, weaponSlot);
        }

        //更新属性
        //切换动画
        attackData.ApplyWeaponData(weapon.weaponData);
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        //InventoryManager.Instance.UpdateStatsText(MaxHealth, attackData.minDamge, attackData.maxDamge);
    }

    public void UnEquipWeapon()
    {
        if (weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }
        //更新属性      
        attackData.ApplyWeaponData(baseAttackData);
        //切换动画
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }

    #endregion


    #region Apply Data Change
    public void ApplyHealth(int amount)
    {
        if (CurrentHealth + amount <= MaxHealth)
            CurrentHealth += amount;
        else
            CurrentHealth = MaxHealth;
    }

    #endregion

}
