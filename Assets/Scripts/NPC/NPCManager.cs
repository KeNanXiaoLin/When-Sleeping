using System.Collections;
using System.Collections.Generic;

public class NPCManager:BaseManager<NPCManager>
{

    private Dictionary<int, NPCData> npcDics = new();

    /// <summary>
    /// 私有构造
    /// </summary>
    private NPCManager()
    {

    }

    public void AddNpcData(NPCData data)
    {
        if (!npcDics.ContainsKey(data.id))
        {
            npcDics.Add(data.id, data);
        }
    }

    public void RemoveNpcData(NPCData data)
    {
        if (npcDics.ContainsKey(data.id))
        {
            npcDics.Remove(data.id);
        }
    }

    public void Clear()
    {
        npcDics.Clear();
    }
}
