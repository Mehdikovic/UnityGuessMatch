using UI;

public static class WindowUITransitionExtenstion {
    static public void TransitionTo(this WindowUI from, WindowUI to) {
        UIUtil.TransitionTo(from, to);
    }

    static public void TransitionShow(this WindowUI to) {
        UIUtil.TransitionShow(to);
    }

    static public void TransitionHide(this WindowUI from) {
        UIUtil.TransitionHide(from);
    }

    static public void ImmediateTo(this WindowUI from, WindowUI to) {
        UIUtil.ImmediateTo(from, to);
    }

    static public void ImmediateShow(this WindowUI to) {
        UIUtil.ImmediateShow(to);
    }

    static public void ImmediateHide(this WindowUI from) {
        UIUtil.ImmediateHide(from);
    }
}
