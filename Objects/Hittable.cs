using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class that implements IHittable allows itself to be hit
public interface IHittable {
    // Invoking BeHit implies the object has been dealt a damage of `damage`
    void BeHit(float damage);
}
