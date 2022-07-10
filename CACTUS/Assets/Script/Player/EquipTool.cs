using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    public enum ToolUsedToGather
    {
        Wood,
        Stone
    };

    [Header("Resource Gathering")]
    public bool doesGatherResources;
    public ToolUsedToGather toolType;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    // components
    private Animator anim;
    private Camera cam;

    void Awake()
    {
        // get our components
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    // called when we press the attack input
    public override void OnAttackInput()
    {
        if (!attacking)
        {
            attacking = true;
            anim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }

    // called when we're able to attack again
    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            // did we hit a resource?
            if (doesGatherResources && hit.collider.GetComponent<Resource>())
            {
                var resType = hit.collider.GetComponent<Resource>().resourceType;

                if ((int)resType == (int)toolType)
                {
                    hit.collider.GetComponent<Resource>().Gather(hit.point, hit.normal);
                }
                else
                {
                    Debug.Log("Incorrect Tool!");
                }
            }

            // did we hit a damagable?
            if (doesDealDamage && hit.collider.GetComponent<IDamageable>() != null)
            {
                hit.collider.GetComponent<IDamageable>().TakePhysicalDmg(damage);
            }
        }
    }

}
