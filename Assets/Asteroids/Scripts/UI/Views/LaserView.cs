using UnityEngine;
using Zenject;

public class LaserView : MonoBehaviour
{
    private LaserModel _laserModel;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.positionCount = 2;
    }

    public void Initialize(LaserModel model)
    {
        _laserModel = model;
        UpdateLaser();
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_laserModel != null && _laserModel.IsActive)
        {
            UpdateLaser();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateLaser()
    {
        _lineRenderer.SetPosition(0, _laserModel.StartPosition);
        _lineRenderer.SetPosition(1, _laserModel.StartPosition + _laserModel.Direction * 1000f);
    }

    public class Factory : PlaceholderFactory<LaserView>
    {
        public LaserView Create(LaserModel model)
        {
            var view = base.Create();
            view.Initialize(model);
            return view;
        }
    }
}