using UnityEngine;
using Zenject;

public class ShipView : MonoBehaviour
{
    [Inject] private ShipViewModel _shipViewModel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _shipViewModel.UpdateMove();
        _shipViewModel.UpdateRotation();

        transform.position = _shipViewModel.Position;
        transform.rotation = _shipViewModel.Rotation;
    }
}
