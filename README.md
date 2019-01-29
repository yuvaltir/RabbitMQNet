# RabbitMQ
RabbitMQ .Net Library


Hi!

This project provides a naive library to simplify RabbitMQ status parsing.
It was created as sometimes one would prefer to use the command line util instead of the management plugin to retrieve the RMQ status.

The parser's "ParseText" method accepts as an input a text which can be extracted from RabbitMQ using the command: rabbitmqctl status
The output is an object of type RabbitStatusParameterBase.

For example:

/******************************************************************************************************************************************

var status = File.ReadAllText("status.txt");

var parser = new RabbitStatusParser();

var res = parser.ParseText(status);

var mgmtDbMemoryStr=res["Status of node 'rabbit@MyMachineName'.memory.mgmt_db"].Value; // assume rabbit@MyMachineName is the name of the RMQ node name

var mgmtDbMemory = int.Parse(mgmtDbMemoryStr);

******************************************************************************************************************************************/


Hope this helps you.
