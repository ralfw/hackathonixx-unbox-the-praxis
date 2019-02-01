using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace unbox.frontend.controls
{
    public class RelationPanel : Panel
    {
        /// <summary>
        /// Get-Accessor für das Attached Property "TopRelation". Details siehe <see cref="TopRelationProperty"/>.
        /// </summary>
        /// <param name="obj">Objekt, bei dem das Property abgerufen werden soll.</param>
        /// <returns>Wert des Properties.</returns>
        public static double GetTopRelation(DependencyObject obj)
        {
            return (double)obj.GetValue(TopRelationProperty);
        }

        /// <summary>
        /// Set-Accessor für das Attached Property "TopRelation". Details siehe <see cref="TopRelationProperty"/>.
        /// </summary>
        /// <param name="obj">Objekt, bei dem das Property gesetzt werden soll.</param>
        /// <param name="value">Gewünschter Wert des Properties.</param>
        public static void SetTopRelation(DependencyObject obj, double value)
        {
            obj.SetValue(TopRelationProperty, value);
        }

        public static readonly DependencyProperty TopRelationProperty =
            DependencyProperty.RegisterAttached("TopRelation",
                typeof(double),
                typeof(RelationPanel),
                new PropertyMetadata(0.0d));


        /// <summary>
        /// Get-Accessor für das Attached Property "BottomRelation". Details siehe <see cref="BottomRelationProperty"/>.
        /// </summary>
        /// <param name="obj">Objekt, bei dem das Property abgerufen werden soll.</param>
        /// <returns>Wert des Properties.</returns>
        public static double GetBottomRelation(DependencyObject obj)
        {
            return (double)obj.GetValue(BottomRelationProperty);
        }

        /// <summary>
        /// Set-Accessor für das Attached Property "BottomRelation". Details siehe <see cref="BottomRelationProperty"/>.
        /// </summary>
        /// <param name="obj">Objekt, bei dem das Property gesetzt werden soll.</param>
        /// <param name="value">Gewünschter Wert des Properties.</param>
        public static void SetBottomRelation(DependencyObject obj, double value)
        {
            obj.SetValue(BottomRelationProperty, value);
        }

        public static readonly DependencyProperty BottomRelationProperty =
            DependencyProperty.RegisterAttached("BottomRelation",
                typeof(double),
                typeof(RelationPanel),
                new PropertyMetadata(1.0d));


        protected override Size MeasureOverride(Size availableSize)
        {
            var neededWidth = 0.0d;
            foreach (UIElement child in InternalChildren)
            {
                var topRelation = GetTopRelation(child);
                var bottomRelation = GetBottomRelation(child);
                if (bottomRelation != topRelation)
                {
                    var height = Math.Round((bottomRelation - topRelation) * availableSize.Height);
                    child.Measure(new Size(availableSize.Width, height));
                    neededWidth = Math.Max(neededWidth, child.DesiredSize.Width);
                }
            }
            return new Size(neededWidth, double.IsInfinity(availableSize.Height) ? 0 : availableSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                var topRelation = GetTopRelation(child);
                var bottomRelation = GetBottomRelation(child);
                var height = Math.Round((bottomRelation - topRelation) * finalSize.Height);
                var top = Math.Round(topRelation * finalSize.Height);
                child.Arrange(new Rect(0, top, finalSize.Width, height));
            }
            return finalSize;
        }
    }
}
