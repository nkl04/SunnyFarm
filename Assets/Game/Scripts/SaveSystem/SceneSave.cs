using System;
using System.Collections.Generic;

[Serializable]
public class SceneSave
{
    public Dictionary<string, List<SceneItem>> listSceneItemDictionary;

    public Dictionary<string, GridPropertiesDetail> gridPropertiesDetailDictionary;
}
