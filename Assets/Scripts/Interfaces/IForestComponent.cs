using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ForestComponent {

    public enum ComponentType {
        Default,
        Tree,
        Mushroom,
        Sunflower,
        Decomposer
    }

    public interface IForestComponent {
        float maintenanceCost { get; set; }
        float energyBuildCost { get; set; }
        float waterBuildCost { get; set; }
        float organicBuildCost { get; set; }
        float health { get; set; }
        float maxHealth { get; set; }
    }
}


