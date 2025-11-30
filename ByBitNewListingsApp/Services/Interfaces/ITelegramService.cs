using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Services.Interfaces
{
    public interface ITelegramService
    {
        Task<bool> SendMessageAsync(string message);
    }
}
