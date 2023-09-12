using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSlot : MonoBehaviour {
    public abstract bool IsSameSlot(BaseSlot other);
}
