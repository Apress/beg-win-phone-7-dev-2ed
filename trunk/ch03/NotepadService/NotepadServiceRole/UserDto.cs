using System.Runtime.Serialization;

namespace NotepadServiceRole
{
    [DataContract]
    public class UserDto
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}