using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG2500_Final.Models
{
    public partial class Rating
    {
        public string AvgFormatted { get
        {
                return "Avg Rating: " + AverageRating;
        } }
    }
}
