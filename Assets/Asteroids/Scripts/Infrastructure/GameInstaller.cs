using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private AsteroidView asteroidViewPrefab;
    [SerializeField] private UfoView ufoViewPrefab;
    [SerializeField] private GameManager gameManagerPrefab;
    public override void InstallBindings()
    {
        Container.Bind<ShipModel>().AsSingle();
        Container.Bind<InputController>().AsSingle();
        Container.Bind<ShipViewModel>().AsSingle();
        
        ShipView shipView = Container.InstantiatePrefabForComponent<ShipView>(shipPrefab, Vector3.zero, Quaternion.identity, null);
        Container.Bind<ShipView>().FromInstance(shipView).AsSingle();
        
        Container.Bind<EnemyFactory>().AsSingle().WithArguments(Container.Resolve<ShipModel>());
        Container.Bind<EnemyPool>().AsSingle().WithArguments(10);
        Container.BindFactory<AsteroidView, AsteroidView.Factory>()
            .FromComponentInNewPrefab(asteroidViewPrefab);
        Container.BindFactory<UfoView, UfoView.Factory>()
            .FromComponentInNewPrefab(ufoViewPrefab);
        Container.Bind<GameManager>().FromComponentInNewPrefab(gameManagerPrefab).AsSingle().NonLazy();
    }
}