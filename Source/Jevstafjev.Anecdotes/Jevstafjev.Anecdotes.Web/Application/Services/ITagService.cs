using Jevstafjev.Anecdotes.Domain;

namespace Jevstafjev.Anecdotes.Web.Application.Services
{
    public interface ITagService
    {
        Task<TagsAdditionResult> AddTagsAsync(Anecdote entity, List<string> tags);
    }

    public readonly struct TagsAdditionResult
    {
        public TagsAdditionResult(List<string> toCreate, List<string> toDelete)
        {
            ToCreate = toCreate;
            ToDelete = toDelete;
        }

        public TagsAdditionResult(Exception exception)
        {
            Exception = exception;
        }

        public List<string>? ToCreate { get; }

        public List<string>? ToDelete { get; }

        public Exception? Exception { get; }

        public bool IsSuccess => Exception is null;
    }
}
