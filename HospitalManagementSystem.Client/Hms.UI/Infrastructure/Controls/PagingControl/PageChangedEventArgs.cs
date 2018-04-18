namespace Hms.UI.Infrastructure.Controls.PagingControl
{
    using System.Windows;

    public class PageChangedEventArgs : RoutedEventArgs
    {
        #region PRIVATE VARIABLES

        private int oldPage, newPage, totalPages;

        #endregion

        #region PROPERTIES

        public int OldPage
        {
            get
            {
                return this.oldPage;
            }
        }

        public int NewPage
        {
            get
            {
                return this.newPage;
            }
        }

        public int TotalPages
        {
            get
            {
                return this.totalPages;
            }
        }

        #endregion

        #region CONSTRUCTOR

        public PageChangedEventArgs(RoutedEvent eventToRaise, int oldPage, int newPage, int totalPages)
            : base(eventToRaise)
        {
            this.oldPage = oldPage;
            this.newPage = newPage;
            this.totalPages = totalPages;
        }

        #endregion
    }
}