using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqNet
{
    /// <summary>
    /// Class representing a parameter which contains a collection of other base parameters, e.g. memory
    /// </summary>
    public class RabbitStatusArrayParameter : RabbitStatusParameterBase
    {
        private List<RabbitStatusParameterBase> _values = new List<RabbitStatusParameterBase>();

        public List<RabbitStatusParameterBase> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public override string Value
        {
            get
            {
                if (Values.Any())
                {
                    var res = Environment.NewLine;
                    for (int i = 0; i < Values.Count; ++i)
                    {
                        res += Values[i].Name + ":" + Values[i].Value + ((i == Values.Count - 1) ? string.Empty : Environment.NewLine);
                    }

                    return res;
                }
                return string.Empty;
            }
        }

        public override string this[string name]
        {
            get
            {
                var splt = name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (splt.Count() == 1 && this.Name == splt.First())
                {
                    return Value;
                }
                if (splt.Count() > 1 && this.Name == splt.First())
                {
                    foreach (var child in Values)
                    {
                        if (child.Name == splt[1])
                        {
                            return child[name.Substring(Name.Length + 1)];
                        }
                    }
                }
                return string.Empty;
            }
        }
    }

}
