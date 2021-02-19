using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Weapon
{
    class Ammo
    {
        private int _556mm = 100;
        private int _762mm = 100;
        private int _9mm = 100;
        private int _858mm = 100;
        private int _50Cal = 100;
        private int rockets = 20;
        private int fuel = 150;

        private int fragGrenade = 3;
        private int smokeGrenade = 3;

        public Ammo() { }

        public int Amount(AmmoType type)
        {
            switch (type)
            {
                case AmmoType._556mm:
                    return _556mm;
                case AmmoType._762mm:
                    return _762mm;
                case AmmoType._9mm:
                    return _9mm;
                case AmmoType._858mm:
                    return _858mm;
                case AmmoType.Rocket:
                    return rockets;
                case AmmoType.Fuel:
                    return fuel;
                case AmmoType._50Cal:
                    return _50Cal;
                default:
                    return 0;
            }
        }

        public void SetAmount(AmmoType type, int value)
        {
            switch (type)
            {
                case AmmoType._556mm:
                    _556mm += value;
                    break;
                case AmmoType._762mm:
                    _762mm += value;
                    break;
                case AmmoType._9mm:
                    _9mm += value;
                    break;
                case AmmoType._858mm:
                    _858mm += value;
                    break;
                case AmmoType.Rocket:
                    rockets += value;
                    break;
                case AmmoType.Fuel:
                    fuel += value;
                    break;
                case AmmoType._50Cal:
                    _50Cal += value;
                    break;
                default:
                    break;
            }
        }

        public int Grenade(GrenadeType type)
        {
            if (type == GrenadeType.Frag)
                return fragGrenade;
            else if (type == GrenadeType.Smoke)
                return smokeGrenade;
            else
                return 0;
        }

        public void UpdateGrenade(GrenadeType type, int amount)
        {
            if (type == GrenadeType.Frag)
                fragGrenade += amount;
            else if (type == GrenadeType.Smoke)
                smokeGrenade += amount;
        }
    }
}
