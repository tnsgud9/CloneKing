using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    public float redColorTwinkleSpeed = 4.0f;
    public float explosionPower = 8.0f;
    public float explosionRange = 0.75f;

    private SpriteRenderer _ownerSpriteRenderer;
    private TextMesh _textMesh;
    private MeshRenderer _meshRenderer;
    private GameObject _ownerPlayer;
    private GameObject _originExplosionEffects;

    public void StartExplosion( GameObject owner, float time)
    {
        _ownerPlayer = owner;
        _ownerSpriteRenderer = owner.GetComponent<SpriteRenderer>();

        StartCoroutine(DriveExplosionEffects( time));
    }

    private void InitializeComponents()
    {
        _textMesh = GetComponent<TextMesh>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.sortingOrder = 10002;

        _originExplosionEffects = Resources.Load<GameObject>("Prefabs/Effects/Explosion");
    }

    void Awake()
    {
        InitializeComponents();
    }


    private IEnumerator DriveExplosionEffects( float time)
    {
        float duration = 0.0f;

        float colorDeltaFlip = 1.0f;
        float currentRedWidget = 0.0f;

        while (duration < time)
        {
            currentRedWidget += redColorTwinkleSpeed * Time.deltaTime * colorDeltaFlip;

            if (currentRedWidget <= 0.0f)
            {
                currentRedWidget = 0.0f;
                colorDeltaFlip *= -1.0f;
            }


            if (currentRedWidget >= 1.0f)
            {
                currentRedWidget = 1.0f;
                colorDeltaFlip *= -1.0f;
            }

            _ownerSpriteRenderer.color = new Color(1.0f, 1.0f - currentRedWidget, 1.0f - currentRedWidget, 1.0f);

            _textMesh.text = Mathf.Max((int)((time + 1.0f) - duration), 0).ToString();

            duration += Time.deltaTime;


            yield return new WaitForEndOfFrame();
        }

        ExplodeSelf();
    }

    private void ExplodeSelf()
    {
        _ownerSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        Instantiate(_originExplosionEffects, transform.position, Quaternion.identity);

        int layerIndex = 1 << LayerMask.NameToLayer("Player");
        foreach (var collider in Physics2D.OverlapCircleAll(transform.position, explosionRange, layerIndex))
        {
            var rigidBody = collider.gameObject.GetComponent<Rigidbody2D>();

            if (rigidBody != null)
            {
                Vector3 direction = collider.gameObject.transform.position - transform.position;

                direction.Normalize();

                rigidBody.velocity = rigidBody.velocity + new Vector2(direction.x, 1.0f) * explosionPower;
            }
        }

        Destroy(gameObject);
    }
}
