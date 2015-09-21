using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegster.Models
{
    public static class Extensions
    {
        public static bool IsEmpty(this IEnumerable src)
        {
            foreach (var _ in src) { return false; }
            return true;
        }
    }
}
