using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WPFlipping
{
    [TemplatePart(Name = PlaceHolderPartName, Type = typeof(Grid))]
    [TemplatePart(Name = OverPartName, Type = typeof(Grid))]
    [TemplatePart(Name = UnderPartName, Type = typeof(Grid))]
    public class DoubleSidesControl : Control
    {
        #region Fields
        /// <summary>
        /// Plane Projection of PlaceHolder
        /// </summary>
        private PlaneProjection AnimatedPlaneProjection;
        #endregion

        #region Template Part names
        //Template Part Names
        private const string PlaceHolderPartName = "PlaceHolder";
        private const string OverPartName = "Over";
        private const string UnderPartName = "Under";
        private const string AnimatedPlaneProjectionPartName = "AnimatedPlaneProjection";
        private const string FrontTemplatePartName = "FrontTemplate";
        private const string BackTemplatePartName = "BackTemplate";

        #endregion

        #region Template Parts

        /// <summary>
        /// PlaceHolder Template part
        /// </summary>
        private Grid PlaceHolder;

        /// <summary>
        /// Over Template part
        /// </summary>
        private Grid Over;

        /// <summary>
        /// Under Template part
        /// </summary>
        private Grid Under;

        #endregion

        #region FrontTemplate Dependency Object
        public DataTemplate FrontTemplate
        {
            get { return (DataTemplate)GetValue(FrontTemplateProperty); }
            set { SetValue(FrontTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrontTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrontTemplateProperty =
            DependencyProperty.Register("FrontTemplate", typeof(DataTemplate), typeof(DoubleSidesControl), new PropertyMetadata(null, OnFrontTemplateChanged));

        private static void OnFrontTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region BackTemplate Dependency Object
        public DataTemplate BackTemplate
        {
            get { return (DataTemplate)GetValue(BackTemplateProperty); }
            set { SetValue(BackTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackTemplateProperty =
            DependencyProperty.Register("BackTemplate", typeof(DataTemplate), typeof(DoubleSidesControl), new PropertyMetadata(null, OnBackTemplateChanged));

        private static void OnBackTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region Initialization
        public DoubleSidesControl()
        {
            //set the default style defined in Themes/Generic.xaml
            DefaultStyleKey = typeof(DoubleSidesControl);
        }

        /// <summary>
        /// Retrieve template parts ( Over, Under and Place holder) and AnimatedPlaneProjection
        /// Register for manipulation events of PlaceHolder 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PlaceHolder = base.GetTemplateChild(PlaceHolderPartName) as Grid;
            Over = base.GetTemplateChild(OverPartName) as Grid;
            Under = base.GetTemplateChild(UnderPartName) as Grid;
            AnimatedPlaneProjection = base.GetTemplateChild(AnimatedPlaneProjectionPartName) as PlaneProjection;

            if (PlaceHolder != null)
            {
                PlaceHolder.ManipulationStarted += PlaceHolder_ManipulationStarted;
                PlaceHolder.ManipulationDelta += PlaceHolder_ManipulationDelta;
                PlaceHolder.ManipulationCompleted += PlaceHolder_ManipulationCompleted;
            }


            if (DesignerProperties.IsInDesignTool)
            {
                //Apply it only in DesignTime
                Under.Visibility = System.Windows.Visibility.Collapsed;
                Over.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                //Apply It only in runtime, to make it easy to design Back Template in Design Time(not Flipped)
                if (Under != null)
                {
                    if (Under.Projection != null && Under.Projection is PlaneProjection)
                    {
                        var underProjection = Under.Projection as PlaneProjection;
                        underProjection.RotationY = 180;
                    }
                    Under.Opacity = 0;
                }
            }

        }
        #endregion

        #region Flipping Logic
        private void PlaceHolder_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            // e.Handled = true;
            e.ManipulationContainer = sender as UIElement;
            startRotation = AnimatedPlaneProjection.RotationY;
        }

        private void PlaceHolder_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //if (Math.Abs(e.CumulativeManipulation.Translation.Y) > 5) return;
            //if (Math.Abs(e.DeltaManipulation.Translation.X) < 2) return;
            var container = sender as FrameworkElement;
            double earlyFlipFactor = container.ActualWidth / 3;// to make it faster to reach the middle
            double desiredFlipWidth = container.ActualWidth - earlyFlipFactor;
            double cumulative = e.CumulativeManipulation.Translation.X;
            double rotateValue = (cumulative / desiredFlipWidth) * -180;
            if (Math.Abs(e.CumulativeManipulation.Translation.X) < 2)
            {
                //If user is clicking
                return;
            }
            var PlaceHolderprojection = (PlaceHolder.Projection as PlaneProjection);
            double NextValue = (360 + startRotation + rotateValue) % 360;

            //Assign New Rotation Value
            PlaceHolderprojection.RotationY = NextValue;
            //Determine which part will be on top
            SetZIndex(NextValue);
            SetOpacity(NextValue);

        }

        /// <summary>
        /// set the ZIndex of Over and Under parts based on the current rotation value of PlaceHolder
        /// </summary>
        /// <param name="value">Current rotation value of PlaceHolder</param>
        private void SetZIndex(double value)
        {
            //Over Condition is true when Over Part should be on top, False if Under Part should be on top
            bool OverCondition = (value >= 0 && value <= 90) || (value >= 270 && value <= 360);
            Canvas.SetZIndex(Over, OverCondition ? 1 : 0);
            Canvas.SetZIndex(Under, OverCondition ? 0 : 1);
        }

        /// <summary>
        /// set the opacity of Over and Under parts based on the current rotation value of PlaceHolder
        /// </summary>
        /// <param name="value">Current rotation value of PlaceHolder</param>
        private void SetOpacity(double value)
        {
            //Over Condition is true when Over Part should be on top, False if Under Part should be on top
            bool OverCondition = (value >= 0 && value <= 90) || (value >= 270 && value <= 360);
            Over.Opacity = OverCondition ? 1 : 0;
            Under.Opacity = OverCondition ? 0 : 1;
        }

        /// <summary>
        /// Indicated that User releases the PlaceHolder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaceHolder_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (Math.Abs(e.TotalManipulation.Translation.X) < 2)
            {
                //If user is clicking
                return;
            }
            // Continue the flipping animation 
            ContinueSwipeFlip();
        }

        /// <summary>
        /// Used to hold the initial rotation value of placeholder before the user starts to flip it
        /// </summary>
        double startRotation = 0;

        /// <summary>
        /// After you release the place holder and no longer flipping it, 
        /// This method will fire Animation to continue the flipping to the nearest  side
        /// </summary>
        private void ContinueSwipeFlip()
        {
            double LastValue = AnimatedPlaneProjection.RotationY;
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = LastValue;
            double FullTime = 400;
            if (LastValue < 90 && LastValue >= 0)
            {
                doubleAnimation.To = 0;
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(FullTime * (LastValue) * 1e-2);
            }
            else if (LastValue >= 90 && LastValue < 180)
            {
                doubleAnimation.To = 180;
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(FullTime * (180 - LastValue) * 1e-2);
            }
            else if (LastValue >= 180 && LastValue < 270)
            {
                doubleAnimation.To = 180;
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(FullTime * (LastValue - 180) * 1e-2);
            }
            else
            {
                doubleAnimation.To = 360;
                doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(FullTime * (360 - LastValue) * 1e-2));
            }
            SetZIndex(doubleAnimation.To.Value);
            SetOpacity(doubleAnimation.To.Value);
            doubleAnimation.AutoReverse = false;
            Storyboard.SetTarget(doubleAnimation, AnimatedPlaneProjection);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RotationY"));
            Storyboard sb = new Storyboard();
            sb.Children.Add(doubleAnimation);
            sb.Begin();
        }
        #endregion
    }
}
