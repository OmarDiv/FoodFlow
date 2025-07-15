namespace FoodFlow.Abstractions
{
    public static class ResultExtenstions
    {
        public static ObjectResult ToProblem(this Result result)
        {

            if (result.IsSuccess)
                throw new InvalidOperationException("Cannot convert a successful result to a Problem.");

            var problem = Results.Problem(statusCode: result.Error.StatusCode);
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
             {
                 {
                    "error",new []
                    {
                        result.Error.Code,
                        result.Error.Description
                    }
                 }
             };
            return new ObjectResult(problemDetails);
        }
    }
}
