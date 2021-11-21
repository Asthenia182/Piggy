﻿using HotChocolate.Data;
using MongoDB.Driver;

public partial class Query
{
    public IExecutable<ConstIncome> ConstIncomes([Service] IMongoDatabase db)
    {
        return db.GetCollection<ConstIncome>("ConstIncomes").AsExecutable();
    }
}

