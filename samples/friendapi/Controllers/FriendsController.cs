using System.Collections.Generic;
using System.Net.Http;
using WebApiContrib.Formatting.CollectionJson.Models;

namespace WebApiContrib.Formatting.CollectionJson.Controllers
{
    public class FriendsController : CollectionJsonController<Friend>
    {
        private readonly IFriendRepository _repo;

        public FriendsController(IFriendRepository repo, ICollectionJsonDocumentWriter<Friend> builder,
                                 ICollectionJsonDocumentReader<Friend> transformer)
            : base(builder, transformer)
        {
            _repo = repo;
        }

        protected override int Create(Friend friend, HttpResponseMessage response)
        {
            return _repo.Add(friend);
        }

        protected override IEnumerable<Friend> Read(HttpResponseMessage response)
        {
            return _repo.GetAll();
        }

        protected override Friend Read(int id, HttpResponseMessage response)
        {
            return _repo.Get(id);
        }

        protected override Friend Update(int id, Friend friend, HttpResponseMessage response)
        {
            friend.Id = id;
            _repo.Update(friend);
            return friend;
        }

        protected override void Delete(int id, HttpResponseMessage response)
        {
            _repo.Remove(id);
        }
    }
}