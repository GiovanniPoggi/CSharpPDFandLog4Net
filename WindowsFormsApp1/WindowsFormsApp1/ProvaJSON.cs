using System;
using Spire.Pdf;
using System.Drawing;
using System.Threading;
using Spire.Pdf.HtmlConverter;

namespace PDF
{
    class ProvaJSON
    {
        public String name;
        public String surname;
        public int years;
        public String city;

        public override String ToString()
        {
            return "The person's name is: " + name + " and the surname is: " + surname + ", He/She has: " + years + " years old and He/She lives in: " + city;
        }
    }
}
