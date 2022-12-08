using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MeeleWeapon : Weapon,ISecondaryAction
{
    [SerializeField] InputActionReference secondaryAction;
    protected const string primaryOneA = "PrimaryAttackOne";
    protected int primaryOneProbability = 70;
    protected const string primaryTwoA = "PrimaryAttackTwo";
    protected int primaryTwoProbability = 30;
    protected const string secondaryA = "SecondaryAttack";
    [Header("Secondary Attack")]
    [SerializeField] protected float secondaryAttackDelay;
    [SerializeField] protected float secondaryAttackDamage;
    protected float nextSecondaryAttackTime;
    [Header("Keybind")]
    [SerializeField] protected KeyCode secondaryActionKey = KeyCode.Mouse1;
    public KeyCode SecondaryActionKey { get{return secondaryActionKey;}}
    

    [Header("SoundKit")]
    public MeeleWeaponSoundKit weaponSounds;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(canAttack)
        {
            if(Time.time > nextSecondaryAttackTime && secondaryAction.action.IsPressed())
            {
                SecondaryAction();
                nextSecondaryAttackTime = Time.time + secondaryAttackDelay;
                StartCoroutine(DisableCanAttack(primaryAttackDelay));

            }
        }
    }
    public override void PrimaryAction()
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
        AudioManager.playSound(AudioSource,weaponSounds.SwingSound);
        StartCoroutine(DisableCanAttack(primaryAttackDelay));
        ProcessRaycast(primaryAttackDamage);
    }

    public virtual void SecondaryAction()
    {
        weaponAnimation.WeaponAttack(secondaryA);
        AudioManager.playSound(AudioSource,weaponSounds.SwingSound);
        ProcessRaycast(secondaryAttackDamage);
    }
    protected override void CreateHitImpact(RaycastHit hit)
    {
        base.CreateHitImpact(hit);
        AudioManager.playSound(AudioSource,weaponSounds.HitSound);


    }
}
