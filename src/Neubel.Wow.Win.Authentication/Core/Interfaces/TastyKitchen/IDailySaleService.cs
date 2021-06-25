﻿using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen
{
    public interface IDailySaleService
    {
        List<DailySale> Get();
        List<DailySale> GetPages(int pageIndex);
        DailySale Get(int id);
        RequestResult<int> Add(DailySale dailySale);
        RequestResult<int> Update(int id, DailySale dailySale);
        bool Delete(int id);
    }
}