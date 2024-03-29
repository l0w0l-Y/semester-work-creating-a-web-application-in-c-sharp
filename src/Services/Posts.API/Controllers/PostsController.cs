﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Posts.API.DTOs;
using Posts.API.Entities;

namespace Posts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : Controller
    {
        private readonly PostsDbContext _context;
        
        public PostsController(PostsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/Posts/Get")]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            var posts = await _context
                .Post
                .ToListAsync();
            return posts.Count == 0 ? null : posts;
        }
        
        [HttpGet]
        [Route("/Posts/Get/{id:int}")]
        public async Task<ActionResult<Post>> Get(int id)
            =>  await _context.Post.FirstOrDefaultAsync(p => p.Id == id);
        
        [HttpGet]
        [Route("/Posts/GetByUser/{userId:int}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetByUser(int userId)
        {
            var posts = await _context
                .Post
                .Where(p => p.UserId == userId)
                .ToListAsync();
            return posts.Count == 0 ? null : posts;
        }

        [HttpGet]
        [Route("/Posts/GetByProject/{projectId:int}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetByProject(int projectId)
        {
            var posts = await _context
                .Post
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();
            return posts.Count == 0 ? null : posts;
        }

        [HttpGet]
        [Route("/Posts/GetByUserAndProject")]
        public async Task<ActionResult<IEnumerable<Post>>> GetByUserAndProject(int userId, int projectId)
        {
            var posts = await _context
                .Post
                .Where(p => p.UserId == userId && p.ProjectId == projectId)
                .ToListAsync();
            return posts.Count == 0 ? null : posts;
        }

        [HttpPost]
        [Route("/Posts/Create")]
        public async Task<Post> Create(Post post)
        {
            await _context.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }
        
        [HttpPost]
        [Route("/Posts/UpdateRequiredType")]
        public async Task UpdateRequiredType(RequiredTypeDto dto)
        {
            var post = await _context.Post.FirstAsync(p => p.Id == dto.PostId);
            post.RequiredSubscriptionType = dto.PriceType;
            await _context.SaveChangesAsync();
        }
        
        [HttpPost]
        [Route("/Posts/UpdateText")]
        public async Task UpdateText(TextDto dto)
        {
            var post = await _context.Post.FirstAsync(p => p.Id == dto.PostId);
            post.Text = dto.Text;
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        [Route("/Posts/Delete")]
        public async Task Delete(int id)
        {
            var post = await _context
                .Post
                .Include(p => p.Comments)
                .FirstAsync(p => p.Id == id);
            post.Comments.ForEach(c => _context.Comment.Remove(c));
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}