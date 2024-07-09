using DG.Tweening;

namespace Game
{
    public static class TweenExtensions
    {
        public static void KillIfActiveAndPlaying(this Tween tween, bool complete = false)
        {
            if (tween != null && tween.IsActive() && tween.IsPlaying())
            {
                tween.Kill(complete);
            }
        }
    }

}