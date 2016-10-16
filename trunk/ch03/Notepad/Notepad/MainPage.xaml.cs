using System.Windows;
using Microsoft.Phone.Controls;

namespace Notepad
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.DataContext = NotepadViewModel.Instance;
            ucNoteList.DataContext = NotepadViewModel.Instance;
            ucUserRegistration.DataContext = NotepadViewModel.Instance;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNote.Text))
            {
                NotepadViewModel.Instance.SaveNote(txtNoteName.Text, txtNote.Text);
            }
        }

        private void btnViewEdit_Click(object sender, RoutedEventArgs e)
        {
            if (NotepadViewModel.Instance.Notes != null
                && NotepadViewModel.Instance.Notes.Count > 0)
            {
                NotepadViewModel.Instance.ShowNoteList = true;
            }
        }

        private void btnAddNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NotepadViewModel.Instance.SelectedNote = null;
        }

        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NotepadViewModel.Instance.DeleteNote();
        }
    }
}

//System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Clear();

