using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using ChatRoom.Services.MessagePaaswordService;
using ChatRoom.Services.MessageService;
using ChatRoom.Services.PersonService;
using ChatRoom.Services.UserSeenService;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services.OlineUserService
{
	public interface IUserOnlineService : ICrudService<OnlieUser>
	{
		public Task<OnlieUser> CheckStatusPersonAsync(Guid UserId);
		public Task<OnlieUser> CheckStatusPersonAsync(string UserId);
		public Task<OnlieUser> CheckStatusPersonAsync(Guid UserId, OnlieUserStatus status);
		public Task<List<OnlieUser>> CheckStatusPersonAsync(List<Guid> UserIds);
		public Task<List<OnlieUser>> CheckStatusPersonAsync(List<Guid> UserIds, OnlieUserStatus status);

	}

	public class UserOnlineService : BaseService<OnlieUser>, IUserOnlineService
	{

		public UserOnlineService(IOptions<MessageDbContext> mongoDatabaseSettings) :
			base(mongoDatabaseSettings)
		{

		}
		public async Task<OnlieUser> CheckStatusPersonAsync(Guid id) =>
			 await _collection.Find(x => x.UserId == id).FirstOrDefaultAsync();


		public async Task<OnlieUser> CheckStatusPersonAsync(Guid id, OnlieUserStatus status) =>
			 await _collection.Find(x => x.UserId == id && x.Status == status).FirstOrDefaultAsync();

		public async Task<List<OnlieUser>> CheckStatusPersonAsync(List<Guid> UserIds) =>
			await _collection.Find(x => UserIds.Contains(x.UserId)).ToListAsync();

		public async Task<List<OnlieUser>> CheckStatusPersonAsync(List<Guid> UserIds, OnlieUserStatus status) =>
			await _collection.Find(x => UserIds.Contains(x.UserId) && x.Status == status).ToListAsync();



		public async Task<OnlieUser> CheckStatusPersonAsync(string UserId) =>
			await _collection.Find(x => x.UserName == UserId).FirstOrDefaultAsync();


		public async Task<OnlieUser?> GetAsync(Guid id) =>
			  await _collection.Find(x => x.UserId == id).FirstOrDefaultAsync();

		public async Task RemoveAsync(Guid id) =>
		await _collection.DeleteOneAsync(x => x.Id == id);

		public async Task RemoveAsync(List<Guid> id) =>
		await _collection.DeleteManyAsync(x => id.Contains(x.Id));

		public async Task UpdateAsync(Guid id, OnlieUser updateModel)
		{

			 
			if (updateModel != null)
				if (await GetAsync(id) != null)
				{
					 
					await _collection.ReplaceOneAsync(x => x.UserId == id, updateModel);

				}



		}

		public async Task UpdateAsync(List<OnlieUser> updateModel)
		{
			foreach (var item in updateModel)
			{
				await UpdateAsync(item.UserId, item);

			}
		}
	}
}
