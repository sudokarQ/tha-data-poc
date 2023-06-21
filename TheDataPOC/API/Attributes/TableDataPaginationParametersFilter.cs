namespace API.Attributes
{
    using System.Net;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class TableDataPaginationParametersFilter : Attribute, IResourceFilter
    {
        private const int MaxRowsNumber = 200;

        private const string ErrorMessage = "Invalid parameters. Please check the properties.";

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var tableName = (string)context.HttpContext.Request.Query["tableName"];

            int count, pageNumber;

            int.TryParse(context.HttpContext.Request.Query["count"], out count);
            int.TryParse(context.HttpContext.Request.Query["pageNumber"], out pageNumber);

            if (count == null || pageNumber == null || string.IsNullOrWhiteSpace(tableName) || count <= 0 || pageNumber <= 0 ||
                count > MaxRowsNumber)
            {
                var response = context.HttpContext.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new ContentResult { Content = ErrorMessage };
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context) { }
    }
}

