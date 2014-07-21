namespace ErikLieben.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class EFRepositoryBase<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext dataContext;
        private readonly IDbSet<T> dbset;

        protected EFRepositoryBase(IDatabaseFactory databaseFactory)
        {
            this.DatabaseFactory = databaseFactory;
            this.dataContext = this.DatabaseFactory.CreateContext();
            this.dbset = DataContext.Set<T>();
        }

        ~EFRepositoryBase()
        {
            this.dataContext.Dispose();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected DbContext DataContext
        {
            get
            {
                return this.dataContext;
            }
        }


        public virtual void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.dbset.Add(item);
        }

        public void Delete(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.dbset.Remove(item);
        }

        public void Update(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.dbset.Attach(item);
            this.DataContext.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<T> Find(ISpecification<T> specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }

            return this.dbset.Where(specification.Predicate);
        }

        public IEnumerable<T> FindAll()
        {
            return this.dbset;
        }

        public async Task<IEnumerable<T>> FindAsync(
            ISpecification<T> specification, 
            CancellationToken token = new CancellationToken())
        { 
            if (specification == null) 
            {
                throw new ArgumentNullException("specification");
            }

            return await this.dbset.Where(specification.Predicate).ToListAsync(token);
        }

        public async Task<int> SubmitChangesAsync()
        {
            return await this.DataContext.SaveChangesAsync();
        }

        public async Task<int> SubmitChangesAsync(CancellationToken token)
        {
            return await this.DataContext.SaveChangesAsync(token);
        }
    }
}
