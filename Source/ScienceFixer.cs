/*
 * ScienceFixer
 * 
 * This code was written by Sigma88 for Crowd Sourced Science and is licensed under
 * the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
 * 
 * See <https://creativecommons.org/licenses/by-nc-sa/4.0/> for full details.
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
                {
                    if (!key.name.StartsWith("default") && !key.name.EndsWith('*'))
                        data.AddValue(key.name + "*", key.value);
                }
                results.ClearData();
                results.AddData(data);
            }
        }
    }
}
