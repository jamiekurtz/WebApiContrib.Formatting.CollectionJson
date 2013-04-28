﻿using System;
using WebApiContrib.Formatting.CollectionJson.Models;

namespace WebApiContrib.Formatting.CollectionJson.Infrastructure
{
    public class FriendDocumentReader : ICollectionJsonDocumentReader<Friend>
    {
        public Friend Read(WriteDocument document)
        {
            Template template = document.Template;
            var friend = new Friend();
            friend.FullName = template.Data.GetDataByName("name").Value;
            friend.Email = template.Data.GetDataByName("email").Value;
            friend.Blog = new Uri(template.Data.GetDataByName("blog").Value);
            friend.Avatar = new Uri(template.Data.GetDataByName("avatar").Value);
            return friend;
        }
    }
}