using HotChocolate.Data;
using MongoDB.Driver;

public partial class Query
{
    public IExecutable<TempIncome> TempIncomes([Service] IMongoDatabase db)
    {
        return db.GetCollection<TempIncome>("TempIncomes").AsExecutable();
    }
}

