using System;
using UnityEngine;
using Zenject;

public class BulletView : MonoBehaviour
{
    private BulletModel _bulletModel;

    private void Awake()
    {
        var collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.5f;
    }

    public void Initialize(BulletModel model)
    {
        _bulletModel = model;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_bulletModel != null && _bulletModel.IsActive)
        {
            transform.position = _bulletModel.Position;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_bulletModel.IsActive) return;

        if (other.TryGetComponent<AsteroidView>(out var asteroidView))
        {
            var asteroid = asteroidView.GetAsteroidModel();
            if (asteroid != null && asteroid.IsActive)
            {
                asteroid.TakeDamage(1);
                _bulletModel.Deactivate();
            }
        }
        else if (other.TryGetComponent<UfoView>(out var ufoView))
        {
            var ufo = ufoView.GetUfoModel();
            if (ufo != null && ufo.IsActive)
            {
                ufo.TakeDamage(1);
                _bulletModel.Deactivate();
            }
        }
    }

    public class Factory : PlaceholderFactory<BulletView>
    {
        public BulletView Create(BulletModel model)
        {
            var view = base.Create();
            view.Initialize(model);
            return view;
        }
    }
}