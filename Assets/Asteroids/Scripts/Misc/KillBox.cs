using UnityEngine;

public class KillBox : MonoBehaviour
{
    private BoxCollider _collider;
    private float _radius;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _radius = ConfigLoader.LoadWorldConfig().killColliderRadius;
        _collider.size = new Vector3(_radius, _radius, _radius);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<AsteroidView>(out var asteroidView))
        {
            var model = asteroidView.GetAsteroidModel();
            model?.Deactivate();
        }
    }
}
