using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using GetemDoneTasksFunctions.Entities;
using System.Security.Claims;
using System.Linq;

namespace GetemDoneTasksFunctions
{
    public static class AddTodo
    {
        [FunctionName("AddTodo")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req, 
                                                          [Table("Todo", Connection = "TodosConnection")]ICollector<Todo> todos, 
                                                          TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            string description = data?.Description;

            if (description == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a description in the request body");
            }

            var userId = ClaimsPrincipal.Current.Claims
                    .First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;


            todos.Add(new Todo()
            {
                PartitionKey = "Todos",
                RowKey = Guid.NewGuid().ToString(),
                Description = description,
                UserId = userId
            });
            return req.CreateResponse(HttpStatusCode.Created);
        }
    }
}
