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
    /// This adapter interface is provided for the benefit of the service consumer without requiring that the BlogService implements it. An adapter can be readily generated based on this interface.
    /// The BlogService implements the IDisposable interface so we make our interface inherit it as well.
    /// </summary>
    /// <typeparam name="T">The type of the blog data transfer object.</typeparam>
    public interface IBlogServiceAdapter<T>: IDisposable
    {
        BlogContext Context { get; set; }

        int Count { get; }

        // Also supported are Task, ValueTask and ValueTask<T> result types. Default adapter method parameters must be specified in the same position as they are in the target method
        Task<T> Add(string url = "https://defaulturl.com");

        Task<T> Add(T blog);

        IEnumerable<T> Find(string term);

        // out and ref parameter modifiers are supported as well
        bool TryGet(long blogId, out Blog result);
    }
}
