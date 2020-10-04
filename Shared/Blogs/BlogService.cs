// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Blogs
{
    public class BlogService : IDisposable
    {
        public BlogService(BlogContext context, ILogger<BlogService>? logger = null)
        {
            if (logger != null) _logger = logger;
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private ILogger _logger = NullLogger.Instance;

        public BlogContext Context { get; set; }

        public int Count
        {
            get
            {
                _logger.Here(l => l.Entering());

                var count = Context.Blogs.Count();

                _logger.Here(l => l.Exiting(count));
                return count;
            }
        }

        public async Task<Blog> Add(string url = "https://defaulturl.com")
        {
            _logger.Here(l => l.Entering(url));

            await Task.Yield();

            var blog = new Blog { Url = url };
            Context.Blogs.Add(blog);
            Context.SaveChanges();

            _logger.Here(l => l.Exiting(blog));
            return blog;
        }

        public async Task<Blog> Add(Blog blog)
        {
            _logger.Here(l => l.Entering(blog));

            await Task.Yield();

            Context.Blogs.Add(blog);
            Context.SaveChanges();

            _logger.Here(l => l.Exiting(blog));
            return blog;
        }

        public IEnumerable<Blog> Find(string term)
        {
            _logger.Here(l => l.Entering(term));

            var tmp = Context.Blogs
                .Where(b => b.Url.Contains(term))
                .OrderBy(b => b.Url)
                .ToList();

            _logger.Here(l => l.Exiting(tmp));
            return tmp;
        }

        public bool TryGet(long blogId, out Blog result)
        {
            result = Context.Blogs.FirstOrDefault(b => b.Id == blogId);
            return result != null;
        }

        private bool disposedValue = false;

        public void Dispose()
        {
            _logger.Here(l => l.Entering());

            _logger.Here(l => l.LogDebug("{@0}", disposedValue));

            if (!disposedValue)
            {
                disposedValue = true;

                // We should explicitely manage our explicitly provided connection
                Context?.Database.CloseConnection();
                Context?.Dispose();
                Context = null!;
            }

            _logger.Here(l => l.Exiting());
        }
    }
}
