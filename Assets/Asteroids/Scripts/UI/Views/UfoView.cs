using UnityEngine;
using Zenject;

public class UfoView : MonoBehaviour
{
    private UfoModel _ufoModel;

    public void Initialize(UfoModel model)
    {
        _ufoModel = model;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_ufoModel != null && _ufoModel.IsActive)
        {
            transform.position = _ufoModel.Position;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public UfoModel GetUfoModel()
    {
        return _ufoModel;
    }

    public class Factory : PlaceholderFactory<UfoView>
    {
        public UfoView Create(UfoModel model)
        {
            var view = base.Create();
            view.Initialize(model);
            return view;
        }
    }
}
