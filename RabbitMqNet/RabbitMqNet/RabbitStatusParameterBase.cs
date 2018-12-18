using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqNet
{
    /// <summary>
    /// Abstract class representing a status parameter
    /// </summary>
    public abstract class RabbitStatusParameterBase
    {
        private string delimeter = "|";

        public string Delimeter
        {
            get { return delimeter; }
            set { delimeter = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public abstract string Value { get; }

        public abstract string this[string name] { get; }
    }
}
