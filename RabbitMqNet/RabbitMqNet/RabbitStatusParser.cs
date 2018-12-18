using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqNet
{
    /// <summary>
    /// Simple RMQ Commandline status parser
    /// </summary>
    public class RabbitStatusParser
    {
        /// <summary>
        /// Parse the status response
        /// </summary>
        /// <param name="text">Ooutput of the rabbitmqctl status command</param>
        /// <returns>RabbitStatusParameterBase</returns>
        public RabbitStatusParameterBase ParseText(string text)
        {
            if(string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var reader = new StringReader(text.Replace(Environment.NewLine, string.Empty));
           
            return ParseTextRec(reader);
        }        

        private RabbitStatusParameterBase ParseTextRec(StringReader reader)
        {
            RabbitStatusParameterBase currentParam = new RabbitStatusValueParameter();

            string currentParamName = string.Empty;
            while (reader.Peek() != -1)
            {
                while (Char.IsWhiteSpace((char)reader.Peek()))
                {
                    reader.Read();
                }
                if (reader.Peek() == -1)
                {                 
                    break;
                }


                var c = (char)reader.Peek();
                switch (c)
                {
                    case '[':
                        reader.Read();
                        
                        currentParam = new RabbitStatusArrayParameter { Name = currentParamName };
                        while (true)
                        {
                            var child = ParseTextRec(reader);
                            if (child == null)
                            {                        
                                return currentParam;
                            }
                            else if (string.IsNullOrWhiteSpace(child.Name))
                            {
                                continue;
                            }
                            else
                            {                             
                                ((RabbitStatusArrayParameter)currentParam).Values.Add(child);
                            }
                        }

                    case '{':
                        reader.Read();                        
                        //either new parameter or value with { } inside
                        if (string.IsNullOrWhiteSpace(currentParam.Name))
                        {
                            currentParamName = ReadString(reader);
                            currentParam.Name = currentParamName;
                            
                            while ((char)reader.Peek() == ',')
                            {
                                reader.Read();
                                var val = ReadString(reader);
                                if (!string.IsNullOrWhiteSpace(val))
                                {                            
                                    ((RabbitStatusValueParameter)currentParam).Values.Add(val);
                                }
                            }
                        }
                        else
                        {
                            var val = ReadUntilCharacter(reader, '}');
                            if (!string.IsNullOrWhiteSpace(val))
                            {                                
                                ((RabbitStatusValueParameter)currentParam).Values.Add(val);
                            }
                        }
                        break;

                    case ']':                        
                        reader.Read();
                        return null;

                    case '}':                        
                        reader.Read();
                        return currentParam;

                    case ',':                        
                        reader.Read();
                        break;

                    default:                        
                        currentParamName = ReadString(reader);
                        break;
                }
            }
            return currentParam;
        }

        private static List<char> SpecialChars = new List<char> { ',', '{', '}', '[', ']' };

        private string ReadUntilCharacter(StringReader reader, char lastChar)
        {
            string res = string.Empty;
            while (reader.Peek() != -1)
            {
                while (Char.IsWhiteSpace((char)reader.Peek()))
                {
                    reader.Read();
                }
                while (reader.Peek() != -1)
                {
                    char c = (char)reader.Read();
                    if (c == lastChar)
                    {
                        return res;
                    }
                    res += c;
                }
            }
            return res;
        }

        private string ReadString(StringReader reader)
        {
            string res = string.Empty;
            while (reader.Peek() != -1)
            {
                while (Char.IsWhiteSpace((char)reader.Peek()))
                {
                    reader.Read();
                }
                while (reader.Peek() != -1)
                {
                    var c = (char)reader.Peek();
                    if (SpecialChars.Contains(c))
                    {
                        return res;
                    }
                    if (c == '"')
                    {
                        res += (char)reader.Read();
                        while ((char)reader.Peek() != '"')
                            res += (char)reader.Read();
                        res += (char)reader.Read();
                    }
                    else
                    {
                        res += (char)reader.Read();
                    }
                }
            }
            return res;
        }
    }
}
