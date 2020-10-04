// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

namespace Shared.Blogs
{
    public class Blog
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public long Id { get; set; }

        public string Url { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
