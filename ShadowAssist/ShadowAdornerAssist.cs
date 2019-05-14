using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace ShadowAssist
{
    public static class ShadowAdornerAssist
    {
        public static DependencyProperty ShadowDepthProperty = DependencyProperty.RegisterAttached("ShadowDepth", typeof(ShadowDepth), typeof(ShadowAdornerAssist),
            new FrameworkPropertyMetadata(default(ShadowDepth), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ShadowDepthProprtyChangedCallback)));
        
        public static ShadowDepth GetShadowDepth(DependencyObject obj)
        {
            return (ShadowDepth)obj.GetValue(ShadowDepthProperty);
        }

        public static void SetShadowDepth(DependencyObject obj, ShadowDepth value)
        {
            obj.SetValue(ShadowDepthProperty, value);
        }

        private static void ShadowDepthProprtyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateShadowDepth((UIElement)d, (ShadowDepth)e.NewValue);
        }

        public static DependencyProperty ShadowAdornerProperty = DependencyProperty.RegisterAttached("ShadowAdorner", typeof(ShadowAdorner), typeof(ShadowAdornerAssist));

        public static ShadowAdorner GetShadowAdorner(DependencyObject obj)
        {
            return (ShadowAdorner)obj.GetValue(ShadowAdornerProperty);
        }

        public static void SetShadowAdorner(DependencyObject obj, ShadowAdorner value)
        {
            obj.SetValue(ShadowAdornerProperty, value);
        }

        private static void UpdateShadowDepth(UIElement element, ShadowDepth shadowDepth, bool loaded = false)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(element);

            if (adornerLayer == null)
            {
                if (!loaded)
                    element.Dispatcher.InvokeAsync(() => UpdateShadowDepth(element, shadowDepth, true), DispatcherPriority.Loaded);

                return;
            }

            var topMostAdornerLayer = adornerLayer;
            //while (topMostAdornerLayer != null)
            //{
            //    topMostAdornerLayer = AdornerLayer.GetAdornerLayer(topMostAdornerLayer);
            //}

            var shadowAdorner = GetShadowAdorner(element);

            if (shadowDepth == ShadowDepth.Depth0)
            {
                if (shadowAdorner != null)
                {
                    SetShadowAdorner(element, null);
                    topMostAdornerLayer.Remove(shadowAdorner);
                }
            }
            else
            {
                if (shadowAdorner == null)
                {
                    shadowAdorner = new ShadowAdorner(element);
                    MaterialDesignThemes.Wpf.ShadowAssist.SetShadowDepth(shadowAdorner.Shadow, shadowDepth);
                    topMostAdornerLayer.Add(shadowAdorner);
                    SetShadowAdorner(element, shadowAdorner);
                }
                else
                {
                    MaterialDesignThemes.Wpf.ShadowAssist.SetShadowDepth(shadowAdorner.Shadow, shadowDepth);
                }
            }
        }
    }
}
