using Bindito.Core;
using DraggableUtils.Buttons;
using DraggableUtils.Factorys;
using DraggableUtils.Tools;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.BottomBarSystem;

namespace DraggableUtils.Configurators
{
    [Configurator(SceneEntrypoint.InGame)]
    public class DraggableUtilsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DraggableToolFactory>().AsSingleton();
            
            containerDefinition.Bind<DraggableUtilsGroup>().AsSingleton();
            containerDefinition.Bind<PauseTool>().AsSingleton();
            containerDefinition.Bind<HaulPrioritizeTool>().AsSingleton();
            containerDefinition.Bind<EmptyStorageTool>().AsSingleton();

            containerDefinition.Bind<DraggableToolButtons>().AsSingleton();
            containerDefinition.MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
            
        }
        
        private class BottomBarModuleProvider : IProvider<BottomBarModule>
        {
            private readonly DraggableToolButtons _draggableToolButtons;

            public BottomBarModuleProvider(DraggableToolButtons draggableToolButtons) => this._draggableToolButtons = draggableToolButtons;

            public BottomBarModule Get()
            {
                BottomBarModule.Builder builder = new BottomBarModule.Builder();
                // The index space [10000; 10010) in the "blue area" is now claimed. Don't add your tool groups there!
                builder.AddLeftSectionElement((IBottomBarElementProvider) this._draggableToolButtons, 10000);
                return builder.Build();
            }
        }
    }
}
