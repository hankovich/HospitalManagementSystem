namespace Hms.UI.Infrastructure.Controls.UpdatableContentControl
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class UpdatableContentControl : ContentControl
    {
        public UpdatableContentControl()
        {
            this.ContentTemplateSelector = new UpdatableDataTemplateSelector();
        }

        class UpdatableDataTemplateSelector : DataTemplateSelector
        {
            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                var declaredDataTemplate = FindDeclaredDataTemplate(item, container);
                var wrappedDataTemplate = WrapDataTemplate(declaredDataTemplate);
                return wrappedDataTemplate;
            }

            private static DataTemplate WrapDataTemplate(DataTemplate declaredDataTemplate)
            {
                var frameworkElementFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                frameworkElementFactory.SetValue(ContentPresenter.ContentTemplateProperty, declaredDataTemplate);
                var dataTemplate = new DataTemplate { VisualTree = frameworkElementFactory };
                return dataTemplate;
            }

            private static DataTemplate FindDeclaredDataTemplate(object item, DependencyObject container)
            {
                if (item == null)
                {
                    return null;
                }

                var dataTemplateKey = new DataTemplateKey(item.GetType());
                var dataTemplate = ((FrameworkElement)container).FindResource(dataTemplateKey) as DataTemplate;
                if (dataTemplate == null)
                {
                    throw new Exception("datatemplate not found");
                }

                return dataTemplate;
            }
        }
    }
}
