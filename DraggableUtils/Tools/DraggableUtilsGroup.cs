using TimberApi.AssetSystem;
using Timberborn.ConstructionMode;
using Timberborn.ToolSystem;
using UnityEngine;

namespace DraggableUtils.Tools
{
    public class DraggableUtilsGroup : ToolGroup, IConstructionModeEnabler {
        
        public DraggableUtilsGroup(IAssetLoader assetLoader)
        {
            // FIXME(ihsoft): Don't construct before asset is loaded!
            Debug.LogWarning("******* ASSET MISSING!");
            //this.Icon = assetLoader.Load<Sprite>("DraggableUtils/UI_Buttons/group_button");
        }
        
        public override string IconName => "BeaverGeneratorTool";

        public override string DisplayNameLocKey => "Kyp.ToolGroups.DraggableUtils";
    }
}