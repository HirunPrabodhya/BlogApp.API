using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.IRepository
{
    public interface IPost
    {
        Task<List<Post>> GetAllPost();
        Task<PostDetailDto?> GetPostDB(int id);
        Task<MessagePostDto> AddPost(PostDto post);
        Task<string> EditPost(int id, PostDto post);
        Task<string> DeletePost(int id);
        Task<List<Post?>> GetPostByCategoryId(int categoryId);
        Task<List<Post?>> GetPostByUserId(int userId);
      

    }
}
