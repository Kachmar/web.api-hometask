using Models.Models;
using Services.Validators;

namespace Services.Abstraction
{
    public interface IValidator<T> where T : class
    {
        ValidationResponse<T> Validate(T model);
    }

    public class CourseValidator : IValidator<Course>
    {
        public ValidationResponse<Course> Validate(Course model)
        {
            throw new System.NotImplementedException();
        }
    }
}
