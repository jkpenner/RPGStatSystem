namespace UtilitySystems.XmlDatabase.Editor {
    public interface IEditorExtension {
        
        bool CanHandleType(System.Type type);
        void OnGUI(object asset);

        void OnEnable();
        void OnDisable();
    }
}