﻿using BookHeaven.Domain.Abstractions.Messaging;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Features.Authors;

public static class GetAuthor
{
    public sealed record Filter
    {
        public Guid? AuthorId { get; init; }
        public string? Name { get; init; }
        public Guid ProfileId { get; init; }
        public bool IncludeBooks { get; init; }
    }

    public sealed record Query(Filter Request): IQuery<Author>;

    internal class Handler(IDbContextFactory<DatabaseContext> dbContextFactory) : IQueryHandler<Query, Author>
    {
        public async Task<Result<Author>> Handle(Query query, CancellationToken cancellationToken)
        {
            if(query.Request.AuthorId == null && query.Request.Name == null)
            {
                return new Error("Error", "You must provide either an AuthorId or a Name");
            }
        
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            try
            {
                var dbQuery = context.Authors.Where(x => 
                    (query.Request.AuthorId != null && x.AuthorId == query.Request.AuthorId) || 
                    (query.Request.Name != null && x.Name!.ToUpper() == query.Request.Name.ToUpper()));
            
                if (query.Request.IncludeBooks)
                {
                    dbQuery = dbQuery
                        .Include(a => a.Books)
                        .ThenInclude(b => b.Series)
                        .Include(a => a.Books)
                        .ThenInclude(b => b.Progresses.Where(bp => bp.ProfileId == query.Request.ProfileId));
                }

                var author = await dbQuery.FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
                return author != null ? author : new Error("Error", "Author not found");
            }
            catch (Exception e)
            {
                return new Error("Error", e.Message);
            }
        }
    }
}