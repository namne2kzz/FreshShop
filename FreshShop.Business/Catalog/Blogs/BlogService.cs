using FreshShop.Business.Common;
using FreshShop.Data.EF;
using FreshShop.Data.Entities;
using FreshShop.Utilities.Exceptions;
using FreshShop.ViewModels.Catalog.Blog;
using FreshShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Blogs
{
    public class BlogService : IBlogService
    {
        private readonly FreshShopDbContext _context;
        private readonly IStorageService _storageService;

        public BlogService(FreshShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<bool> ChangeStatus(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if(blog==null) throw new FreshShopException("Không tìm thấy bài viết");
            blog.Status = !blog.Status;
            await _context.SaveChangesAsync();
            return blog.Status;

        }

        public async Task<int> Create(BlogCreateRequest request)
        {
            var blog = new Blog()
            {
                Title = request.Title,
                Content = request.Content,
                Image = await this.SaveFile(request.ThumbnailImage),
                CreatedDate = DateTime.Now,
                Status = true,
            };
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return blog.ID;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return new ApiErrorResult<bool>("Không tìm thấy bài viết hợp lệ");
            _context.Blogs.Remove(blog);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                await _storageService.DeleteFileAsync(blog.Image);
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Xóa không thành công");

        }

        public async Task<ApiResult<PagedResult<BlogViewModel>>> GetAllPaging(GetBlogPagingRequest request)
        {
            var blogs = from a in _context.Blogs
                        select new { a };

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                blogs = blogs.Where(x => x.a.Title.Contains(request.Keyword) || x.a.Content.Contains(request.Keyword));
            }
            int totalRow = await blogs.CountAsync();

            var data = await blogs.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new BlogViewModel()
                {
                    Id=x.a.ID,
                    Title=x.a.Title,
                    Content=x.a.Content,
                    ImagePath=x.a.Image,
                    CreatedDate=x.a.CreatedDate,
                    Status=x.a.Status
                }).ToListAsync();

            var pageResult = new PagedResult<BlogViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };

            return new ApiSuccessResult<PagedResult<BlogViewModel>>(pageResult);
        }

        public async Task<ApiResult<BlogViewModel>> GetById(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return new ApiErrorResult<BlogViewModel>("Không tìm thấy bài viết hợp lệ");
            var blogViewModel = new BlogViewModel()
            {
                Id = blog.ID,
                Title = blog.Title,
                Content = blog.Content,
                ImagePath = blog.Image,
                CreatedDate = blog.CreatedDate,
                Status = blog.Status
            };

            return new ApiSuccessResult<BlogViewModel>(blogViewModel);

        }

        public async Task<ApiResult<bool>> Update(BlogUpdateRequest request)
        {
            var blog = await _context.Blogs.FindAsync(request.Id);
            if (blog == null) return new ApiErrorResult<bool>("Không tìm thấy bài viết hợp lệ");
            blog.Title = request.Title;
            blog.Content = request.Content;
            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        public async Task<List<BlogViewModel>> GetAllLatest()
        {
            var blogs = await _context.Blogs.OrderByDescending(x => x.CreatedDate).Take(3)
                .Select(x => new BlogViewModel()
                {
                    Id=x.ID,
                    Content=x.Content,
                    Title=x.Title,
                    ImagePath=x.Image,
                    CreatedDate=x.CreatedDate,
                    Status=x.Status

                }).ToListAsync();

            return blogs;

        }
    }
}
