using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services.MessageDocumentService
{
	public interface IMessageDocumentService:ICrudService<MessageDocument>
	{}
	public class MessageDocumentService : BaseService<MessageDocument>, IMessageDocumentService
	{
		public MessageDocumentService(IOptions<MessageDbContext> mongoDatabaseSettings) : base(mongoDatabaseSettings)
		{
		}


		public async Task<MessageDocument?> GetAsync(Guid id) =>
		  await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task RemoveAsync(Guid id) =>
		  await _collection.DeleteOneAsync(x => x.Id == id);

		public async Task RemoveAsync(List<Guid> id) =>
		   await _collection.DeleteManyAsync(x => id.Contains(x.Id));

		public async Task UpdateAsync(Guid id, MessageDocument updateModel) =>
			 await _collection.ReplaceOneAsync(x => x.Id == id, updateModel);


		public async Task UpdateAsync(List<MessageDocument> updateModel)
		{
			foreach (var item in updateModel)
			{
				await UpdateAsync(item.Id, item);
			}

		}
	   
	
	
	}
}
