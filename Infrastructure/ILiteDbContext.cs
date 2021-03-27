using LiteDB;

namespace AlsTools.Infrastructure
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}
