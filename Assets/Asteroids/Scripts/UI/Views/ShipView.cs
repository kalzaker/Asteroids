using UnityEngine;
using Zenject;

public class ShipView : MonoBehaviour
{
    [Inject] private ShipViewModel _shipViewModel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        var collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.5f;
    }

    private void Update()
    {
        _shipViewModel.Update();

        transform.position = _shipViewModel.Position;
        transform.rotation = _shipViewModel.Rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AsteroidView>(out var asteroidView))
        {
            var asteroid = asteroidView.GetAsteroidModel();
            if (asteroid != null && asteroid.IsActive)
            {
                _shipViewModel.HandleCollision(asteroid);
            }
        }
        else if (other.TryGetComponent<UfoView>(out var ufoView))
        {
            var ufo = ufoView.GetUfoModel();
            if (ufo != null && ufo.IsActive)
            {
                _shipViewModel.HandleCollision(ufo);
            }
        }
    }
}
