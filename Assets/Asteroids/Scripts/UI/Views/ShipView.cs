using UnityEngine;
using Zenject;

public class ShipView : MonoBehaviour
{
    private ShipModel _shipModel;
    private GameManager _gameManager;

    [Inject]
    public void Construct(ShipModel shipModel, GameManager gameManager)
    {
        _shipModel = shipModel;
        _gameManager = gameManager;
        Debug.Log("ShipView: Initialized");
    }

    private void Update()
    {
        float moveSpeed = 5f;
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 newPosition = transform.position + input.normalized * moveSpeed * Time.deltaTime;
        _shipModel.UpdatePosition(newPosition);
        transform.position = newPosition;

        if (input.magnitude > 0)
        {
            _shipModel.UpdateDirection(input.normalized);
            transform.rotation = Quaternion.LookRotation(input.normalized);
        }

        if (Input.GetKey(KeyCode.Space) && _shipModel.CanShoot())
        {
            _gameManager.Shoot(transform.position + transform.forward, transform.forward);
            Debug.Log($"ShipView: Fired bullet at {transform.position}");
        }
    }
}