namespace Api.Services.BackgroundJobs
{
    public interface ICheckPostToxicity
    {
        Task Check(int postId);
    }
}