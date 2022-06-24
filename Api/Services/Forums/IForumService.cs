﻿using Api.Models;

namespace Api.Services.Forums
{
    public interface IForumService
    {
        Task<IdModel<int>> Create(EditForumModel model);
        Task<SimilarForumModel[]> FindSimilarForums(int forumId);
        Task<ForumModel> Get(int forumId);
        Task<ForumPostGuideLinesModel> GetForumGuidelines(int forumId);
        Task<bool> Remove(int forumId);
        Task<bool> SlugAlreadyTaken(string slug, int? forumId);
        Task<KeyValueModel[]> SuggestTopics(string queryText);
        Task<KeyValueModel[]> SuggestUsers(string queryText);
        Task<bool> Update(int forumId, EditForumModel model);
        Task<bool> VerifyCredentials(int forumId);
    }
}