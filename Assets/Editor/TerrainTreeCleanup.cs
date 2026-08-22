using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// One-off maintenance tool: strips tree prototypes whose prefab reference is
// missing, plus every tree instance that pointed at them. Those instances were
// already invisible at runtime; Unity logged them as errors and the Android
// player build aborted on the error count.
public static class TerrainTreeCleanup
{
    [MenuItem("Build/Remove missing terrain trees")]
    public static void Run()
    {
        var guids = AssetDatabase.FindAssets("t:TerrainData");
        Debug.Log("[TerrainTreeCleanup] scanning " + guids.Length + " TerrainData asset(s).");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var data = AssetDatabase.LoadAssetAtPath<TerrainData>(path);
            if (data == null) continue;

            var prototypes = data.treePrototypes;
            var keep = new List<TreePrototype>();
            var remap = new int[prototypes.Length];

            for (int i = 0; i < prototypes.Length; i++)
            {
                if (prototypes[i] != null && prototypes[i].prefab != null)
                {
                    remap[i] = keep.Count;
                    keep.Add(prototypes[i]);
                }
                else
                {
                    remap[i] = -1;
                }
            }

            if (keep.Count == prototypes.Length)
            {
                Debug.Log("[TerrainTreeCleanup] " + path + ": all " + prototypes.Length + " prototype(s) valid, left alone.");
                continue;
            }

            var instances = data.treeInstances;
            var kept = new List<TreeInstance>(instances.Length);
            foreach (var inst in instances)
            {
                if (inst.prototypeIndex < 0 || inst.prototypeIndex >= remap.Length) continue;
                var mapped = remap[inst.prototypeIndex];
                if (mapped < 0) continue;
                var copy = inst;
                copy.prototypeIndex = mapped;
                kept.Add(copy);
            }

            data.treeInstances = new TreeInstance[0];
            data.treePrototypes = keep.ToArray();
            data.treeInstances = kept.ToArray();
            data.RefreshPrototypes();
            EditorUtility.SetDirty(data);

            Debug.Log("[TerrainTreeCleanup] " + path + ": removed "
                + (prototypes.Length - keep.Count) + " missing prototype(s) and "
                + (instances.Length - kept.Count) + " orphaned instance(s); "
                + keep.Count + " prototype(s) and " + kept.Count + " instance(s) kept.");
        }

        AssetDatabase.SaveAssets();
        Debug.Log("[TerrainTreeCleanup] done.");
    }
}
