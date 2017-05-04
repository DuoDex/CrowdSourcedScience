/*
 * ScienceFixer
 * 
 * This code is courtesy of Sigma88 and is used with permission.
 * Copyright © 2017, Sigma88. All rights reserved.
 */

using UnityEngine;

namespace ScienceFixer
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class ScienceFixer : MonoBehaviour
    {
        public void Start()
        {
            foreach (ConfigNode config in GameDatabase.Instance.GetConfigNodes("EXPERIMENT_DEFINITION"))
            {
                // Safety check
                if (!config.HasNode("RESULTS"))
                    return;

                ConfigNode results = config.GetNode("RESULTS");
                ConfigNode data = new ConfigNode();
                foreach (ConfigNode.Value key in results.values)
                    data.AddValue(key.name + (key.name != "default" ? "*" : ""), key.value);

                results.ClearData();
                results.AddData(data);
            }
        }
    }
}
