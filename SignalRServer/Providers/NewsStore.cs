using System;
using System.Collections.Generic;
using System.Linq;
using SignalRServer.DTO;

namespace SignalRServer.Providers
{
    public class NewsStore
    {
        private readonly NewsContext _newsContext;

        public NewsStore(NewsContext newsContext)
        {
            _newsContext = newsContext;
        }

        public void AddGroup(string group)
        {
            _newsContext.NewsGroups.Add(new Equity
            {
                Name = group
            });
            _newsContext.SaveChanges();
        }

        public bool GroupExists(string group)
        {
            var item = _newsContext.NewsGroups.FirstOrDefault(t => t.Name == group);
            if (item == null)
            {
                return false;
            }

            return true;
        }

        public void CreateNewItem(NewsItem item)
        {
            if (GroupExists(item.Equity))
            {
                _newsContext.NewsItemEntities.Add(new NewsItemEntity
                {
                    Headline = item.Headline,
                    Source = item.Source,
                    Equity = item.Equity,
                    NewsText = item.NewsText
                });
                _newsContext.SaveChanges();
            }
            else
            {
                throw new Exception("group does not exist");
            }
        }

        public IEnumerable<NewsItem> GetAllNewsItems(string group)
        {
            return _newsContext.NewsItemEntities.Where(item => item.Equity == group).Select(z =>
                new NewsItem
                {
                    Source = z.Source,
                    Headline = z.Headline,
                    Equity = z.Equity,
                    NewsText = z.NewsText
                });
        }

        public List<string> GetAllGroups()
        {
            return _newsContext.NewsGroups.Select(t => t.Name).ToList();
        }
    }
}