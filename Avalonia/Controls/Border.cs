// -----------------------------------------------------------------------
// <copyright file="Border.cs" company="Steven Kirk">
// Copyright 2013 MIT Licence
// See licence.md for more information
// </copyright>
// -----------------------------------------------------------------------

namespace Avalonia.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Avalonia.Media;

    public class Border : Decorator
    {
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(
                "Background", 
                typeof(Brush), 
                typeof(Border),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Colors.White),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(Border),
                new FrameworkPropertyMetadata(
                    new CornerRadius(),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(
                "Padding",
                typeof(Thickness),
                typeof(Border),
                new FrameworkPropertyMetadata(
                    new Thickness(),
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        public CornerRadius CornerRadius 
        {
            get { return (CornerRadius)this.GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        public Thickness Padding
        {
            get { return (Thickness)this.GetValue(PaddingProperty); }
            set { this.SetValue(PaddingProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (this.Child != null)
            {
                Rect rect = Deflate(new Rect(new Point(), constraint), this.Padding);
                this.Child.Measure(constraint);
                return this.Child.DesiredSize;
            }
            else
            {
                return base.MeasureOverride(constraint);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.Child != null)
            {
                Rect rect = Deflate(new Rect(new Point(), finalSize), this.Padding);
                this.Child.Arrange(rect);
            }

            return finalSize;
        }

        protected internal override void OnRender(DrawingContext drawingContext)
        {
            Rect rect = new Rect(new Point(), new Size(this.ActualWidth, this.ActualHeight));

            if (this.CornerRadius.TopLeft > 0 || this.CornerRadius.BottomLeft > 0)
            {
                drawingContext.DrawRoundedRectangle(
                    this.Background,
                    null,
                    rect,
                    this.CornerRadius.TopLeft,
                    this.CornerRadius.BottomLeft);
            }
            else
            {
                drawingContext.DrawRectangle(
                    this.Background,
                    null,
                    rect);
            }
        }

        private static Rect Deflate(Rect rect, Thickness thickness)
        {
            return new Rect(
                rect.Left + thickness.Left,
                rect.Top + thickness.Top,
                Math.Max(0.0, rect.Width - thickness.Left - thickness.Right),
                Math.Max(0.0, rect.Height - thickness.Top - thickness.Bottom));
        }
    }
}
