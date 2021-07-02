using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[System.Serializable]
public class BarnUpgrade {
    public BarnUpgrades.Upgrade upgrade;

    public GameObject barnPrefab;

    [Tooltip("The cost to upgrade your barn to this upgrade.")]
    public float costForThis;
}

/// <summary>
/// This scriptable object stores metadata of all the available barn upgrades, including
/// the default barn.
/// <summary>
[CreateAssetMenu]
[System.Serializable]
public class BarnUpgrades : CustomScriptableObject {
    public enum Upgrade {
        Small,
        Medium,
        Large,
        XLarge,
    }

    [SerializeField]
    private List<BarnUpgrade> upgrades;

    public void Initialize() {
        int i = 0;
        // Ensure all Upgrade correspond to a unique BarnUpgrade
        foreach (Upgrade upgrade in (Upgrade[]) Enum.GetValues(typeof(Upgrade))) {
            Assert.IsTrue(upgrades[i].upgrade == upgrade);
            i++;
        }
        Assert.IsTrue(upgrades.Count == NumUpgrades());
    }

    public BarnUpgrade Get(Upgrade upgrade) {
        return upgrades.Find(x => x.upgrade == upgrade);
    }

    public static int NumUpgrades() {
        return Enum.GetNames(typeof(Upgrade)).Length;
    }

    public float CostToUpgrade(Upgrade upgrade) {
        int enumIdx = (int) upgrade;
        if (enumIdx + 1 < NumUpgrades()) {
            return upgrades[enumIdx + 1].costForThis;
        }
        return float.PositiveInfinity;
    }
}
