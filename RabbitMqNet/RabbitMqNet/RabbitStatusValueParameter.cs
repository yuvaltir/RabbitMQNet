using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqNet
{
    /// <summary>
    /// Class represeting a value parameter, e.g. total memory
    /// </summary>
    public class RabbitStatusValueParameter : RabbitStatusParameterBase
    {
        private List<string> _values = new List<string>();

        public List<string> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public override string Value 
        { 
            get 
            { 
                if(Values.Any())
                {
                    return Values.Aggregate((i, j) => i + Delimeter + j).TrimEnd();
                }
                return string.Empty;
            }
        }
        
        public override string this[string name]
        {
            get
            {
                var splt = name.Split(new[] { '.' });

                if (splt.Count() == 1 && this.Name == splt.First())
                {
                    return this.Value;
                }
                return string.Empty;
            }
        }
    }
}
