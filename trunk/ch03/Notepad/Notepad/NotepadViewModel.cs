using System;
using System.Windows;
using System.IO.IsolatedStorage;
using System.ComponentModel;
using Notepad.NotepadServiceProxy;
using System.Collections.ObjectModel;
using System.Linq;

namespace Notepad
{
    /// <summary>
    /// Settings class is singleton instance that will contain various application
    /// configuration that will be used by all the controls of the application.
    /// </summary>
    public sealed class NotepadViewModel : INotifyPropertyChanged
    {
        // For creating Singleton instance
        public static NotepadViewModel Instance = new NotepadViewModel();

        // For calling Notepad web service
        private ServiceClient _svc;

        // Populated when the user registers firstime
        // and the value is saved to the isolated storage
        public Guid UserId
        {
            get 
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("UserId"))
                {
                    return (Guid)IsolatedStorageSettings.ApplicationSettings["UserId"];
                }
                else
                {
                    return Guid.Empty;
                }
            }
            set
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("UserId"))
                {
                    IsolatedStorageSettings.ApplicationSettings["UserId"] = value;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("UserId", value);
                }

                // Raise property chnaged event to alert user registration control
                // so that if the UserId is empty user registration screen
                // will be prompted for the user to register.
                //
                // To see how raise property changed event works with control Binding
                // see Binding attributes on ucUserRegistration control in MainPage.xaml
                this.RaisePropertyChanged("UserId");
                this.RaisePropertyChanged("NeedUserId");
            }
        }

        // Checks to see if the UserId exist in the isolated storage
        // and make sure UserId is not a empty Guid
        public bool NeedUserId
        {
            get
            {
                return !IsolatedStorageSettings.ApplicationSettings.Contains("UserId") 
                    || (Guid)IsolatedStorageSettings.ApplicationSettings["UserId"] == Guid.Empty;
            }
        }

        // ShowNoteList is bound to NoteListUserControl in the MainPage
        // and it will hide if false and else unhide if true.
        private bool _showNoteList = false;
        public bool ShowNoteList
        {
            get
            {
                return _showNoteList;
            }

            set
            {
                _showNoteList = value;
                this.RaisePropertyChanged("ShowNoteList");
            }
        }

        // ShowNoteList is bound to NoteListUserControl in the MainPage
        // and it will hide if false and else unhide if true.
        private NoteDto _note;
        public NoteDto SelectedNote
        {
            get
            {
                return _note;
            }

            set
            {
                _note = value;
                this.RaisePropertyChanged("SelectedNote");
            }
        }

        // SelectedNote is populated from NoteListUserControl 
        // when the user selectes the note from the list box.
        // SelectedNote is then used in MainPage by txtNote and
        // txtNoteName to populate to textbox content.
        private ObservableCollection<NoteDto> _notes;
        public ObservableCollection<NoteDto> Notes
        {
            get
            {
                return _notes;
            }

            set
            {
                _notes = value;
                this.RaisePropertyChanged("Notes");
            }
        }

        private NotepadViewModel()
        {
            _svc = new ServiceClient();
            _svc.GetNotesCompleted += new EventHandler<GetNotesCompletedEventArgs>(_svc_GetNotesCompleted);
            _svc.AddNoteCompleted += new EventHandler<AddNoteCompletedEventArgs>(_svc_AddNoteCompleted);
            _svc.UpdateNoteCompleted += new EventHandler<AsyncCompletedEventArgs>(_svc_UpdateNoteCompleted);
            _svc.AddUserCompleted += new EventHandler<AddUserCompletedEventArgs>(_svc_AddUserCompleted);
            _svc.DeleteNoteCompleted += new EventHandler<AsyncCompletedEventArgs>(_svc_DeleteNoteCompleted);

            if (this.NeedUserId)
            {
                this.Notes = new ObservableCollection<NoteDto>();
            }
            else
            {
                this.RebindData();
            }
        }

        // To rebind the data GetNotes will be called to retrieve
        // all the user notes and resetting Notes value.
        public void RebindData()
        {
            _svc.GetNotesAsync(this.UserId);
        }

        public void SaveNote(string noteName, string noteText)
        {
            // Search the user notes and see if the note already exist
            var note = (from eachNote in this.Notes
                       where eachNote.NoteText.Equals(noteText, StringComparison.InvariantCultureIgnoreCase)
                       select eachNote).SingleOrDefault();

            if (note == null)
            {
                _svc.AddNoteAsync(this.UserId, noteName, noteText);
            }
            else
            {
                _svc.UpdateNoteAsync(note.NoteId, noteText);
            }

            this.SelectedNote = note;
        }

        public void AddUser(Guid userId, string userName)
        {
            if (this.NeedUserId)
            {
                _svc.AddUserAsync(userId, userName);
            }
        }

        public void DeleteNote()
        {
            _svc.DeleteNoteAsync(this.UserId, this.SelectedNote.NoteId);
        }


        private void _svc_AddNoteCompleted(object sender, AddNoteCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.SelectedNote = e.Result;
                this.RebindData();
            }
            else
            {
                MessageBox.Show("Failed to add the note. Please ty again!");
            }
        }

        private void _svc_GetNotesCompleted(object sender, GetNotesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.Notes = e.Result;
            }
            else
            {
                MessageBox.Show("Failed to get the notes. Please ty again!");
            }
        }

        private void _svc_UpdateNoteCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.RebindData();
            }
            else
            {
                MessageBox.Show("Failed to update the note. Please ty again!");
            }
        }

        private void _svc_AddUserCompleted(object sender, AddUserCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                // Set the UserId only when AddUser service call 
                // was made successfully
                this.UserId = e.Result;
            }
            else
            {
                this.UserId = Guid.Empty;
                MessageBox.Show("Failed to add user please try again!");
            }
        }

        private void _svc_DeleteNoteCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.SelectedNote = null;
                this.RebindData();
            }
            else
            {
                MessageBox.Show("Failed to delet note please try again!");
            }
        }
        
        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
