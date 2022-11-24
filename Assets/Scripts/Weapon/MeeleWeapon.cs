using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleWeapon : Weapon
{
    [SerializeField] protected KeyCode secondaryAction = KeyCode.Mouse1;
    [SerializeField] protected float secondaryAttackDelayMultiplier;

    protected const string primaryOneA = "PrimaryAttackOne";
    int primaryOneProbability = 80;
    protected const string primaryTwoA = "PrimaryAttackTwo";
    int primaryTwoProbability = 20;
    protected const string secondaryA = "SecondaryAttack";
    

    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(canAttack && Input.GetKeyDown(secondaryAction))
        {
            SecondaryAttack();
            nextAttackTime = Time.time + (attackDelay * secondaryAttackDelayMultiplier);

        }
    }
    public override void Attack()
    {
        int animationToPlay = Random.Range(0,100);
        if(animationToPlay <= primaryOneProbability)
        {
            weaponAnimation.WeaponAttack(primaryOneA);
        }
        else
        {
            weaponAnimation.WeaponAttack(primaryTwoA);
        }
    }
    public virtual void SecondaryAttack()
    {
        weaponAnimation.WeaponAttack(secondaryA);
    }
}
