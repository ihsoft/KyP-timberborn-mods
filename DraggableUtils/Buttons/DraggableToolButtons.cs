using System.Collections.Generic;
using DraggableUtils.Factorys;
using DraggableUtils.Tools;
using KSPDev.ProcessingUtils;
using TimberApi.AssetSystem;
using Timberborn.BottomBarSystem;
using Timberborn.BuilderPrioritySystemUI;
using Timberborn.ToolSystem;
using UnityEngine;

namespace DraggableUtils.Buttons
{
    public class DraggableToolButtons : IBottomBarElementProvider
    {
        private readonly ToolGroupButtonFactory _toolGroupButtonFactory;

        private readonly DraggableUtilsGroup _draggableUtilsGroup;
        
        private readonly ToolButtonFactory _toolButtonFactory;

        private readonly DraggableToolFactory _draggableToolFactory;

        private readonly HaulPrioritizeTool _haulPrioritizeTool;

        private readonly IAssetLoader _assetLoader;

        readonly ToolButtonService _toolButtonService;

        // This is a bad way of doing things, actually. But if that's what you need, then it's a good way.
        static readonly ReflectedField<ToolButtonService, List<ToolGroupButton>>
                // ReSharper disable once InconsistentNaming
                ToolButtonServiceTool_GroupButtons_Field = new("_toolGroupButtons");
        static readonly ReflectedField<ToolGroupButton, ToolGroup>
                // ReSharper disable once InconsistentNaming
                ToolGroupButton_ToolGroup_Field = new("_toolGroup");

        public DraggableToolButtons(
            DraggableUtilsGroup draggableUtilsGroup,
            ToolButtonFactory toolButtonFactory,
            ToolGroupButtonFactory toolGroupButtonFactory,
            DraggableToolFactory draggableToolFactory,
            IAssetLoader assetLoader,
            HaulPrioritizeTool haulPrioritizeTool,
            ToolButtonService toolButtonService)
        {
            this._draggableUtilsGroup = draggableUtilsGroup;
            this._toolButtonFactory = toolButtonFactory;
            this._toolGroupButtonFactory = toolGroupButtonFactory;
            this._draggableToolFactory = draggableToolFactory;
            this._assetLoader = assetLoader;
            this._haulPrioritizeTool = haulPrioritizeTool;
            _toolButtonService = toolButtonService;
        }
        
        
        public BottomBarElement GetElement() {
            // Attempt to inject the new controls into the stock group first. 
            if (ToolButtonServiceTool_GroupButtons_Field.IsValid() && ToolGroupButton_ToolGroup_Field.IsValid()) {
                var stockGroups = ToolButtonServiceTool_GroupButtons_Field.Get(_toolButtonService);
                foreach (var stockGroup in stockGroups) {
                    var toolGroup = ToolGroupButton_ToolGroup_Field.Get(stockGroup);
                    if (toolGroup is BuilderPriorityToolGroup) {
                        Debug.Log("Attaching DraggableUtils buttons to the stock priority tool group");
                        AddButtonsToGroup(stockGroup);
                        return BottomBarElement.CreateMultiLevel(stockGroup.Root, stockGroup.ToolButtonsElement);
                    }
                }
            }

            // No luck, but we still can create our own tool group button.
            Debug.LogWarningFormat(
                    "Cannot add draggable tool buttons into the stock priority group. Using own button.");
            _draggableUtilsGroup.LoadIcon(_assetLoader);
            var blue = _toolGroupButtonFactory.CreateBlue(_draggableUtilsGroup);
            AddButtonsToGroup(blue);
            return BottomBarElement.CreateMultiLevel(blue.Root, blue.ToolButtonsElement);
        }

        void AddButtonsToGroup(ToolGroupButton group) {
            //Pause buildings
            AddTool((Tool) this._draggableToolFactory.CreatePauseTool(), _assetLoader.Load<Sprite>("DraggableUtils/UI_Buttons/pause_button"), group);

            //Prioritize haulers
            AddTool(this._draggableToolFactory.CreateHaulPrioritizeTool(), _assetLoader.Load<Sprite>("DraggableUtils/UI_Buttons/hauler_button"), group);

            //Empty storage
            AddTool(this._draggableToolFactory.CreateEmptyStorageTool(), _assetLoader.Load<Sprite>("DraggableUtils/UI_Buttons/storage_empty_button"), group);
        }

        private void AddTool(Tool tool, Sprite sprite, ToolGroupButton toolGroupButton)
        {
            ToolButton button = this._toolButtonFactory.Create(tool, sprite, toolGroupButton.ToolButtonsElement);
            toolGroupButton.AddTool(button);
        }
    }
}
