using MediatR;
using Asperitas.Api.Application.Common.Repositories;

namespace Asperitas.Api.Application.Commands.Posts;

public record DeletePostCommand(Guid Id) : IRequest;

public class DeletePostHandler(IPostRepository postRepository) : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        await postRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
