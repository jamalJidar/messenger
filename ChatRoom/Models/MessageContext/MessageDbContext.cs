namespace ChatRoom.Models.MessageContext
{
	public class MessageDbContext
	{
		public string Name { get; init; } = null!;

		public string ConnectionString => $"mongodb://localhost:27017";

		public string DatabaseName { get; set; } = null!;

		public string PersonCollectionName { get; set; } = null!;
		public string MessageCollectionName { get; set; } = null!;
		public string GroupCollectionName { get; set; } = null!;
		public string PersonGroupCollectionName { get; set; } = null!;
		public string GroupMessageCollectionName { get; set; } = null!;
		public string MessageDocumentCollectionName { get; set; } = null!;
		public string UserSeenCollectionName { get; set; } = null!;
		public string OnlineCollectionName { get; set; } = null!;


        public string ContactCollectionName { get; set; } = null!;
		public string MessagePaaswordCollectionName { get; set; } = null!;



	}
}
