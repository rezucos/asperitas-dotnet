using Asperitas.Api.Application.Commands.Comments;
using Asperitas.Api.Application.Commands.Posts;
using Asperitas.Api.Application.Commands.Users;
using Asperitas.Api.Application.Queries.Posts;
using FluentValidation;

namespace Asperitas.Api.Application.Validation;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(32);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(64);
    }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(32);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(64);
    }
}

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(200);

        RuleFor(x => x.Category)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(type =>
                string.Equals(type, "link", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(type, "text", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Type must be either 'link' or 'text'.");

        RuleFor(x => x.Url)
            .MaximumLength(2048);

        RuleFor(x => x.Text)
            .MaximumLength(4000);

        When(x => string.Equals(x.Type, "link", StringComparison.OrdinalIgnoreCase), () =>
        {
            RuleFor(x => x.Url)
                .NotEmpty()
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Url must be a valid absolute URL for link posts.");
        });

        When(x => string.Equals(x.Type, "text", StringComparison.OrdinalIgnoreCase), () =>
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Text is required for text posts.");
        });
    }
}

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.PostId).NotEmpty();
        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(1000);
    }
}

public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.PostId).NotEmpty();
        RuleFor(x => x.CommentId).NotEmpty();
    }
}

public class ViewPostCommandValidator : AbstractValidator<ViewPostCommand>
{
    public ViewPostCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpvotePostCommandValidator : AbstractValidator<UpvotePostCommand>
{
    public UpvotePostCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DownvotePostCommandValidator : AbstractValidator<DownvotePostCommand>
{
    public DownvotePostCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UnvotePostCommandValidator : AbstractValidator<UnvotePostCommand>
{
    public UnvotePostCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
{
    public GetPostsQueryValidator()
    {
        When(x => x.Category is not null, () =>
        {
            RuleFor(x => x.Category!)
                .NotEmpty()
                .MaximumLength(50);
        });

        When(x => x.Username is not null, () =>
        {
            RuleFor(x => x.Username!)
                .NotEmpty()
                .MaximumLength(32);
        });
    }
}
