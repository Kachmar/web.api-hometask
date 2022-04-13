using System.Collections.Generic;
using System.Linq;

namespace Services.Validators
{
    public class ValidationResponse
    {
        public bool HasErrors => Errors.Any();

        public Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        public void AddError(string key, string errorMessage)
        {
            Errors.Add(key, errorMessage);
        }
    }

    public class ValidationResponse<T> : ValidationResponse where T : class
    {
        public ValidationResponse(string key, string errorMessage)
        {
            AddError(key, errorMessage);
        }

        public ValidationResponse(T result)
        {
            Result = result;
        }

        public T Result { get; }
    }
}
