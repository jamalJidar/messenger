using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using ChatRoom.Services.MessageService;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services.PersonService
{
	public interface IPersonService : ICrudService<Person>
	{
		public Task<Person> GetByUserName(string userName);
		public Task<List< Person>> SearchByUserName(string userName);
		public Task<List<Person>> GetAsync(List<Guid> Ids);
		public Task<Person?> GetByPhoneNumber(string phone);
	}
	public class PersonService : BaseService<Person>, IPersonService
	{


		public PersonService(IOptions<MessageDbContext> mongoDatabaseSettings) : base(mongoDatabaseSettings)
		{

		}

		public async Task<Person?> GetAsync(Guid id) =>
		 await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
		public async Task<List<Person>> GetAsync(List<Guid> Ids) =>
		   await _collection.Find(x => Ids.Contains(x.Id)).ToListAsync();

		public async Task<Person?> GetByPhoneNumber(string phone)=>
			   await _collection.Find(x=>x.PhoneNumber.Trim()== phone.Trim()).FirstOrDefaultAsync();

		public async Task<Person> GetByUserName(string userName) =>
		 await _collection.Find(x => x.UserName.Trim().ToLower() == userName.Trim().ToLower()).FirstOrDefaultAsync();


		public async Task RemoveAsync(Guid id) =>
		  await _collection.DeleteOneAsync(x => x.Id == id);

		public async Task RemoveAsync(List<Guid> id) =>
		   await _collection.DeleteManyAsync(x => id.Contains(x.Id));

		public async Task<List< Person>> SearchByUserName(string userName)=>
		 await _collection.Find(x => x.UserName.Contains( userName.ToLower())).ToListAsync();


		public async Task UpdateAsync(Guid id, Person updateModel) =>
			 await _collection.ReplaceOneAsync(x => x.Id == id, updateModel);


		public async Task UpdateAsync(List<Person> updateModel)
		{
			foreach (var item in updateModel)
			{
				await UpdateAsync(item.Id, item);
			}

		}


	}

}
