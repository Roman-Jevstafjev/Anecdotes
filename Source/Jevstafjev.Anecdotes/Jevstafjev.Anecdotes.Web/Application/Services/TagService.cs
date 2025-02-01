using Arch.EntityFrameworkCore.UnitOfWork;
using Jevstafjev.Anecdotes.Domain;
using Microsoft.EntityFrameworkCore;

namespace Jevstafjev.Anecdotes.Web.Application.Services
{
    public class TagService(IUnitOfWork unitOfWork) : ITagService
    {
        public async Task<TagsAdditionResult> AddTagsAsync(Anecdote anecdote, List<string> tags)
        {
            if (tags.Count > 8)
            {
                return new TagsAdditionResult(new ArgumentException("Anecdote must have less than 8 tags"));
            }

            var anecdoteRepository = unitOfWork.GetRepository<Anecdote>();
            var tagRepository = unitOfWork.GetRepository<Tag>();

            var currentTags = tags.ToList();
            var oldTags = tagRepository.GetAll()
                        .AsTracking()
                        .Where(x => x.Anecdotes!.Select(x => x.Id).Contains(anecdote.Id))
                        .Select(x => x.Name.ToLower())
                        .ToList();

            var mask = currentTags.Intersect(oldTags);
            var toDelete = oldTags.Except(currentTags).ToList();
            var toCreate = currentTags.Except(mask).ToList();

            foreach (var tagName in toDelete)
            {
                var tag = await tagRepository.GetFirstOrDefaultAsync(
                    predicate: x => x.Name.ToLower() == tagName,
                    disableTracking: false);
                if (tag is null)
                {
                    continue;
                }

                var used = anecdoteRepository.GetAll()
                    .AsTracking()
                    .Where(x => x.Tags!.Select(t => t.Name).Contains(tag.Name))
                    .ToList();
                if (used.Count is 1)
                {
                    tagRepository.Delete(tag);
                }
            }

            if (anecdote.Tags is null)
            {
                anecdote.Tags = new List<Tag>();
            }

            foreach (var tagName in toCreate)
            {
                var tag = await tagRepository.GetFirstOrDefaultAsync(
                    predicate: x => x.Name.ToLower() == tagName,
                    disableTracking: false);
                if (tag is not null)
                {
                    anecdote.Tags!.Add(tag);
                    continue;
                }

                var newTag = new Tag
                {
                    Name = tagName.Trim().ToLower()
                };

                await tagRepository.InsertAsync(newTag);
                anecdote.Tags!.Add(newTag);
            }

            return new TagsAdditionResult(toCreate, toDelete);
        }
    }
}
