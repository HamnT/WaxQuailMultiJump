using BepInEx;
using RoR2;
using EntityStates;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using R2API.Utils;

namespace WaxQuailMultiJump
{
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
    [BepInPlugin("com.HamnT.WaxQuailMultiJump", "WaxQuailMultiJump", "1.0.2")]
    class WaxQuailMultiJump : BaseUnityPlugin
    {
        public void Awake()
        {
            IL.EntityStates.GenericCharacterMain.ProcessJump_bool += il => 
            {
                var c = new ILCursor(il);

                if (c.TryGotoNext(
                    x => x.MatchLdarg(0),
                    x => x.MatchCall<EntityState>("get_characterMotor"),
                    x => x.MatchLdfld<CharacterMotor>("jumpCount"),
                    x => x.MatchLdarg(0),
                    x => x.MatchCall<EntityState>("get_characterBody"),
                    x => x.MatchLdfld<CharacterBody>("baseJumpCount")))
                {
                    c.Index += 5;
                    c.Remove();
                    c.Emit<CharacterBody>(OpCodes.Callvirt, "get_maxJumpCount");
                }
                else
                {
                    Log.Error("Failed to apply wax quail multi jump patch");
                }
            };
        }
    }
}
