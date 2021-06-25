using Dapper;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces.TastyKitchen;
using Neubel.Wow.Win.Authentication.Infrastructure;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class DailySaleRepository : IDailySaleRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public DailySaleRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool Delete(int id)
        {
            string query = @"update [DailySale] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }

        public List<DailySale> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<DailySale>("Select * From [DailySale] where IsDeleted=0").ToList();
        }

        public DailySale Get(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<DailySale>("Select top 1 * From [DailySale] where Id=@id and IsDeleted=0", new { id }).FirstOrDefault();
        }

        public IPagedList<DailySale> GetPages(int pageIndex = 1, int pageSize = 25)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            var query = db.QueryMultiple("SELECT COUNT(*) FROM [DailySale] where isDeleted=0;SELECT* FROM [DailySale] where isDeleted=0 ORDER BY Id desc OFFSET ((@PageNumber - 1) * @Rows) ROWS FETCH NEXT @Rows ROWS ONLY", new { PageNumber = pageIndex, Rows = pageSize }, commandType: CommandType.Text);
            var row = query.Read<int>().First();
            var pageResult = query.Read<DailySale>().ToList();
            return new StaticPagedList<DailySale>(pageResult, pageIndex, pageSize, row);
        }

        public int Insert(DailySale dailySale)
        {
            string query = @"Insert into [DailySale](BillNumber, Amount, Date, Type) 
                values (@BillNumber, @Amount, @Date, @Type)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, dailySale);
        }

        public int Update(DailySale dailySale)
        {
            string query = @"update [DailySale] Set 
                                BillNumber = @BillNumber, 
                                Amount = @Amount,
                                Date = @Date,
                                Type = @Type
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, dailySale);
        }
    }
}
