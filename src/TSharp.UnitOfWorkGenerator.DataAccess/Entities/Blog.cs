﻿using TSharp.UnitOfWorkGenerator.EFCore.Utils;

namespace TSharp.UnitOfWorkGenerator.DataAccess.Entities;

[UoWGenerateRepository]
public partial class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; } = new();


}
