using Api.Data;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Moderation
{
    public class ModerationService : IModerationService
    {
        private readonly IMapper mapper;
        private readonly YARCContext context;
        private readonly IIdentityService identity;

        public ModerationService(
            IMapper mapper,
            YARCContext context,
            IIdentityService identity)
        {
            this.mapper = mapper;
            this.context = context;
            this.identity = identity;
        }

        public async Task<bool> IsOwner(int forumId)
        {
            var userId = this.identity.GetIdentity().Id;

            var isOwner = await this.context
                .ForumOwners
                .AnyAsync(M => M.OwnerId == userId && M.ForumId == forumId);

            return isOwner;
        }

        public async Task<bool> IsModerator(int forumId)
        {
            var userId = this.identity.GetIdentity().Id;

            var isModerator = await this.context
                .ForumModerator
                .AnyAsync(M => M.ModeratorId == userId && M.ForumId == forumId);

            return isModerator;
        }
    }
}
