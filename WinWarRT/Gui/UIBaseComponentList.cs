using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui
{
    public class UIBaseComponentList
    {
        private List<UIBaseComponent> components;

        public int Count
        {
            get
            {
                return components.Count;
            }
        }

        public UIBaseComponent this[int index]
        {
            get
            {
                return components[index];
            }
            set
            {
                components[index] = value;
            }
        }

        public UIBaseComponentList(List<UIBaseComponent> setComponents)
        {
            components = setComponents;
        }
    }
}
