using TimberApi.AssetSystem;
using Timberborn.ConstructionMode;
using Timberborn.ToolSystem;
using UnityEngine;

namespace DraggableUtils.Tools
{
    public class DraggableUtilsGroup : ToolGroup, IConstructionModeEnabler {
        public override string IconName => "BeaverGeneratorTool";

        public override string DisplayNameLocKey => "Kyp.ToolGroups.DraggableUtils";

        /// <summary>Load button's icon from assets.</summary>
        /// <remarks>
        /// Icon asset may not be loaded at the time of the tool group construction. So, postpone it till the very
        /// moment when the visual element is created.
        /// </remarks>
        public void LoadIcon(IAssetLoader assetLoader) {
            Icon = assetLoader.Load<Sprite>("DraggableUtils/UI_Buttons/group_button");
        }
    }
}