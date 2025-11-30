using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Services.Interfaces
{
    public interface INewsStorageService
    {
        HashSet<string> LoadSeenNews();
        void SaveSeenNews(HashSet<string> seenNews);
    }
}
