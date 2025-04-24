using UnityEngine;
using Zenject;

public class UfoView : MonoBehaviour
{
    private UfoModel _model;

    public void Initialize(UfoModel model)
    {
        _model = model;
        gameObject.SetActive(_model.IsActive);
        transform.position = _model.Position;
        Debug.Log($"UfoView: Initialized at {_model.Position}");
    }

    private void Update()
    {
        if (_model == null || !_model.IsActive)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = _model.Position;
    }

    public class Factory : PlaceholderFactory<UfoModel, UfoView>
    {
        public override UfoView Create(UfoModel model)
        {
            var view = base.Create();
            view.Initialize(model);
            return view;
        }
    }
}