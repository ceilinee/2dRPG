using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DescriptionSign is supposed to be added to instances of the Sign prefab.
/// It represents a read only sign that shows some text, and does nothing else.
/// </summary>
public class DescriptionSign : Sign {
    protected override void OnOpen() {
    }

    protected override void OnClose() { }
}
