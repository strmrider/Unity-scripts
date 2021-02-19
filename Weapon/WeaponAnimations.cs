using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Weapon
{
    public enum WeaponAnimation { Run, Sight, Reload, Fire, Hide, Ready, Melee, SniperBolt }
    static class WeaponAnimations
    {
        public const string Run = "Run";
        public const string Sight = "Sight";
        public const string Reload = "Reload";
        public const string ReloadHalf = "ReloadHalf";
        public const string Fire = "Fire";
        public const string Hide = "Hide";
        public const string Ready = "Ready";
        public const string Melee = "Melee";
        public const string SniperBolt = "Bolt";
        public const string Pump = "Pump";
        public const string ReloadOnce = "ReloadOnce";
    }
}
