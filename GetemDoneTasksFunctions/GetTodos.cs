using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.Security.Claims;
using GetemDoneTasksFunctions.Entities;

namespace GetemDoneTasksFunctions
{
    public static class GetTodos
    {
        [FunctionName("GetTodos")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req, 
                                              [Table("Todo", Connection = "TodosConnection")]IQueryable<Todo> todos, 
                                              TraceWriter log)
        {
            var userId = ClaimsPrincipal.Current.Claims
                    .First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            var userTodos = todos.Where(t => t.UserId == userId);

            var response = req.CreateResponse(HttpStatusCode.OK, userTodos.Select(t=>new Model.Todo() {
                TodoId = t.RowKey,
                Description = t.Description,
                UserId = t.UserId
            }).ToList());
            return response;
        }

    }
}
