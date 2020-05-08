using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Root of all game objects that can be damaged or die.
/// 
/// Handles a bulk of the standard behaviour for you, but should you wish to add more,
/// there are Events setup to do just that.
/// </summary>
public abstract class BaseHealth : MonoBehaviour {

    [Tooltip("How many lives this entity will have.")]
    [SerializeField] private int startingHealth = 3;

    // we want to remember the original health of this object, should we need to re-initialize later
    private int currentHealth;

    //-------------------------TAKING DAMAGE-------------------------//
    [Tooltip("How long to prevent the player from being hurt again.")]
    [SerializeField] private float _invulnerabilityPeriod = 1f;

    // will be turned off/on depending on how long your invulnerability period is
    private bool _canBeDamaged = true;

    // this is an action that other scripts can 'subscribe' to get updates
    public Action<int> OnDamageTaken;

    // allows other classes to add their methods here, to prevent damaging the player
    public delegate bool PreventDamageTaken();

    // used internally to call all those methods. If any of them return true, the entity will not be damaged
    public PreventDamageTaken preventDamageTaken;

    //-------------------------UPON DEATH-------------------------//
    // this is the action you wish to take when you die
    public Action OnDeath;

    protected void Start() {
        currentHealth = startingHealth;
    }

    /// <summary>
    /// Handles doing damage to the player. Defaults to just decrementing the health,
    /// and doing any other calls you wish to do when the player gets damaged.
    /// 
    /// Also has a cooldown time, so you can't get spammed over and over when being damaged.
    /// 
    /// If you want to do extra things to this, add to the event of 'OnDamageTaken'.
    /// </summary>
    public void Damage() {
        // call any other events defined to prevent damage!
        if (preventDamageTaken != null) {
            if (preventDamageTaken()) {
                return;
            }
        }

        // if you can't be hurt right now, just skip this call
        if (!_canBeDamaged) {
            return;
        }

        StandardDamage();

        // did the player die? If so, we may want to do some extra work.
        if (currentHealth == 0 && OnDeath != null){
	        // turn off the ability to be damaged once you've taken enough damage
	        _canBeDamaged = false;
            OnDeath();
        } else {
            StartCoroutine(StartInvulnerabilityRoutine());
        }
    }

    /// <summary>
    /// Decrements the current health of the entity, and also creates
    /// an invulnerability period for the entity too.
    /// 
    /// Also calls OnDamageTaken() event.
    /// </summary>
    private void StandardDamage() {
	    Debug.Log("Damage taken!");
        currentHealth--;

        // if any events were subscribed to this event, call those now!
        if (OnDamageTaken != null) {
            OnDamageTaken(currentHealth);
        }
    }

    /// <summary>
    /// A routine to run and prevent spamming of the player when attacked.
    /// </summary>
    private IEnumerator StartInvulnerabilityRoutine() {
        _canBeDamaged = false;
        yield return new WaitForSeconds(_invulnerabilityPeriod);
        _canBeDamaged = true;
    }
}
