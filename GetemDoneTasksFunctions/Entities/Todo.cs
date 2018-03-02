using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace GetemDoneTasksFunctions.Entities
{
    public class Todo : TableEntity
    {
        public string UserId { get; set; }
        public string Description { get; set; }
    }
}
