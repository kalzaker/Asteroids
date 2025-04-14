using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private AsteroidView asteroidViewPrefab;
    [SerializeField] private UfoView ufoViewPrefab;
    [SerializeField] private BulletView bulletViewPrefab;
    [SerializeField] private LaserView laserViewPrefab;
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private GameOverUI gameOverUIPrefab;
    [SerializeField] private UIManager uiManagerPrefab;
    public override void InstallBindings()
    {
        Container.Bind<ShipModel>().AsSingle();
        Container.Bind<InputController>().AsSingle();
        Container.Bind<BulletPool>().AsSingle();
        Container.Bind<LaserPool>().AsSingle();
        Container.Bind<EnemyPool>().AsSingle().WithArguments(10);
        Container.Bind<ShipViewModel>().AsSingle();
        
        var shipView = Container.InstantiatePrefabForComponent<ShipView>(shipPrefab, Vector3.zero, Quaternion.identity, null);
        Container.Bind<ShipView>().FromInstance(shipView).AsSingle();
        
        Container.Bind<EnemyFactory>().AsSingle().WithArguments(Container.Resolve<ShipModel>());
        Container.BindFactory<AsteroidView, AsteroidView.Factory>()
            .FromComponentInNewPrefab(asteroidViewPrefab);
        Container.BindFactory<UfoView, UfoView.Factory>()
            .FromComponentInNewPrefab(ufoViewPrefab);
        Container.BindFactory<BulletView, BulletView.Factory>()
            .FromComponentInNewPrefab(bulletViewPrefab);
        Container.BindFactory<LaserView, LaserView.Factory>()
            .FromComponentInNewPrefab(laserViewPrefab);
        
        Container.Bind<GameManager>().FromComponentInNewPrefab(gameManagerPrefab).AsSingle().NonLazy();
        Container.Bind<GameOverUI>().FromInstance(gameOverUIPrefab).AsSingle().NonLazy();
        Container.Bind<UIManager>().FromInstance(uiManagerPrefab).AsSingle().NonLazy();
    }
}