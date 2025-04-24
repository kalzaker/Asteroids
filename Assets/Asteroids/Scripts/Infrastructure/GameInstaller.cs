using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private AsteroidView asteroidViewPrefab;
    [SerializeField] private UfoView ufoViewPrefab;
    [SerializeField] private BulletView bulletViewPrefab;

    public override void InstallBindings()
    {
        Debug.Log("GameInstaller: Starting bindings");

        // Модели
        Container.Bind<ShipModel>().AsSingle();
        Debug.Log("GameInstaller: ShipModel bound");

        // Фабрики вьюх
        Container.BindFactory<AsteroidModel, AsteroidView, AsteroidView.Factory>()
            .FromComponentInNewPrefab(asteroidViewPrefab);
        Container.BindFactory<UfoModel, UfoView, UfoView.Factory>()
            .FromComponentInNewPrefab(ufoViewPrefab);
        Container.BindFactory<BulletView, BulletView.Factory>()
            .FromComponentInNewPrefab(bulletViewPrefab);
        Debug.Log("GameInstaller: View factories bound");

        // Фабрики данных
        var enemyFactoryData = new FactoryData<IEnemy>
        {
            MaxCountInstance = 30,
            LifeTime = 10,
            FactoryObjData = new System.Collections.Generic.List<FactoryObjData<IEnemy>>
            {
                new FactoryObjData<IEnemy> { Key = "Asteroid" },
                new FactoryObjData<IEnemy> { Key = "Ufo" }
            }
        };
        var bulletFactoryData = new FactoryData<BulletModel>
        {
            MaxCountInstance = 100,
            LifeTime = 10,
            FactoryObjData = new System.Collections.Generic.List<FactoryObjData<BulletModel>>
            {
                new FactoryObjData<BulletModel> { Key = "Bullet" }
            }
        };
        Container.Bind<FactoryData<IEnemy>>().FromInstance(enemyFactoryData).AsSingle();
        Container.Bind<FactoryData<BulletModel>>().FromInstance(bulletFactoryData).AsSingle();
        Debug.Log("GameInstaller: FactoryData bound");

        // Фабрики
        Container.Bind<EnemyFactory>()
            .AsSingle()
            .WithArguments(
                enemyFactoryData,
                Container.Resolve<DiContainer>(),
                Container.Resolve<ShipModel>(),
                Container.Resolve<AsteroidView.Factory>(),
                Container.Resolve<UfoView.Factory>()
            );
        Container.Bind<BulletFactory>()
            .AsSingle()
            .WithArguments(
                bulletFactoryData,
                Container.Resolve<DiContainer>(),
                Container.Resolve<BulletView.Factory>()
            );
        Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BulletFactory>().AsSingle().NonLazy();
        Debug.Log("GameInstaller: EnemyFactory, BulletFactory bound");

        // Вью корабля
        var shipView = Container.InstantiatePrefabForComponent<ShipView>(shipPrefab, Vector3.zero, Quaternion.identity, null);
        Container.Bind<ShipView>().FromInstance(shipView).AsSingle();
        Debug.Log("GameInstaller: ShipView bound");

        // GameManager
        Container.Bind<GameManager>()
            .FromComponentInNewPrefab(gameManagerPrefab)
            .AsSingle()
            .WithArguments(
                Container.Resolve<EnemyFactory>(),
                Container.Resolve<BulletFactory>()
            );
        Debug.Log("GameInstaller: GameManager bound");

        Debug.Log("GameInstaller: Bindings completed");
    }
}