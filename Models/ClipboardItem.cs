namespace Shared_Clipboard_Frontend.Models
{
    public class ClipboardItem
    {
        public Guid Id { get; set; }
        public string Data { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
