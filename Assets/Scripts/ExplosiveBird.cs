using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBird : Bird
{
    public float radius = 5.0F;
    public float power = 10.0F;

    public bool _hasExplode = false;

    public SpriteRenderer radiusRenderer;


    private void Update()
    {
        radiusRenderer.size = new Vector2(radius, radius);

    }
    public override void OnTap()
    {
        radiusRenderer.enabled = true;
        radiusRenderer.size = new Vector2(radius, radius);
    }

    public override void OnBirdHit()
    {
        base.OnBirdHit();
        Explode();
    }

    public void Explode()
    {
        if (!_hasExplode)
        {
            Vector3 explosionPos = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
            foreach (Collider2D hit in colliders)
            {
                if (hit.gameObject.name != this.gameObject.name)
                {
                    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                    Vector2 dir = hit.transform.position - this.transform.position;
                    Vector2 dirNormalized = dir.normalized;
                    float magnitude = dir.magnitude;

                    if (rb != null)
                    {
                        print(dir);
                        print(dirNormalized);
                        print(magnitude);

                        rb.AddForce(dir.normalized * (1 / magnitude) * power, ForceMode2D.Impulse);

                        _hasExplode = true;
                    }
                }

            }
        }
    }
}
