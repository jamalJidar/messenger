﻿using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services
{
    public class BaseService<T> where T : class
    {

        public IMongoCollection<T> _collection;


        public BaseService(
            IOptions<MessageDbContext> mongoDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                mongoDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDatabaseSettings.Value.Name);
            string collectionName = $"{typeof(T)}";
       
            _collection = mongoDatabase.GetCollection<T>(collectionName.Replace("ChatRoom.Models.", "").Trim());
            //mongoDatabaseSettings.Value.PersonCollectionName
        }


        public async Task<List<T>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(T model) =>
         await _collection.InsertOneAsync(model);

        public async Task CreateAsync(List<T> models) =>
        await _collection.InsertManyAsync(models);
 
    }
}
