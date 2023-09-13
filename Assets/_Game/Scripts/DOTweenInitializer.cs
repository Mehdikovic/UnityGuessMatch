using DG.Tweening;
using UnityEngine;

public class DOTweenInitializer : MonoBehaviour {

    [SerializeField] private bool recycleAllByDefault = true;
    [SerializeField] private bool useSafeMode = true;

    private void Awake() {
        DOTween.Init(recycleAllByDefault, useSafeMode);
    }
}
