using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using ChatRoom.Services.GroupMessageService;
using ChatRoom.Services.MessageService;
using ChatRoom.Services.PersonGroupService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services.GroupService
{
	public interface IGroupService : ICrudService<Group>
	{
		public Task<List<Group>> ListGroup(List<Guid> Gids);
		public Task<List<GroupMessage>> LsitMessageGroup(Guid Gid);


		public Task<Group> FindByName(string Name);
	}

	public class GroupService : BaseService<Group>, IGroupService
	{
		private readonly IGroupMessageService messageService;
		private readonly IPersonGroupService personGroupService;
		public GroupService(IOptions<MessageDbContext> mongoDatabaseSettings, IGroupMessageService messageService) : base(mongoDatabaseSettings)
		{
			this.messageService = messageService;
		}
	

		public async Task<List<Group>> ListGroup(List<Guid> Gids) => await _collection.Find(x => Gids.Contains(x.Id)).ToListAsync();
		public async Task<Group> FindByName(string name) => await _collection.Find(x => x.Name.Trim().ToLower() == name.ToLower()).FirstOrDefaultAsync();

		public async Task<Group?> GetAsync(Guid id) =>
		await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task<List<GroupMessage>> LsitMessageGroup(Guid Gid)
         => await messageService.GetListAsync(Gid);

		public async Task RemoveAsync(Guid id) =>
		  await _collection.DeleteOneAsync(x => x.Id == id);

		public async Task RemoveAsync(List<Guid> id) =>
		   await _collection.DeleteManyAsync(x => id.Contains(x.Id));

		public async Task UpdateAsync(Guid id, Group updateModel) =>
			 await _collection.ReplaceOneAsync(x => x.Id == id, updateModel);


		public async Task UpdateAsync(List<Group> updateModel)
		{
			foreach (var item in updateModel)
			{
				await UpdateAsync(item.Id, item);
			}

		}

	}

}
