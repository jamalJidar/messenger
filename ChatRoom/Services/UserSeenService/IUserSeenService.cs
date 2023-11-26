using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using ChatRoom.Services.PersonService;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services.UserSeenService
{
	public interface IUserSeenService 
	{
		public   Task CreateOrUpdateAsync(Guid  userID, string UserName);
		public Task CreateOrUpdateAsync(Guid userID, string UserName , DateTime dt);

		public Task<UserSeen?> GetAsync(string userName);
		public   Task UpdateAsync(Guid userid, UserSeen updateModel);
		public Task RemoveAsync(Guid id);
		public Task RemoveAsync(List<Guid> id);
		public Task<UserSeen> GetByUserIdAsync(Guid userId);

	}

	public class UserSeenService  :IUserSeenService
	{ 

		public IMongoCollection<UserSeen> _collection;


		public UserSeenService(
			IOptions<MessageDbContext> mongoDatabaseSettings)
		{
			var mongoClient = new MongoClient(
				mongoDatabaseSettings.Value.ConnectionString);

			var mongoDatabase = mongoClient.GetDatabase(
				mongoDatabaseSettings.Value.Name);
			string collectionName = $"{typeof(UserSeen)}";

			_collection = mongoDatabase.GetCollection<UserSeen>(mongoDatabaseSettings.Value.UserSeenCollectionName);
			 
		}





		public async Task CreateOrUpdateAsync(Guid userID, string UserName)
		{
			 var seen = await GetByUserIdAsync(userID);
			////create new userSeen
			if (seen == null)
			{
				await _collection.InsertOneAsync(new UserSeen ()
				{
					Id= Guid.NewGuid() , 
					LastSeen= Utility.RelativeTimeCalculator.SetKind( DateTime.Now , DateTimeKind.Local),
					Time=DateTime.Now,
					UserName= UserName , UserId = userID
				});
			
			}

			else
			{
				seen.LastSeen= Utility.RelativeTimeCalculator.SetKind(DateTime.Now, DateTimeKind.Local);
				seen.Time = DateTime.Now;
                await _collection.ReplaceOneAsync(x => x.UserId == userID, seen);

            }


        }
		//id= > userid
		public async Task<UserSeen?> GetAsync(string userName) =>
		await _collection.Find(x => x.UserName.Trim().ToLower() == userName.Trim().ToLower()).FirstOrDefaultAsync();

		public async Task<UserSeen?> GetAsync(Guid id) =>
		 await _collection.Find(x => x.UserId == id).FirstOrDefaultAsync();

        public async Task<UserSeen> GetByUserIdAsync(Guid userId)=>
          await _collection.Find(x => x.UserId== userId).FirstOrDefaultAsync();


        

		public async Task UpdateAsync(Guid userid, UserSeen updateModel) =>
			 await _collection.ReplaceOneAsync(x => x.UserId == updateModel.UserId, updateModel);
		public async Task RemoveAsync(List<Guid> id) =>
		   await _collection.DeleteManyAsync(x => id.Contains(x.Id));
		public async Task RemoveAsync(Guid id) =>
		   await _collection.DeleteOneAsync(x=>x.Id==id);

		public async Task CreateOrUpdateAsync(Guid userID, string UserName, DateTime dt)
		{

			var seen = await GetByUserIdAsync(userID);
			////create new userSeen
			if (seen == null)
			{
				await _collection.InsertOneAsync(new UserSeen()
				{
					Id = Guid.NewGuid(),
					LastSeen = Utility.RelativeTimeCalculator.SetKind(DateTime.Now, DateTimeKind.Local),
					Time = dt,
					UserName = UserName,
					UserId = userID
				});

			}

			else
			{
				seen.LastSeen = Utility.RelativeTimeCalculator.SetKind(DateTime.Now, DateTimeKind.Local);
				seen.Time = dt;
				await _collection.ReplaceOneAsync(x => x.UserId == userID, seen);

			}


		}
	}



}
