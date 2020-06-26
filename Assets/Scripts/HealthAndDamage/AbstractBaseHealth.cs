using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Root of all game objects that can be damaged or die.
/// 
/// Handles a bulk of the standard behaviour for you, but should you wish to add more,
/// there are Events setup to do just that.
/// </summary>
public abstract class AbstractBaseHealth : MonoBehaviour {

    [Tooltip("How many lives this entity will have.")]
    [SerializeField] private int _startingHealth = 3;

    // we want to remember the original health of this object, should we need to re-initialize later
    // intended to be kept within the range of 0-startingHealth
    private int _currentHealth;

    //-------------------------TAKING DAMAGE-------------------------//
    [Tooltip("How long to prevent the object from being hurt again.")]
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
        _currentHealth = _startingHealth;
    }

    /// <summary>
    /// Handles doing damage to the player. Defaults to just decrementing the health,
    /// and doing any other calls you wish to do when the player gets damaged.
    /// 
    /// Also has a cooldown time, so you can't get spammed over and over when being damaged.
    /// 
    /// If you want to do extra things to this, add to the event of 'OnDamageTaken'.
    /// </summary>
    public void Damage(int damageApplied) {
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

        ApplyDamageAndNotify(damageApplied);

        // did the player die? If so, we may want to do some extra work.
        if (_currentHealth == 0 && OnDeath != null){
	        // turn off the ability to be damaged once you've taken enough damage
	        _canBeDamaged = false;
            OnDeath();
        } else {
            StartCoroutine(StartInvulnerabilityRoutine());
        }
    }

    /// <summary>
    /// Applies the amount of damage you asked for
    /// and calls OnDamageTaken() event.
    ///
    /// Makes sure health stays within a 0-X range.
    /// </summary>
    private void ApplyDamageAndNotify(int damageApplied) {
	    Debug.Log("[" + gameObject.name + "] had " + damageApplied + " damage applied.");
	    _currentHealth -= damageApplied;
	    
	    // catch if we went under 0
	    if (_currentHealth < 0){
		    _currentHealth = 0;
	    }

        // if any events were subscribed to this event, call those now!
        if (OnDamageTaken != null) {
            OnDamageTaken(_currentHealth);
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
