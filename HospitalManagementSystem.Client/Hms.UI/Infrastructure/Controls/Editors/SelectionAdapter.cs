namespace Hms.UI.Infrastructure.Controls.Editors
{
    using System.Diagnostics;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class SelectionAdapter
    {
        public SelectionAdapter(Selector selector)
        {
            this.SelectorControl = selector;
            this.SelectorControl.PreviewMouseUp += this.OnSelectorMouseDown;
        }

        public delegate void CancelEventHandler();

        public delegate void CommitEventHandler();

        public delegate void SelectionChangedEventHandler();

        public event CancelEventHandler Cancel;

        public event CommitEventHandler Commit;

        public event SelectionChangedEventHandler SelectionChanged;

        public Selector SelectorControl { get; set; }

        public void HandleKeyDown(KeyEventArgs key)
        {
            Debug.WriteLine(key.Key);
            switch (key.Key)
            {
                case Key.Down:
                    this.IncrementSelection();
                    break;
                case Key.Up:
                    this.DecrementSelection();
                    break;
                case Key.Enter:
                    this.Commit?.Invoke();
                    break;
                case Key.Escape:

                    break;
                case Key.Tab:
                    this.Commit?.Invoke();
                    break;
            }
        }

        private void DecrementSelection()
        {
            if (this.SelectorControl.SelectedIndex == -1)
            {
                this.SelectorControl.SelectedIndex = this.SelectorControl.Items.Count - 1;
            }
            else
            {
                this.SelectorControl.SelectedIndex -= 1;
            }

            this.SelectionChanged?.Invoke();
        }

        private void IncrementSelection()
        {
            if (this.SelectorControl.SelectedIndex == this.SelectorControl.Items.Count - 1)
            {
                this.SelectorControl.SelectedIndex = -1;
            }
            else
            {
                this.SelectorControl.SelectedIndex += 1;
            }

            this.SelectionChanged?.Invoke();
        }

        private void OnSelectorMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Commit?.Invoke();
        }
    }
}