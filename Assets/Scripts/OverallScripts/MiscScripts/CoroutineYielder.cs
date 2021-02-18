using UnityEngine;


namespace SzymonPeszek.Misc
{
    /// <summary>
    /// Class for storing coroutines WaitForSecond enumerators for less cache garbage creation
    /// </summary>
    public static class CoroutineYielder
    {
        public static WaitForFixedUpdate fixedUpdateWait { get; } = new WaitForFixedUpdate();
        public static WaitForEndOfFrame endOffFrameWaiter { get; } = new WaitForEndOfFrame();

        public static WaitForSeconds stepSoundStopWaiter { get; } = new WaitForSeconds(1f);
        public static WaitForSeconds stepSoundEnablerWaiter { get; } = new WaitForSeconds(1f);
        public static WaitForSeconds bonfireLitWaiter { get; } = new WaitForSeconds(2f);
        public static WaitForSeconds bonfireTeleportFirstWaiter { get; } = new WaitForSeconds(5f);
        public static WaitForSeconds bonfireTeleportSecondWaiter { get; } = new WaitForSeconds(1.5f);
        public static WaitForSeconds areaNameWaiter { get; } = new WaitForSeconds(1.5f);
        public static WaitForSeconds bossHealWaiter { get; } = new WaitForSeconds(1f);
        public static WaitForSeconds bossPositionResetWaiter { get; } = new WaitForSeconds(5f);
        public static WaitForSeconds spawnRefreshWaiter { get; } = new WaitForSeconds(1f);
        public static WaitForSeconds fogWallDestroyWaiter { get; } = new WaitForSeconds(2f);
        public static WaitForSeconds fogWallRemoveFirstWaiter { get; } = new WaitForSeconds(1f);
        public static WaitForSeconds fogWallRemoveSecondWaiter { get; } = new WaitForSeconds(2.5f);
        public static WaitForSeconds playerRespawnWaiter { get; } = new WaitForSeconds(5f);
        public static WaitForSeconds enemyHpUpdateWaiter { get; } = new WaitForSeconds(3.5f);
        public static WaitForSeconds jumpFirstWaiter { get; } = new WaitForSeconds(0.2f);
        public static WaitForSeconds jumpSecondWaiter { get; } = new WaitForSeconds(0.5f);
        public static WaitForSeconds disolveAfterBackStabWaiter { get; } = new WaitForSeconds(1.5f);
        public static WaitForSeconds lesserBuffWaiter { get; } = new WaitForSeconds(20f);
        public static WaitForSeconds mediumBuffWaiter { get; } = new WaitForSeconds(40f);
        public static WaitForSeconds grandBuffWaiter { get; } = new WaitForSeconds(60f);
    }
}