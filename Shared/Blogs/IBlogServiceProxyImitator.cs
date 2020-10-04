// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Blogs
{
    /// <summary>
    /// This proxy imitator interface is provided for the benefit of the service consumer without requiring that the BlogService implements it. A proxy imitator can be readily generated based on this interface.
    /// The BlogService implements the IDisposable interface so we make our interface inherit it as well.
    /// </summary>
    public interface IBlogServiceProxyImitator : IDisposable
    {
        BlogContext Context { get; set; }

        int Count { get; }

        // Also supported are Task, ValueTask and ValueTask<T> result types. Default method parameters must be specified in the same position as they are in the target method
        Task<Blog> Add(string url = "https://defaulturl.com");

        Task<Blog> Add(Blog blog);

        IEnumerable<Blog> Find(string term);

        // out and ref parameter modifiers are supported as well
        bool TryGet(long blogId, out Blog result);
    }
}
