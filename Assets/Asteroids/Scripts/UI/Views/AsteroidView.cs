using UnityEngine;
using Zenject;

public class AsteroidView : MonoBehaviour
{
    private AsteroidModel _asteroidModel;

    public void Initialize(AsteroidModel model)
    {
        _asteroidModel = model;
        transform.localScale = Vector3.one * model.Size;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_asteroidModel != null && _asteroidModel.IsActive)
        {
            transform.position = _asteroidModel.Position;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public AsteroidModel GetAsteroidModel()
    {
        return _asteroidModel;
    }

    public class Factory : PlaceholderFactory<AsteroidView>
    {
        public AsteroidView Create(AsteroidModel model)
        {
            var view = base.Create();
            view.Initialize(model);
            return view;
        }
    }
}
