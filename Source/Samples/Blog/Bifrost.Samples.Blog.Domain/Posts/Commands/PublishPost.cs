﻿using System;
using Bifrost.Commands;

namespace Bifrost.Samples.Blog.Domain.Posts.Commands
{
    public class PublishPost : ICommand
    {
        public Guid Id { get; set; }
    }
}