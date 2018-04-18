namespace Hms.UI.Infrastructure.Controls.PagingControl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [TemplatePart(Name = "PART_FirstPageButton", Type = typeof(Button)),
     TemplatePart(Name = "PART_PreviousPageButton", Type = typeof(Button)),
     TemplatePart(Name = "PART_PageTextBox", Type = typeof(TextBox)),
     TemplatePart(Name = "PART_NextPageButton", Type = typeof(Button)),
     TemplatePart(Name = "PART_LastPageButton", Type = typeof(Button)),
     TemplatePart(Name = "PART_PageSizesCombobox", Type = typeof(ComboBox))]
    public class PagingControl : Control
    {
        #region CUSTOM CONTROL VARIABLES

        protected Button BtnFirstPage;

        protected Button BtnPreviousPage;

        protected Button BtnNextPage;

        protected Button BtnLastPage;

        protected TextBox TxtPage;

        protected ComboBox CmbPageSizes;

        #endregion

        #region PROPERTIES

        public static readonly DependencyProperty ItemsSourceProperty;

        public static readonly DependencyProperty PageProperty;

        public static readonly DependencyProperty TotalPagesProperty;

        public static readonly DependencyProperty PageSizesProperty;

        public static readonly DependencyProperty PageContractProperty;

        public static readonly DependencyProperty FilterProperty;

        public int Page
        {
            get
            {
                return (int)this.GetValue(PageProperty);
            }

            set
            {
                this.SetValue(PageProperty, value);
            }
        }

        public int TotalPages
        {
            get
            {
                return (int)this.GetValue(TotalPagesProperty);
            }

            protected set
            {
                this.SetValue(TotalPagesProperty, value);
            }
        }

        public ObservableCollection<int> PageSizes
        {
            get
            {
                return this.GetValue(PageSizesProperty) as ObservableCollection<int>;
            }
        }

        public ObservableCollection<object> ItemsSource
        {
            get
            {
                return this.GetValue(ItemsSourceProperty) as ObservableCollection<object>;
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        public IPageControlContract PageContract
        {
            get
            {
                return this.GetValue(PageContractProperty) as IPageControlContract;
            }

            set
            {
                this.SetValue(PageContractProperty, value);
            }
        }

        public object Filter
        {
            get
            {
                return this.GetValue(FilterProperty);
            }

            set
            {
                this.SetValue(FilterProperty, value);
            }
        }

        #endregion

        #region EVENTS

        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs args);

        public static readonly RoutedEvent PreviewPageChangeEvent;

        public static readonly RoutedEvent PageChangedEvent;

        public event PageChangedEventHandler PreviewPageChange
        {
            add
            {
                this.AddHandler(PreviewPageChangeEvent, value);
            }

            remove
            {
                this.RemoveHandler(PreviewPageChangeEvent, value);
            }
        }

        public event PageChangedEventHandler PageChanged
        {
            add
            {
                this.AddHandler(PageChangedEvent, value);
            }

            remove
            {
                this.RemoveHandler(PageChangedEvent, value);
            }
        }

        #endregion

        #region CONTROL CONSTRUCTORS

        static PagingControl()
        {
            TotalPagesProperty = DependencyProperty.Register("TotalPages", typeof(int), typeof(PagingControl));
            PageProperty = DependencyProperty.Register("Page", typeof(int), typeof(PagingControl));
            ItemsSourceProperty = DependencyProperty.Register(
                "ItemsSource",
                typeof(ObservableCollection<object>),
                typeof(PagingControl),
                new PropertyMetadata(new ObservableCollection<object>()));
            PageSizesProperty = DependencyProperty.Register(
                "PageSizes",
                typeof(ObservableCollection<int>),
                typeof(PagingControl),
                new PropertyMetadata(new ObservableCollection<int>()));
            PageContractProperty = DependencyProperty.Register(
                "PageContract",
                typeof(IPageControlContract),
                typeof(PagingControl));
            FilterProperty = DependencyProperty.Register(
                "Filter",
                typeof(object),
                typeof(PagingControl),
                new FrameworkPropertyMetadata(Target));

            PreviewPageChangeEvent = EventManager.RegisterRoutedEvent(
                "PreviewPageChange",
                RoutingStrategy.Bubble,
                typeof(PageChangedEventHandler),
                typeof(PagingControl));
            PageChangedEvent = EventManager.RegisterRoutedEvent(
                "PageChanged",
                RoutingStrategy.Bubble,
                typeof(PageChangedEventHandler),
                typeof(PagingControl));
        }

        private static void Target(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var target = (PagingControl)dependencyObject;
            target.NavigateAsync(PageChanges.Current);
        }

        public PagingControl()
        {
            this.ItemsSource = new ObservableCollection<object>();
            this.Loaded += this.PaggingControlLoaded;
        }

        ~PagingControl()
        {
            this.UnregisterEvents();
        }

        #endregion

        #region EVENTS

        void PaggingControlLoaded(object sender, RoutedEventArgs e)
        {
            if (this.Template == null)
            {
                throw new Exception("Control template not assigned.");
            }

            if (this.PageContract == null)
            {
                throw new Exception("IPageControlContract not assigned.");
            }

            this.RegisterEvents();
            this.SetDefaultValues();
            this.BindProperties();
        }

        public async void BtnFirstPageClick(object sender, RoutedEventArgs e)
        {
            await this.NavigateAsync(PageChanges.First);
        }

        public async void BtnPreviousPageClick(object sender, RoutedEventArgs e)
        {
            await this.NavigateAsync(PageChanges.Previous);
        }

        public async void BtnNextPageClick(object sender, RoutedEventArgs e)
        {
            await this.NavigateAsync(PageChanges.Next);
        }

        public async void BtnLastPageClick(object sender, RoutedEventArgs e)
        {
            await this.NavigateAsync(PageChanges.Last);
        }

        public async void TxtPageLostFocus(object sender, RoutedEventArgs e)
        {
            await this.NavigateAsync(PageChanges.Current);
        }

        public async void CmbPageSizesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await this.NavigateAsync(PageChanges.Current);
        }

        #endregion

        #region INTERNAL METHODS

        public override void OnApplyTemplate()
        {
            this.BtnFirstPage = this.Template.FindName("PART_FirstPageButton", this) as Button;
            this.BtnPreviousPage = this.Template.FindName("PART_PreviousPageButton", this) as Button;
            this.TxtPage = this.Template.FindName("PART_PageTextBox", this) as TextBox;
            this.BtnNextPage = this.Template.FindName("PART_NextPageButton", this) as Button;
            this.BtnLastPage = this.Template.FindName("PART_LastPageButton", this) as Button;
            this.CmbPageSizes = this.Template.FindName("PART_PageSizesCombobox", this) as ComboBox;

            if (this.BtnFirstPage == null || this.BtnPreviousPage == null || this.TxtPage == null || this.BtnNextPage == null
                || this.BtnLastPage == null || this.CmbPageSizes == null)
            {
                throw new Exception("Invalid Control template.");
            }

            base.OnApplyTemplate();
        }

        private void RegisterEvents()
        {
            this.BtnFirstPage.Click += this.BtnFirstPageClick;
            this.BtnPreviousPage.Click += this.BtnPreviousPageClick;
            this.BtnNextPage.Click += this.BtnNextPageClick;
            this.BtnLastPage.Click += this.BtnLastPageClick;

            this.TxtPage.LostFocus += this.TxtPageLostFocus;

            this.CmbPageSizes.SelectionChanged += this.CmbPageSizesSelectionChanged;
        }

        private void UnregisterEvents()
        {
            this.BtnFirstPage.Click -= this.BtnFirstPageClick;
            this.BtnPreviousPage.Click -= this.BtnPreviousPageClick;
            this.BtnNextPage.Click -= this.BtnNextPageClick;
            this.BtnLastPage.Click -= this.BtnLastPageClick;

            this.TxtPage.LostFocus -= this.TxtPageLostFocus;

            this.CmbPageSizes.SelectionChanged -= this.CmbPageSizesSelectionChanged;
        }

        private void SetDefaultValues()
        {
            this.ItemsSource.Clear();

            this.CmbPageSizes.IsEditable = false;
            this.CmbPageSizes.SelectedIndex = 0;
        }

        private void BindProperties()
        {
            var propBinding = new Binding("Page")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            this.TxtPage.SetBinding(TextBox.TextProperty, propBinding);

            propBinding = new Binding("PageSizes")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            this.CmbPageSizes.SetBinding(ItemsControl.ItemsSourceProperty, propBinding);
        }

        private void RaisePageChanged(int oldPage, int newPage)
        {
            PageChangedEventArgs args = new PageChangedEventArgs(PageChangedEvent, oldPage, newPage, this.TotalPages);
            this.RaiseEvent(args);
        }

        private void RaisePreviewPageChange(int oldPage, int newPage)
        {
            PageChangedEventArgs args = new PageChangedEventArgs(PreviewPageChangeEvent, oldPage, newPage, this.TotalPages);
            this.RaiseEvent(args);
        }

        private async Task NavigateAsync(PageChanges change)
        {
            if (this.ItemsSource == null)
            {
                this.ItemsSource = new ObservableCollection<object>();
            }

            if (this.PageContract == null)
            {
                return;
            }

            var totalRecords = await this.PageContract.GetTotalCountAsync();
            var newPageSize = (int)this.CmbPageSizes.SelectedItem;

            if (totalRecords == 0)
            {
                this.ItemsSource.Clear();
                this.TotalPages = 1;
                this.Page = 1;
            }
            else
            {
                this.TotalPages = (totalRecords / newPageSize) + ((totalRecords % newPageSize == 0) ? 0 : 1);
            }

            int newPage = 1;

            switch (change)
            {
                case PageChanges.First:
                    if (this.Page == 1)
                    {
                        return;
                    }

                    break;
                case PageChanges.Previous:
                    newPage = (this.Page - 1 > this.TotalPages) ? this.TotalPages : (this.Page - 1 < 1) ? 1 : this.Page - 1;
                    break;
                case PageChanges.Current:
                    newPage = (this.Page > this.TotalPages) ? this.TotalPages : (this.Page < 1) ? 1 : this.Page;
                    break;
                case PageChanges.Next:
                    newPage = (this.Page + 1 > this.TotalPages) ? this.TotalPages : this.Page + 1;
                    break;
                case PageChanges.Last:
                    if (this.Page == this.TotalPages)
                    {
                        return;
                    }

                    newPage = this.TotalPages;
                    break;
            }

            var startingIndex = (newPage - 1) * newPageSize;

            var oldPage = this.Page;
            this.RaisePreviewPageChange(this.Page, newPage);

            this.Page = newPage;

            if (this.ItemsSource == null)
            {
                this.ItemsSource = new ObservableCollection<object>();
            }

            this.ItemsSource.Clear();

            ICollection<object> fetchData = await this.PageContract.GetRecordsAsync(startingIndex, newPageSize, this.Filter);
            foreach (object row in fetchData)
            {
                this.ItemsSource.Add(row);
            }

            this.RaisePageChanged(oldPage, this.Page);
        }

        #endregion
    }
}