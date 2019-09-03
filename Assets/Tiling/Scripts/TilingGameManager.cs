using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TilingGameManager
{
    public delegate void TriggerCallBack(string triggerName, UnityTile tile, Collider collider);
    public static TilingGameManager Instance { get; } = new TilingGameManager();
    private Dictionary<string, List<TriggerCallBack>> triggerEvents = new Dictionary<string, List<TriggerCallBack>>();
    private TilingGameManager()
    {
    }
    public void RegisterNewTrigger(string triggerName)
    {
        if (!triggerEvents.ContainsKey(triggerName) )
            triggerEvents[triggerName] = new List<TriggerCallBack>();
    }
    public void SubscribeToTrigger(string triggerName, TriggerCallBack callBack)
    {
        triggerEvents[triggerName].Add(callBack);
    }
    public void PublishTrigger(string triggerName, UnityTile tile, Collider collider)
    {
        if(triggerEvents.ContainsKey(triggerName) && triggerEvents[triggerName] != null)
        {
            foreach(var trigger in triggerEvents[triggerName])
            {
                trigger(triggerName, tile, collider);
            }
        }
    }
}
