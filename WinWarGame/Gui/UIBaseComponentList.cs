using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarGame.Gui
{
    internal class UIBaseComponentList
    {
        private List<UIBaseComponent> components;

        internal int Count
        {
            get
            {
                return components.Count;
            }
        }

        internal UIBaseComponent this[int index]
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

        internal UIBaseComponentList(List<UIBaseComponent> setComponents)
        {
            components = setComponents;
        }
    }
}
