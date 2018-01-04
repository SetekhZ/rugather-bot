using Microsoft.EntityFrameworkCore;

namespace RuGatherBot.Contracts
{
    public abstract class DbManager<T> where T : DbContext
    {
        internal readonly T db;

        public DbManager(T db)
        {
            this.db = db;
        }
    }
}
