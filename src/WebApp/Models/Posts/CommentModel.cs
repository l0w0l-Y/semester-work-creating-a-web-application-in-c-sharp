﻿namespace WebApp.Models.Posts
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}