using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Weapon
{
    class GearVest
    {
        Ammo ammo = new Ammo();

        public GearVest()
        {

        }

        public Ammo Ammo
        {
            get { return ammo; }
        }
    }
}
