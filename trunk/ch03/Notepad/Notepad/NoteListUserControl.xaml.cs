using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Notepad.NotepadServiceProxy;

namespace Notepad
{
    public partial class NoteListUserControl : UserControl
    {
        public NoteListUserControl()
        {
            InitializeComponent();
        }

        private void lstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                NotepadViewModel.Instance.SelectedNote = (NoteDto)e.AddedItems[0];
                NotepadViewModel.Instance.ShowNoteList = false;
            }
        }

       
    }
}
