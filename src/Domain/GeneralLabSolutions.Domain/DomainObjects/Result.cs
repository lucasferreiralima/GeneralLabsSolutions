using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLabSolutions.Domain.DomainObjects
{
    public class Result
    {
        public Result()
        {
            Errors = new List<string>();
            Data = new object();
        }

        public object Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
