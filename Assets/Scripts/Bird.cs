using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    public GameObject Parent;
    public Rigidbody2D RigidBody;
    public CircleCollider2D Collider;

    public BirdState _state;
    public BirdState State{ get { return _state; } }

    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };


    // Start is called before the first frame update
    void Start()
    {
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    private void FixedUpdate()
    {
        if(_state == BirdState.Idle && 
            RigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if((_state == BirdState.Thrown || _state == BirdState.HitSomething) &&
            RigidBody.velocity.sqrMagnitude < _minVelocity &&
            !_flagDestroy)
        {
            // Destroy gameObject after 2 seconds
            // If the velocity is already below minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;

        OnBirdShot(this);
    }

    private void OnDestroy()
    {
        if(_state == BirdState.Thrown || _state == BirdState.HitSomething)
            OnBirdDestroyed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _state = BirdState.HitSomething;
        OnBirdHit();
    }

    public virtual void OnTap()
    {

    }

    public virtual void OnBirdHit()
    {

    }


}
