using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerNeeds : MonoBehaviour, IDamageable
{
    public Need health;
    public Need hunger;
    public Need thirst;
    public Need sleep;

    public float noHungerHealthDecay;
    public float noThirstHealthDecay;

    public UnityEvent OnTakeDamage;

    void Start ()
    {
        // initializing starting values
        health.currValue = health.startValue;
        hunger.currValue = hunger.startValue;
        thirst.currValue = thirst.startValue;
        sleep.currValue = sleep.startValue;
    }

    void Update ()
    {
        // decay values over time
        hunger.Subtract(hunger.decayRate * Time.deltaTime);
        thirst.Subtract(thirst.decayRate * Time.deltaTime);
        sleep.Add(sleep.regenRate * Time.deltaTime);

        // we add this so that when we reach 0 for both hungry and thirst, it will decay our HP
        if(hunger.currValue == 0.0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if(thirst.currValue == 0.0f)
        {
            health.Subtract(noThirstHealthDecay * Time.deltaTime);
        }

        // check if player is dead
        if(health.currValue == 0.0f)
        {
            Die();
        }


        // UI - Progress bar updates
        health.uiBar.fillAmount = health.GetPercentage();
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        thirst.uiBar.fillAmount = thirst.GetPercentage();
        sleep.uiBar.fillAmount = sleep.GetPercentage();
    }

    // Player Actions 
    public void Heal (float amount)
    {
        health.Add(amount);
    }

    public void Eat (float amount)
    {
        hunger.Add(amount);
    }

    public void Drink (float amount)
    {
        thirst.Add(amount);
    }

    public void Sleep (float amount)
    {
        sleep.Subtract(amount);
    }

    // player HP
    public void TakePhysicalDmg(int amount)
    {
        health.Subtract(amount);
        OnTakeDamage?.Invoke();
    }

    public void Die ()
    {
        Debug.Log("Player is Dead");
    }
}


[System.Serializable]
public class Need
{
    [HideInInspector]
    public float currValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar;

    // add to the need
    public void Add(float amount)
    {
        currValue = Mathf.Min(currValue + amount, maxValue);
    }

    // subtract from the need
    public void Subtract(float amount)
    {
        currValue = Mathf.Max(currValue - amount, 0.0f);
    }

    // return the percentage value (0.0 - 1.0)
    public float GetPercentage()
    {
        return currValue / maxValue;
    }

}

public interface IDamageable
{
    // every object that is damagable will have the interface

    void TakePhysicalDmg(int damageAmount);
}