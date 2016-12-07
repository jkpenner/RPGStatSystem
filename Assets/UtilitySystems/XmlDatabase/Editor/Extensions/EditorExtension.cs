namespace UtilitySystems.XmlDatabase.Editor {
    public abstract class EditorExtension : IEditorExtension {
        public abstract bool CanHandleType(System.Type type);
        public abstract void OnGUI(object asset);

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
    }
}
