namespace Hms.UI.Infrastructure.Controls
{
    using System.Windows;
    using System.Windows.Data;

    public class BindingEvaluator : FrameworkElement
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(BindingEvaluator),
            new FrameworkPropertyMetadata(string.Empty));

        public BindingEvaluator(Binding binding)
        {
            this.ValueBinding = binding;
        }

        public string Value
        {
            get
            {
                return (string)this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        public Binding ValueBinding { get; set; }

        public string Evaluate(object dataItem)
        {
            this.DataContext = dataItem;
            this.SetBinding(ValueProperty, this.ValueBinding);
            return this.Value;
        }
    }
}