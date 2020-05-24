// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using global::Shared.Blogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using WebApp.Blogs;

namespace WebApp.Pages
{
    public class BlogsBase : ComponentBase
    {
        [Inject] public IHttpContextAccessor hc { get; set; }

        [Inject] public IBlogServiceAdapter<BlogDto> blogService { get; set; }

        [Inject] public ILoggerFactory loggerFactory { get; set; }

        [Inject] public ILogger<WebApp.Pages.BlogsBase> logger { get; set; }

        public BlogDto BlogDto { get; set; } = new BlogDto();

        public async System.Threading.Tasks.Task AddNewRandomBlogAsync()
        {
            logger.Here(l => l.Entering());

            var blogDto = await blogService.Add(Guid.NewGuid().ToString());
            // Will automatically use transaction
            blogService.Context.SaveChanges();

            this.StateHasChanged();

            logger.Here(l => l.Exiting());
        }

        public async System.Threading.Tasks.Task<BlogDto> HandleValidSubmitAsync()
        {
            logger.Here(l => l.Entering(BlogDto));

            var blogDto = await blogService.Add(BlogDto);
            // Will automatically use transaction
            blogService.Context.SaveChanges();
            
            BlogDto = new BlogDto();

            this.StateHasChanged();

            logger.Here(l => l.Exiting(blogDto));
            return blogDto;
        }
    }
}
