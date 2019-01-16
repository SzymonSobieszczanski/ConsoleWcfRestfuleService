using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumoxTest.Common
{
  public static  class Parser
    {
        public static long ParseToLong(string value)
        { 
            long result;
            long.TryParse(value, out result);
            return result;
        }

    }
}
