namespace Hms.UI.Infrastructure
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    public class ReloadOnDataContextChangedBehavior : Behavior<UserControl>
    {
        protected override void OnAttached()
        {
            AssociatedObject.DataContextChanged += (sender, args) =>
            {
                AssociatedObject.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            };
        }
    }
}
