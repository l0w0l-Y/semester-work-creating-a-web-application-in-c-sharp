﻿using System.Collections.Generic;

namespace WebApp.Models
{
    public interface ICreator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public IEnumerable<TagModel> Tags { get; set; }
    }
}