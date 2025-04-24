using UnityEngine;
using Zenject;

public class BulletView : MonoBehaviour
{
    private BulletModel _model;

    private void Start()
    {
        transform.localScale = new Vector3(2, 2, 2);
        Debug.Log($"BulletView: Created at {transform.position}, scale={transform.localScale}");
    }

    public void Initialize(BulletModel model)
    {
        _model = model;
        gameObject.SetActive(_model.IsActive);
        transform.position = _model.Position;
        Debug.Log($"BulletView: Initialized at {_model.Position}, isActive={_model.IsActive}");
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

    public class Factory : PlaceholderFactory<BulletView>
    {
    }
}