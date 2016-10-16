using System.ServiceModel;
using System.Collections.Generic;
using System;

namespace NotepadServiceRole
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        Guid AddUser(Guid userId, string userName);

        [OperationContract]
        NoteDto AddNote(Guid userId, string notedescription, string noteText);

        [OperationContract]
        void UpdateNote(int noteId, string noteText);

        [OperationContract]
        void DeleteNote(Guid userId, int noteId);

        [OperationContract]
        List<NoteDto> GetNotes(Guid userId);

        [OperationContract]
        NoteDto GetNote(Guid userId, int noteId);
    }
}
