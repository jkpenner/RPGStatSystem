using System;
using RPGSystems.Utility.Editor;

namespace RPGSystems.StatSystem.Editor {
    public class RPGVitalEditorExtension : IEditorExtension {
        public bool CanHandleType(Type type) {
            return typeof(RPGVitalAsset).IsAssignableFrom(type);
        }

        public void OnGUI(object asset) {

        }
    }
}
