using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerToMouse.Source.GUI
{
    public abstract class ctmAction
    {
        public String Name { get; set; } = "";
        public String Description { get; set; } = "";
        public int Cooldown { get; set; } = 0;

        public abstract void Activate();
    }
}
