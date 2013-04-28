using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiContrib.Formatting.CollectionJson.Models
{
    public class FakeFriendRepository : IFriendRepository
    {
        private static readonly List<Friend> Friends = new List<Friend>();
        private static int _id = 1;

        static FakeFriendRepository()
        {
            Friends.Add(new Friend
                {
                    Id = _id++,
                    FullName = "J. Doe",
                    Email = "jdoe@example.org",
                    Blog = new Uri("http://examples.org/blogs/jdoe"),
                    Avatar = new Uri("http://examples.org/images/jode")
                });
            Friends.Add(new Friend
                {
                    Id = _id++,
                    FullName = "M. Smith",
                    Email = "msmith@example.org",
                    Blog = new Uri("http://examples.org/blogs/msmith"),
                    Avatar = new Uri("http://examples.org/images/msmith")
                });
            Friends.Add(new Friend
                {
                    Id = _id++,
                    FullName = "R. Williams",
                    Email = "rwilliams@example.org",
                    Blog = new Uri("http://examples.org/blogs/rwilliams"),
                    Avatar = new Uri("http://examples.org/images/rwilliams")
                });
        }

        public IEnumerable<Friend> GetAll()
        {
            return Friends;
        }

        public Friend Get(int id)
        {
            return Friends.FirstOrDefault(f => f.Id == id);
        }

        public int Add(Friend friend)
        {
            friend.Id = _id++;
            Friends.Add(friend);
            return friend.Id;
        }

        public void Remove(int id)
        {
            Friend friend = Get(id);
            Friends.Remove(friend);
        }

        public void Update(Friend friend)
        {
            Friend existingFriend = Get(friend.Id);
            existingFriend.Blog = friend.Blog;
            existingFriend.Avatar = friend.Avatar;
            existingFriend.Email = friend.Email;
            existingFriend.FullName = friend.FullName;
        }
    }
}