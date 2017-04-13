using RogueFlashNetCoreMvc.Model;
using System;

namespace RogueFlashNetCoreMvc.Daos
{
    public class AbstractDao : IDisposable
    {
        protected AppDbContext DbContext { get; } = null;


        public AbstractDao(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }


        private bool disposed = false;

        protected virtual void Dispose(bool dispose)
        {
            if (!disposed)
            {
                if (dispose)
                {
                    DbContext.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
