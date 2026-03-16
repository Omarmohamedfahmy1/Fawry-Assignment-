namespace SportsManagement.API.Models;

public class Message
{
    public int MessageId { get; set; }
    public int SenderUserId { get; set; }
    public int ReceiverUserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;

    public User Sender { get; set; } = null!;
    public User Receiver { get; set; } = null!;
}
