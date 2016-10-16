using System.Runtime.Serialization;

namespace NotepadServiceRole
{
    [DataContract]
    public class NoteDto
    {
        [DataMember]
        public int NoteId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string NoteText { get; set; }

    }
}