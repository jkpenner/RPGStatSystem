using System;
using UnityEngine;
using UtilitySystems.XmlDatabase;

public class EntityDatabase : AbstractXmlDatabase<EntityAsset> {
    static private EntityDatabase _instance = null;
    static public EntityDatabase Instance {
        get {
            if (_instance == null) {
                _instance = new EntityDatabase();
            }
            return _instance;
        }
    }

    public override string DatabaseName { get { return @"EntityDatabase.xml"; } }
    public override string DatabasePath { get { return @"Databases/Entity/"; } }

    public override EntityAsset CreateAssetOfType(string type) {
        if (type == typeof(EntityAsset).Name) {
            return new EntityAsset(this.GetNextHighestId());
        }
        return null;
    }
}
