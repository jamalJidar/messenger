using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services.GroupMessageService
{
	public interface IGroupMessageService:ICrudService<GroupMessage>
	{
		public Task<List<GroupMessage>> GetListAsync(Guid Gid);
	}

	public class GroupMessageService : BaseService<GroupMessage>, IGroupMessageService
	{
		public GroupMessageService(IOptions<MessageDbContext> mongoDatabaseSettings) : base(mongoDatabaseSettings)
		{
		}


		public async Task<GroupMessage?> GetAsync(Guid Gid) =>
	      await _collection.Find(x => x.GroupId== Gid).FirstOrDefaultAsync();
		public async Task<List<GroupMessage>> GetListAsync(Guid Gid)=>
			  await _collection.Find(x => x.GroupId == Gid).ToListAsync();



		public async Task RemoveAsync(Guid id) =>
		  await _collection.DeleteOneAsync(x => x.Id == id);

		public async Task RemoveAsync(List<Guid> id) =>
		   await _collection.DeleteManyAsync(x => id.Contains(x.Id));

		public async Task UpdateAsync(Guid id, GroupMessage updateModel) =>
			 await _collection.ReplaceOneAsync(x => x.Id == id, updateModel);


		public async Task UpdateAsync(List<GroupMessage> updateModel)
		{
			foreach (var item in updateModel)
			{
				await UpdateAsync(item.Id, item);
			}

		}
	}
}
