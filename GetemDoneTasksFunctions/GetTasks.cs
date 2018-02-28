using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;

namespace GetemDoneTasksFunctions
{
    public static class GetTasks
    {
        [FunctionName("GetTasks")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req, [Table("Task", Connection = "")]IQueryable<Person> inTable, TraceWriter log)
        {
            var query = from person in inTable select person;
            foreach (Person person in query)
            {
                log.Info($"Name:{person.Name}");
            }
            return req.CreateResponse(HttpStatusCode.OK, inTable.ToList());
        }

        public class Person : TableEntity
        {
            public string Name { get; set; }
        }
    }
}
