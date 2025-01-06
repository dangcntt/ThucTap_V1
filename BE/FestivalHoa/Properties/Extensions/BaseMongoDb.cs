using MongoDB.Driver;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Extensions
{
    public class BaseMongoDb<T, TUEntity> where T : class, TEntity<TUEntity>
    {
        IMongoCollection<T> collection;
        public BaseMongoDb(IMongoCollection<T> collection)
        {
            this.collection = collection;
        }
        public async Task<ResultBaseMongo<T>> CreateAsync(T entity)
        {
            await collection.InsertOneAsync(entity);
            if (entity.Id == null)
            {
                return new ResultBaseMongo<T>(entity);
            }
            return new ResultBaseMongo<T>(entity, true);
        }
        public async Task<ResultBaseMongo<T>> UpdateAsync(T entity)
        {
            ReplaceOneResult actionResult
              = await collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id)
                      , entity
                      , new ReplaceOptions { IsUpsert = true });
            var result = actionResult.IsAcknowledged && actionResult.MatchedCount > 0;
            return new ResultBaseMongo<T>(entity, result);
        }

        public async Task<ResultBaseMongo<T>> DeleteAsync(T entity)
        {
            ReplaceOneResult actionResult
              = await collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id)
                      , entity
                      , new ReplaceOptions { IsUpsert = true });
            var result = actionResult.IsAcknowledged && actionResult.MatchedCount > 0;
            return new ResultBaseMongo<T>(entity, result);
        }

        public async Task<ResultBaseMongo<T>> DeletedAsync(T entity)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(p => p.Id, entity.Id);

            var deleteResult = await collection.DeleteOneAsync(filter);

            var result = deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
            return new ResultBaseMongo<T>(result);
        }

        public async Task<ResultBaseMongo<T>> DeleteByIdsync(TUEntity id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(p => p.Id, id);

            var deleteResult = await collection.DeleteOneAsync(filter);

            var result = deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
            return new ResultBaseMongo<T>(result);
        }

    }

    public class ResultBaseMongo<T>
    {
        public ResultBaseMongo()
        {
        }
        public ResultBaseMongo(bool Success = false)
        {
            this.Success = Success;
        }
        public ResultBaseMongo(T Entity, bool Success = false)
        {
            this.Entity = Entity;
            this.Success = Success;
        }
        public T Entity { get; set; }
        public bool Success { get; set; }
    }
}
