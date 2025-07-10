namespace FoodFlow.Abstractions
{
    public static class ResultExtenstions
    {
        public static ObjectResult ToProblem(this Result result, int statusCode)
        {

            if (result.IsSuccess)
                throw new InvalidOperationException("Cannot convert a successful result to a Problem.");

            var problem = Results.Problem(statusCode: statusCode);
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
             {
                 {
                    "error",new []{result.Error }
                 }
             };
            return new ObjectResult(problemDetails);
        }
    }
}
