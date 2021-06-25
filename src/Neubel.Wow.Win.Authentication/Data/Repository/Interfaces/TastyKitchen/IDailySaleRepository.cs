using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using PagedList;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Data.Repository.Interfaces.TastyKitchen
{
    public interface IDailySaleRepository
    {
        int Insert(DailySale dailySale);
        int Update(DailySale dailySale);
        List<DailySale> Get();
        IPagedList<DailySale> GetPages(int pageIndex = 1, int pageSize = 25);
        DailySale Get(int id);
        bool Delete(int id);
    }
}
