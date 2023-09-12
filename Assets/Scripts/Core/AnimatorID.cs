using UnityEngine;

public static class AnimatorID {
    static public readonly int PlayerIdle = Animator.StringToHash(nameof(PlayerIdle));
    static public readonly int PlayerRun = Animator.StringToHash(nameof(PlayerRun));
    static public readonly int PlayerRunBackward = Animator.StringToHash(nameof(PlayerRunBackward));
    static public readonly int PlayerTurning = Animator.StringToHash(nameof(PlayerTurning));
}
