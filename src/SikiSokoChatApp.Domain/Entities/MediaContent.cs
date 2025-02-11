using Microsoft.VisualBasic;
using SikiSokoChatApp.Shared.Domain.Entities;
using System.Buffers.Text;

namespace SikiSokoChatApp.Domain.Entities;

public class MediaContent : BaseEntity // pdf foto video
{
    public int MessageId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string StoragePath { get; set; } // c:user/sadfafas
    public Message Message { get; set; }
}