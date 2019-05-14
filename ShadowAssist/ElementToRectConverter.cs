using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace ShadowAssist
{
    class ShadowCutOutConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is FrameworkElement element && values[1] is ShadowDepth shadowDepth)
            {
                var dropShadowEffect = (DropShadowEffect)ShadowConverter.Instance.Convert(shadowDepth, null, null, null);
                if (dropShadowEffect == null) return Binding.DoNothing;
                var blurRadius = dropShadowEffect.BlurRadius;
                return new Rect(-blurRadius, -blurRadius, element.ActualWidth + blurRadius*2, element.ActualHeight + blurRadius*2);
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
