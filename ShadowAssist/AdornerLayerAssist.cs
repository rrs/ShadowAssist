using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace ShadowAssist
{
    public static class AdornerLayerAssist
    {
        public static DependencyProperty FrameworkElementAdornerInfoProperty = DependencyProperty.RegisterAttached("FrameworkElementAdornerInfo", typeof(FrameworkElementAdornerInfo), typeof(AdornerLayerAssist));

        public static FrameworkElementAdornerInfo GetFrameworkElementAdornerInfo(DependencyObject obj)
        {
            return (FrameworkElementAdornerInfo)obj.GetValue(FrameworkElementAdornerInfoProperty);
        }

        public static void SetFrameworkElementAdornerInfo(DependencyObject obj, FrameworkElementAdornerInfo value)
        {
            obj.SetValue(FrameworkElementAdornerInfoProperty, value);
        }

        public static DependencyProperty ElevateToAdornerLayerProperty = DependencyProperty.RegisterAttached("ElevateToAdornerLayer", typeof(bool), typeof(AdornerLayerAssist),
           new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ElevateToAdornerLayerPropertyChangedCallback)));

        public static bool GetElevateToAdornerLayer(DependencyObject obj)
        {
            return (bool)obj.GetValue(ElevateToAdornerLayerProperty);
        }

        public static void SetElevateToAdornerLayer(DependencyObject obj, bool value)
        {
            obj.SetValue(ElevateToAdornerLayerProperty, value);
        }

        private static void ElevateToAdornerLayerPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue) return;
            UpdateAdornerLayer((FrameworkElement)d, (bool)e.NewValue);
        }

        private static void UpdateAdornerLayer(FrameworkElement element, bool elevate, bool loaded = false)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(element);

            if (adornerLayer == null)
            {
                if (!loaded)
                    element.Dispatcher.InvokeAsync(() => UpdateAdornerLayer(element, elevate, true), DispatcherPriority.Loaded);

                return;
            }

            var info = GetFrameworkElementAdornerInfo(element);

            if (elevate)
            {
                if (info == null)
                {
                    var parent = VisualTreeHelper.GetParent(element) as Panel;
                    if (parent == null) return;
                    var index = parent.Children.IndexOf(element);
                    parent.Children.RemoveAt(index);
                    parent.Children.Insert(index, new Control());
                    var frameworkElementAdorner = new FrameworkElementAdorner(element);
                    frameworkElementAdorner.Child = element;

                    info = new FrameworkElementAdornerInfo
                    {
                        Parent = parent,
                        Index = index,
                        Adorner = frameworkElementAdorner
                    };

                    adornerLayer.Add(frameworkElementAdorner);
                    SetFrameworkElementAdornerInfo(element, info);
                }
            }
            else
            {
                if (info != null)
                {
                    info.Adorner.Child = null;
                    SetFrameworkElementAdornerInfo(element, null);
                    adornerLayer.Remove(info.Adorner);
                    info.Parent.Children.RemoveAt(info.Index);
                    info.Parent.Children.Insert(info.Index, element);
                }
            }
        }
    }
}
