using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using ChatRoom.ViewModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services.PersonGroupService
{
	public interface IPersonGroupService : ICrudService<PersonGroup>
	{
		public Task<List<PersonGroup>> ListGroup(Guid UserId);
		public Task<PersonGroup> FindGroup(Guid UserId, Guid GroupId);
	}

	public class PersonGroupService : BaseService<PersonGroup>, IPersonGroupService
	{
		public PersonGroupService(IOptions<MessageDbContext> mongoDatabaseSettings) : base(mongoDatabaseSettings)
		{
		}


		public async Task<PersonGroup?> GetAsync(Guid id) =>
	    await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task<PersonGroup> FindGroup(Guid UserId, Guid GroupId)=>
			await _collection.Find(x=>x.PersonId==UserId && x.GroupId==GroupId).FirstOrDefaultAsync();


		public async Task<List<PersonGroup>> ListGroup(Guid UserId)
		 =>	await _collection.Find(x => x.PersonId == UserId).ToListAsync();
		

		public async Task RemoveAsync(Guid id) =>
		  await _collection.DeleteOneAsync(x => x.Id == id);

		public async Task RemoveAsync(List<Guid> id) =>
		   await _collection.DeleteManyAsync(x => id.Contains(x.Id));

		public async Task UpdateAsync(Guid id, PersonGroup updateModel) =>
			 await _collection.ReplaceOneAsync(x => x.Id == id, updateModel);


		public async Task UpdateAsync(List<PersonGroup> updateModel)
		{
			foreach (var item in updateModel)
			{
				await UpdateAsync(item.Id, item);
			}

		}
	}
}
