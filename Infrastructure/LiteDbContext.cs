using System;
using AlsTools.Config;
using LiteDB;
using Microsoft.Extensions.Options;

namespace AlsTools.Infrastructure
{
    public class LiteDbContext// : ILiteDbContext
    {
        public LiteDatabase Database { get; }

        public LiteDbContext(IOptions<LiteDbOptions> options)
        {
            Database = new LiteDatabase(options.Value.DatabaseLocation);
        }
    }
}
