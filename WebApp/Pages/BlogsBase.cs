// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using global::Shared.Blogs;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using WebApp.Blogs;

namespace WebApp.Pages
{
    public class BlogsBase : ComponentBase
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Inject] public IBlogServiceAdapter<BlogDto> BlogService { get; set; }

        [Inject] private ILogger<BlogsBase> Logger { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BlogDto BlogDto { get; set; } = new BlogDto();

        public async System.Threading.Tasks.Task AddNewRandomBlogAsync()
        {
            Logger.Here(l => l.Entering());

            var blogDto = await BlogService.Add(Guid.NewGuid().ToString());
            // Will automatically use transaction
            BlogService.Context.SaveChanges();

            this.StateHasChanged();

            Logger.Here(l => l.Exiting());
        }

        public async System.Threading.Tasks.Task<BlogDto> HandleValidSubmitAsync()
        {
            Logger.Here(l => l.Entering(BlogDto));

            var blogDto = await BlogService.Add(BlogDto);
            // Will automatically use transaction
            BlogService.Context.SaveChanges();

            BlogDto = new BlogDto();

            this.StateHasChanged();

            Logger.Here(l => l.Exiting(blogDto));
            return blogDto;
        }
    }
}
