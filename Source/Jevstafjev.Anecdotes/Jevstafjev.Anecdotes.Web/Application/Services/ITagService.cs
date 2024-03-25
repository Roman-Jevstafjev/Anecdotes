using Jevstafjev.Anecdotes.Domain;

namespace Jevstafjev.Anecdotes.Web.Application.Services
{
    public interface ITagService
    {
        Task<AssignTagsResult> AssignTagsAsync(Anecdote entity, List<string> tags);
    }

    public readonly struct AssignTagsResult
    {
        public AssignTagsResult(List<string> toCreate, List<string> toDelete)
        {
            ToCreate = toCreate;
            ToDelete = toDelete;
        }

        public AssignTagsResult(Exception exception)
        {
            Exception = exception;
        }

        public List<string>? ToCreate { get; }

        public List<string>? ToDelete { get; }

        public Exception? Exception { get; }

        public bool IsSuccess => Exception is null;
    }
}
