using EAITMApp.Application.UseCases.Commands.TaskCMD;
using FluentValidation;

namespace EAITMApp.Application.Validators.Task
{
    public class AddTodoTaskCommandValidator : AbstractValidator<AddTodoTaskCommand>
    {
        public AddTodoTaskCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required for the task.");
            RuleFor(x => x.Title).MinimumLength(3).WithMessage("Title must be at least 3 characters.");
            RuleFor(x => x.Title).MaximumLength(250).WithMessage("Title must not exceed 250 characters.");

            RuleFor(x => x.Description).MinimumLength(3).WithMessage("Description must be at least 3 characters.");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
