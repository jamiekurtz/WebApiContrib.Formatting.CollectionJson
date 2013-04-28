﻿using WebApiContrib.Formatting.CollectionJson.Models;
using System;
using System.Collections.Generic;

namespace WebApiContrib.Formatting.CollectionJson.Infrastructure
{
    public class FriendDocumentWriter : ICollectionJsonDocumentWriter<Friend>
    {
        public ReadDocument Write(IEnumerable<Friend> friends)
        {
            var document = new ReadDocument();
            var collection = document.Collection;
            collection.Version = "1.0";
            collection.Href = new Uri("http://example.org/friends/");

            collection.Links.Add(new Link { Rel = "Feed", Href = new Uri("http://example.org/friends/rss") });

            foreach (var friend in friends)
            {
                var item = new Item { Href = new Uri("http://example.org/friends/" + friend.ShortName) };
                item.Data.Add(new Data { Name = "full-name", Value = friend.FullName, Prompt = "Full Name" });
                item.Data.Add(new Data { Name = "email", Value = friend.Email, Prompt = "Email" });
                item.Links.Add(new Link { Rel = "blog", Href = friend.Blog, Prompt = "Blog" });
                item.Links.Add(new Link { Rel = "avatar", Href = friend.Avatar, Prompt = "Avatar", Render = "Image" });
                collection.Items.Add(item);
            }

            var query = new Query { Rel = "search", Href = new Uri("http://example.org/friends/search"), Prompt = "Search" };
            query.Data.Add(new Data { Name = "name" });
            collection.Queries.Add(query);

            var data = collection.Template.Data;
            data.Add(new Data { Name = "name", Prompt = "Full Name" });
            data.Add(new Data { Name = "email", Prompt = "Email" });
            data.Add(new Data { Name = "blog", Prompt = "Blog" });
            data.Add(new Data { Name = "avatar", Prompt = "Avatar" });
            return document;
        }
    }
}
