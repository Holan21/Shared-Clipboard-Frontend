using Shared_Clipboard_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shared_Clipboard_Frontend.Services.api
{
    public interface IClipboardService
    {
        Task<List<ClipboardItem>> GetAllClipboardItems(CancellationToken cancellationToken = default);
    }
}