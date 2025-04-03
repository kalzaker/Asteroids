using UnityEngine;

public class ShipViewModel
{
    private readonly ShipModel _shipModel;
    private readonly InputController _inputController;
    
    public Vector3 Position => _shipModel.Position;
    public Quaternion Rotation => _shipModel.Rotation;

    public ShipViewModel(ShipModel shipModel, InputController inputController)
    {
        this._shipModel = shipModel;
        this._inputController = inputController;
    }

    public void UpdateMove()
    {
        _shipModel.Move(_inputController.GetThrustInput());
    }

    public void UpdateRotation()
    {
        _shipModel.Rotate(_inputController.GetRotationInput());
    }
}
