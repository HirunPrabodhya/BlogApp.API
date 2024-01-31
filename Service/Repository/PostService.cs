using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Service.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
    public class PostService : IPost
    {
        // constructor dependency
        private readonly PostDBContext _postDBContext;

        public PostService(PostDBContext postDBContext)
        {
            _postDBContext = postDBContext;
        }
        //get all Post
        public async Task<List<Post>> GetAllPost()
        {
            return await _postDBContext.posts.ToListAsync();
        }

        // get specific post
        public async Task<PostDetailDto?> GetPostDB(int id)
        {
            var getPost = await _postDBContext.posts.SingleOrDefaultAsync(p => p.Id == id);
            if (getPost == null)
            {
                return null;
            }
            var postData = await (from post in _postDBContext.posts
                                  join user in _postDBContext.users
                                  on post.UserId equals user.Id
                                  where post.Id == id
                                  select new PostDetailDto
                                  {
                                      Id = post.Id,
                                      Title = post.Title,
                                      Summary = post.Summary,
                                      Content = post.Content,
                                      Thumbnail = post.Thumbnail,
                                      PublishDate = post.PublishDate,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                  }).SingleAsync();

            return postData;
        }
        // add post
        public async Task<MessagePostDto> AddPost(PostDto post)
        {
            if (post == null)
            {
                return new MessagePostDto
                {
                    Message = "post data is incompleted",
                    NotifiedPost = null
                };
                
            }
            var postdb = new Post
            {
                Title = post.Title,
                Content = post.Content,
                Summary = post.Summary,
                Thumbnail = post.Thumbnail,
                PublishDate = post.PublishDate,
                UserId = post.UserId,
                CategoryId = post.CategoryId
            };
            await _postDBContext.posts.AddAsync(postdb);
            await _postDBContext.SaveChangesAsync();

            return new MessagePostDto
            {
                Message = "post is added",
                NotifiedPost = new PostNotificationDto
                {
                    Title = postdb.Title,
                    Thumbnail = postdb.Thumbnail,
                    
                }
			};
			
        }
        // update post
        public async Task<string> EditPost(int id, PostDto post)
        {
            var existPost = await _postDBContext.posts.FirstOrDefaultAsync(x => x.Id == id);
            if (existPost == null)
            {
                return id + "st post does not exist";
            }
            existPost.Title = post.Title;
            existPost.Content = post.Content;
            existPost.Summary = post.Summary;
            existPost.Thumbnail = post.Thumbnail;
            existPost.PublishDate = post.PublishDate;
            existPost.UpdateDate = DateTime.Now;
            existPost.UserId = post.UserId;
            existPost.CategoryId = post.CategoryId;

            await _postDBContext.SaveChangesAsync();
            return id + "st Post is updated";
        }
        // delete post
        public async Task<string> DeletePost(int id)
        {
            var post = await _postDBContext.posts.FirstOrDefaultAsync(post => post.Id == id);
            if (post == null)
            {
                return id + "st Post does not exist";
            }
            _postDBContext.posts.Remove(post);
            await _postDBContext.SaveChangesAsync();
            return "post was deleted";
        }

        public async Task<List<Post?>> GetPostByCategoryId(int categoryId)
        {
            var posts = await (from post in _postDBContext.posts
                               where post.CategoryId == categoryId
                               select post
                            ).ToListAsync();
            return posts;
        }

        public async Task<List<Post?>> GetPostByUserId(int userId)
        {
            var posts = await (from post in _postDBContext.posts
                               where post.UserId == userId
                               select post
                               ).ToListAsync();
            return posts;
        }

		
	}
}
