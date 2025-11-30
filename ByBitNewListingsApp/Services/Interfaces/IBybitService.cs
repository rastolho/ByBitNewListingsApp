using ByBitNewListingsApp.Dtos;

namespace ByBitNewListingsApp.Services.Interfaces
{
    public interface IBybitService
    {
        Task<List<Listing>> FetchNewsAsync();
    }
}
