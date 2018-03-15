namespace Hms.UI.Infrastructure.Controls.Editors
{
    using System;
    using System.Collections;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using Hms.UI.Infrastructure.Controls;

    [TemplatePart(Name = AutoCompleteTextBox.PartEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = AutoCompleteTextBox.PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = AutoCompleteTextBox.PartSelector, Type = typeof(Selector))]
    public class AutoCompleteTextBox : Control
    {

        #region "Fields"

        public const string PartEditor = "PART_Editor";

        public const string PartPopup = "PART_Popup";

        public const string PartSelector = "PART_Selector";

        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
            "Delay",
            typeof(int),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(200));

        public static readonly DependencyProperty DisplayMemberProperty = DependencyProperty.Register(
            "DisplayMember",
            typeof(string),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
            "IconPlacement",
            typeof(IconPlacement),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(IconPlacement.Left));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(object),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register(
            "IconVisibility",
            typeof(Visibility),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen",
            typeof(bool),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            "IsLoading",
            typeof(bool),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly",
            typeof(bool),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register(
            "ItemTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(AutoCompleteTextBox));

        public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.Register(
            "LoadingContent",
            typeof(object),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ProviderProperty = DependencyProperty.Register(
            "Provider",
            typeof(ISuggestionProvider),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(null, OnSelectedItemChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            "MaxLength",
            typeof(int),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(0));

        public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register(
            "CharacterCasing",
            typeof(CharacterCasing),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(CharacterCasing.Normal));

        public static readonly DependencyProperty MaxPopUpHeightProperty = DependencyProperty.Register(
            "MaxPopUpHeight",
            typeof(int),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(600));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark",
            typeof(string),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SuggestionBackgroundProperty = DependencyProperty.Register(
            "SuggestionBackground",
            typeof(Brush),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(Brushes.White));

        private BindingEvaluator _bindingEvaluator;

        private TextBox _editor;

        private DispatcherTimer _fetchTimer;

        private string _filter;

        private bool _isUpdatingText;

        private Selector _itemsSelector;

        private Popup _popup;

        private SelectionAdapter _selectionAdapter;

        private bool _selectionCancelled;

        private SuggestionsAdapter _suggestionsAdapter;


        #endregion

        #region "Constructors"

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AutoCompleteTextBox),
                new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        #endregion

        #region "Properties"


        public int MaxPopupHeight
        {
            get
            {
                return (int)this.GetValue(MaxPopUpHeightProperty);
            }
            set
            {
                this.SetValue(MaxPopUpHeightProperty, value);
            }
        }


        public BindingEvaluator BindingEvaluator
        {
            get
            {
                return this._bindingEvaluator;
            }
            set
            {
                this._bindingEvaluator = value;
            }
        }

        public CharacterCasing CharacterCasing
        {
            get
            {
                return (System.Windows.Controls.CharacterCasing)this.GetValue(CharacterCasingProperty);
            }
            set
            {
                this.SetValue(CharacterCasingProperty, value);
            }
        }

        public int MaxLength
        {
            get
            {
                return (int)this.GetValue(DelayProperty);
            }
            set
            {
                this.SetValue(MaxLengthProperty, value);
            }
        }

        public int Delay
        {
            get
            {
                return (int)this.GetValue(DelayProperty);
            }

            set
            {
                this.SetValue(DelayProperty, value);
            }
        }

        public string DisplayMember
        {
            get
            {
                return (string)this.GetValue(DisplayMemberProperty);
            }

            set
            {
                this.SetValue(DisplayMemberProperty, value);
            }
        }

        public TextBox Editor
        {
            get
            {
                return this._editor;
            }
            set
            {
                this._editor = value;
            }
        }

        public DispatcherTimer FetchTimer
        {
            get
            {
                return this._fetchTimer;
            }
            set
            {
                this._fetchTimer = value;
            }
        }

        public string Filter
        {
            get
            {
                return this._filter;
            }
            set
            {
                this._filter = value;
            }
        }

        public object Icon
        {
            get
            {
                return this.GetValue(IconProperty);
            }

            set
            {
                this.SetValue(IconProperty, value);
            }
        }

        public IconPlacement IconPlacement
        {
            get
            {
                return (IconPlacement)this.GetValue(IconPlacementProperty);
            }

            set
            {
                this.SetValue(IconPlacementProperty, value);
            }
        }

        public Visibility IconVisibility
        {
            get
            {
                return (Visibility)this.GetValue(IconVisibilityProperty);
            }

            set
            {
                this.SetValue(IconVisibilityProperty, value);
            }
        }

        public bool IsDropDownOpen
        {
            get
            {
                return (bool)this.GetValue(IsDropDownOpenProperty);
            }

            set
            {
                this.SetValue(IsDropDownOpenProperty, value);
            }
        }

        public bool IsLoading
        {
            get
            {
                return (bool)this.GetValue(IsLoadingProperty);
            }

            set
            {
                this.SetValue(IsLoadingProperty, value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool)this.GetValue(IsReadOnlyProperty);
            }

            set
            {
                this.SetValue(IsReadOnlyProperty, value);
            }
        }

        public Selector ItemsSelector
        {
            get
            {
                return this._itemsSelector;
            }
            set
            {
                this._itemsSelector = value;
            }
        }

        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ItemTemplateProperty);
            }

            set
            {
                this.SetValue(ItemTemplateProperty, value);
            }
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get
            {
                return ((DataTemplateSelector)(this.GetValue(AutoCompleteTextBox.ItemTemplateSelectorProperty)));
            }
            set
            {
                this.SetValue(AutoCompleteTextBox.ItemTemplateSelectorProperty, value);
            }
        }

        public object LoadingContent
        {
            get
            {
                return this.GetValue(LoadingContentProperty);
            }

            set
            {
                this.SetValue(LoadingContentProperty, value);
            }
        }

        public Popup Popup
        {
            get
            {
                return this._popup;
            }
            set
            {
                this._popup = value;
            }
        }

        public ISuggestionProvider Provider
        {
            get
            {
                return (ISuggestionProvider)this.GetValue(ProviderProperty);
            }

            set
            {
                this.SetValue(ProviderProperty, value);
            }
        }

        public object SelectedItem
        {
            get
            {
                return this.GetValue(SelectedItemProperty);
            }

            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        public SelectionAdapter SelectionAdapter
        {
            get
            {
                return this._selectionAdapter;
            }
            set
            {
                this._selectionAdapter = value;
            }
        }

        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }

            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        public string Watermark
        {
            get
            {
                return (string)this.GetValue(WatermarkProperty);
            }

            set
            {
                this.SetValue(WatermarkProperty, value);
            }
        }

        public Brush SuggestionBackground
        {
            get
            {
                return (Brush)this.GetValue(SuggestionBackgroundProperty);
            }

            set
            {
                this.SetValue(SuggestionBackgroundProperty, value);
            }
        }

        #endregion

        #region "Methods"

        public static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox act = null;
            act = d as AutoCompleteTextBox;
            if (act != null)
            {
                if (act.Editor != null & !act._isUpdatingText)
                {
                    act._isUpdatingText = true;
                    act.Editor.Text = act.BindingEvaluator.Evaluate(e.NewValue);
                    act._isUpdatingText = false;
                }
            }
        }

        private void ScrollToSelectedItem()
        {
            ListBox listBox = this.ItemsSelector as ListBox;
            if (listBox != null && listBox.SelectedItem != null) listBox.ScrollIntoView(listBox.SelectedItem);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.Editor = this.Template.FindName(PartEditor, this) as TextBox;
            this.Popup = this.Template.FindName(PartPopup, this) as Popup;
            this.ItemsSelector = this.Template.FindName(PartSelector, this) as Selector;
            this.BindingEvaluator = new BindingEvaluator(new Binding(this.DisplayMember));

            if (this.Editor != null)
            {
                this.Editor.TextChanged += this.OnEditorTextChanged;
                this.Editor.PreviewKeyDown += this.OnEditorKeyDown;
                this.Editor.LostFocus += this.OnEditorLostFocus;

                if (this.SelectedItem != null)
                {
                    this.Editor.Text = this.BindingEvaluator.Evaluate(this.SelectedItem);
                }

            }

            this.GotFocus += this.AutoCompleteTextBox_GotFocus;

            if (this.Popup != null)
            {
                this.Popup.StaysOpen = false;
                this.Popup.Opened += this.OnPopupOpened;
                this.Popup.Closed += this.OnPopupClosed;
            }
            if (this.ItemsSelector != null)
            {
                this.SelectionAdapter = new SelectionAdapter(this.ItemsSelector);
                this.SelectionAdapter.Commit += this.OnSelectionAdapterCommit;
                this.SelectionAdapter.Cancel += this.OnSelectionAdapterCancel;
                this.SelectionAdapter.SelectionChanged += this.OnSelectionAdapterSelectionChanged;
                this.ItemsSelector.PreviewMouseDown += this.ItemsSelector_PreviewMouseDown;
            }
        }

        private void ItemsSelector_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos_item = (e.OriginalSource as FrameworkElement)?.DataContext;
            if (pos_item == null) return;
            if (!this.ItemsSelector.Items.Contains(pos_item)) return;
            this.ItemsSelector.SelectedItem = pos_item;
            this.OnSelectionAdapterCommit();
        }

        private void AutoCompleteTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Editor?.Focus();
        }

        private string GetDisplayText(object dataItem)
        {
            if (this.BindingEvaluator == null)
            {
                this.BindingEvaluator = new BindingEvaluator(new Binding(this.DisplayMember));
            }
            if (dataItem == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(this.DisplayMember))
            {
                return dataItem.ToString();
            }
            return this.BindingEvaluator.Evaluate(dataItem);
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (this.SelectionAdapter != null)
            {
                if (this.IsDropDownOpen) this.SelectionAdapter.HandleKeyDown(e);
                else this.IsDropDownOpen = e.Key == Key.Down || e.Key == Key.Up;
            }
        }

        private void OnEditorLostFocus(object sender, RoutedEventArgs e)
        {
            if (!this.IsKeyboardFocusWithin)
            {
                this.IsDropDownOpen = false;
            }
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this._isUpdatingText) return;
            if (this.FetchTimer == null)
            {
                this.FetchTimer = new DispatcherTimer();
                this.FetchTimer.Interval = TimeSpan.FromMilliseconds(this.Delay);
                this.FetchTimer.Tick += this.OnFetchTimerTick;
            }
            this.FetchTimer.IsEnabled = false;
            this.FetchTimer.Stop();
            this.SetSelectedItem(null);
            if (this.Editor.Text.Length > 0)
            {
                this.IsLoading = true;
                this.IsDropDownOpen = true;
                this.ItemsSelector.ItemsSource = null;
                this.FetchTimer.IsEnabled = true;
                this.FetchTimer.Start();
            }
            else
            {
                this.IsDropDownOpen = false;
            }
        }

        private void OnFetchTimerTick(object sender, EventArgs e)
        {
            this.FetchTimer.IsEnabled = false;
            this.FetchTimer.Stop();
            if (this.Provider != null && this.ItemsSelector != null)
            {
                this.Filter = this.Editor.Text;
                if (this._suggestionsAdapter == null)
                {
                    this._suggestionsAdapter = new SuggestionsAdapter(this);
                }
                this._suggestionsAdapter.GetSuggestions(this.Filter);
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            if (!this._selectionCancelled)
            {
                this.OnSelectionAdapterCommit();
            }
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            this._selectionCancelled = false;
            this.ItemsSelector.SelectedItem = this.SelectedItem;
        }

        private void OnSelectionAdapterCancel()
        {
            this._isUpdatingText = true;
            this.Editor.Text = this.SelectedItem == null ? this.Filter : this.GetDisplayText(this.SelectedItem);
            this.Editor.SelectionStart = this.Editor.Text.Length;
            this.Editor.SelectionLength = 0;
            this._isUpdatingText = false;
            this.IsDropDownOpen = false;
            this._selectionCancelled = true;
        }

        private void OnSelectionAdapterCommit()
        {
            if (this.ItemsSelector.SelectedItem != null)
            {
                this.SelectedItem = this.ItemsSelector.SelectedItem;
                this._isUpdatingText = true;
                this.Editor.Text = this.GetDisplayText(this.ItemsSelector.SelectedItem);
                this.SetSelectedItem(this.ItemsSelector.SelectedItem);
                this._isUpdatingText = false;
                this.IsDropDownOpen = false;
            }
        }

        private void OnSelectionAdapterSelectionChanged()
        {
            this._isUpdatingText = true;
            if (this.ItemsSelector.SelectedItem == null)
            {
                this.Editor.Text = this.Filter;
            }
            else
            {
                this.Editor.Text = this.GetDisplayText(this.ItemsSelector.SelectedItem);
            }
            this.Editor.SelectionStart = this.Editor.Text.Length;
            this.Editor.SelectionLength = 0;
            this.ScrollToSelectedItem();
            this._isUpdatingText = false;
        }

        private void SetSelectedItem(object item)
        {
            this._isUpdatingText = true;
            this.SelectedItem = item;
            this._isUpdatingText = false;
        }

        #endregion

        #region "Nested Types"

        private class SuggestionsAdapter
        {

            #region "Fields"

            private AutoCompleteTextBox _actb;

            private string _filter;

            #endregion

            #region "Constructors"

            public SuggestionsAdapter(AutoCompleteTextBox actb)
            {
                this._actb = actb;
            }

            #endregion

            #region "Methods"

            public void GetSuggestions(string searchText)
            {
                this._filter = searchText;
                this._actb.IsLoading = true;
                ParameterizedThreadStart thInfo = new ParameterizedThreadStart(this.GetSuggestionsAsync);
                Thread th = new Thread(thInfo);
                th.Start(new object[] { searchText, this._actb.Provider });
            }

            private void DisplaySuggestions(IEnumerable suggestions, string filter)
            {
                if (this._filter != filter)
                {
                    return;
                }
                if (this._actb.IsDropDownOpen)
                {
                    this._actb.IsLoading = false;
                    this._actb.ItemsSelector.ItemsSource = suggestions;
                    this._actb.IsDropDownOpen = this._actb.ItemsSelector.HasItems;
                }

            }

            private void GetSuggestionsAsync(object param)
            {
                object[] args = param as object[];
                string searchText = Convert.ToString(args[0]);
                ISuggestionProvider provider = args[1] as ISuggestionProvider;
                IEnumerable list = provider.GetSuggestions(searchText);
                this._actb.Dispatcher.BeginInvoke(
                    new Action<IEnumerable, string>(this.DisplaySuggestions),
                    DispatcherPriority.Background,
                    new object[] { list, searchText });
            }

            #endregion

        }

        #endregion
    }
}