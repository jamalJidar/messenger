using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatRoom.Services.ContactService
{
    public interface IContactService : ICrudService<Contact>
    {
        public Task<List<Contact>?> GetListAsync(Guid userId);
        public Task<Contact?> GeSingleAsync(Guid owner, Guid subc);

      
    }

    public class ContactService : BaseService<Contact>, IContactService
    {
        public ContactService(IOptions<MessageDbContext> mongoDatabaseSettings) : base(mongoDatabaseSettings)
        { }

        public async Task<Contact?> GeSingleAsync(Guid owner, Guid subc) =>
          await _collection.Find(x => x.Owner == owner && x.Subscribers == subc).FirstOrDefaultAsync();

        public async Task<Contact?> GetAsync(Guid id) =>
          await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<List<Contact>?> GetListAsync(Guid userId) =>
          await _collection.Find(x => x.Owner == userId).ToListAsync();
        public async Task RemoveAsync(Guid id) =>
          await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task RemoveAsync(List<Guid> id) =>
           await _collection.DeleteManyAsync(x => id.Contains(x.Id));

        public async Task UpdateAsync(Guid id, Contact updateModel) =>
             await _collection.ReplaceOneAsync(x => x.Id == id, updateModel);


        public async Task UpdateAsync(List<Contact> updateModel)
        {

            
            foreach (var item in updateModel)
            {
                await UpdateAsync(item.Id, item);
            }

        }



    }


}
