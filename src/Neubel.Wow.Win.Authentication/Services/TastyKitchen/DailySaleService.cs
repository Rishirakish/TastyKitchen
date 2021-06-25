using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces.TastyKitchen;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Services.TastyKitchen
{
    public class DailySaleService : IDailySaleService
    {
        IDailySaleRepository _dailySaleRepository;
        public DailySaleService(IDailySaleRepository dailySaleRepository)
        {
            _dailySaleRepository = dailySaleRepository;
        }

        public RequestResult<int> Add(DailySale dailySale)
        {
            _dailySaleRepository.Insert(dailySale);
            return new RequestResult<int>(1);
        }

        public bool Delete(int id)
        {
            _dailySaleRepository.Delete(id);
            return true;
        }

        public List<DailySale> Get()
        {
            return _dailySaleRepository.Get();
        }

        public DailySale Get(int id)
        {
            return _dailySaleRepository.Get(id);
        }

        public List<DailySale> GetPages(int pageIndex)
        {

            return _dailySaleRepository.GetPages(pageIndex).ToNonNullList();
        }

        public RequestResult<int> Update(int id, DailySale dailySale)
        {
            _dailySaleRepository.Update(dailySale);
            return new RequestResult<int>(1);
        }
    }
}
