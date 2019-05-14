using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ShadowAssist
{
    public class FrameworkElementAdornerInfo
    {
        public Panel Parent { get; set; }
        public int Index { get; set; }
        public FrameworkElementAdorner Adorner { get; set; }
    }
}
