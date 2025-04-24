using UnityEngine;
using Zenject;

public class AsteroidView : MonoBehaviour
{
    private AsteroidModel _model;

    public void Initialize(AsteroidModel model)
    {
        _model = model;
        transform.localScale = Vector3.one * _model.Size;
        gameObject.SetActive(_model.IsActive);
        transform.position = _model.Position;
        Debug.Log($"AsteroidView: Initialized at {_model.Position}, size={_model.Size}, isFragment={_model.IsFragment}");
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

    public class Factory : PlaceholderFactory<AsteroidModel, AsteroidView>
    {
        public override AsteroidView Create(AsteroidModel model)
        {
            var view = base.Create();
            view.Initialize(model);
            return view;
        }
    }
}