using ResourceItem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData/LevelData")]
public class LevelDataSO : DatabaseItemSO {
    public int count = 8;
    public int columnCount = 4;
    public float width = 350;
    public float height = 350;
    public float xSpace = 50;
    public float ySpace = 50;

    [Header("Timer")]
    public TimeSegment timeToEnd;
}
