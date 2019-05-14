using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ShadowAssist
{
    public class ShadowAdorner : Adorner
    {
        public Shadow Shadow { get; }

        public ShadowAdorner(UIElement adornedElement) : base(adornedElement)
        {
            Shadow = new Shadow();
            AddVisualChild(Shadow);
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0) throw new ArgumentOutOfRangeException();
            return Shadow;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Shadow.Measure(constraint);
            return new Size(AdornedElement.RenderSize.Width, AdornedElement.RenderSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Shadow.Arrange(new Rect(new Point(0, 0), new Size(finalSize.Width, finalSize.Height)));
            return new Size(AdornedElement.RenderSize.Width, AdornedElement.RenderSize.Height);
        }
    }
}
