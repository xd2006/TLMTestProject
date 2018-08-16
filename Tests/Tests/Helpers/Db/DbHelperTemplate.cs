
namespace Tests.Helpers.Db
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using global::Tests.Managers;

    using PetaPoco;

    public abstract class DbHelperTemplate : HelperTemplate
    {
        protected DbHelperTemplate(ApplicationManager app)
            : base(app)
        {
        }

        protected IDatabase Db { get; set; }

        public T GetEntity<T>(Sql query, int retries = 25)
        {
            return GetEntity(Db.Single<T>, query, retries);
        }

        public T GetEntity<T>(int primaryKey, int retries = 25)
        {
            return GetEntity(Db.Single<T>, primaryKey, retries);         
        }

        public List<T> GetEntities<T>(Sql query, int retries = 25)
        {
            int i = 0;
            List<T> entities = new List<T>();
            string message = string.Empty;

            do
            {
                try
                {
                    entities = Db.Fetch<T>(query);
                    message = string.Empty;
                }
                catch (Exception e)
                {
                    message = e.Message;
                    Thread.Sleep(500);
                }
            }
            while (message != string.Empty && i++ < retries);

            if (i >= retries)
            {
                throw new Exception(message);
            }
           
            return entities;
        }

        private T GetEntity<T>(Func<object, T> action, object query, int retries = 25)
        {
            int i = 0;
            T entity = default(T);
            string message = string.Empty;

            do
            {
                try
                {
                    entity = action(query);
                    message = string.Empty;
                }
                catch (Exception e)
                {
                    message = e.Message;
                    Thread.Sleep(500);
                }
            }
            while (message != string.Empty && i++ < retries);

            if (i >= retries)
            {
                throw new Exception(message);
            }

            return entity;
        }
    }
}
