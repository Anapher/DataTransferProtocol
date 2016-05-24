# Data Transfer Protocol
A solution to execute methods and functions using streams.

Sample code for the server side (where the methods/functions are executed)
```csharp
            var processor = new DtpProcessor();
            processor.RegisterMethod("WriteToConsole", parameters => Console.WriteLine(parameters.GetString(0)));
            processor.RegisterFunction("GetMachineName", parameters => Environment.MachineName);
```

That's it! When you receive data, you can just use the function `processor.Receive(data)`, which also returns the response as a `byte[]`.

Sample code for the client side:
```csharp
            var factory = new DtpFactory(x => myStream.Write(x));
            factory.ExecuteMethod("WriteToConsole", "Hello World!");
            var machineName = factory.ExecuteFunction<string>("GetMachineName");
```

When you receive data from the `processor.Receive(data)` function, you should execute `factory.Receive(data)`. The send delegate provides the data for the `processor.Receive(data)` part.

## Complex data structures
Abstract classes etc. are supported. Just look into the example code from the unit test and from the Tcp sample.


## References
- [NetSerializer](https://github.com/tomba/netserializer) is used to serialize the data, licensed under the Mozilla Public
License v. 2.0
- A modfied version of [LZF](https://csharplzfcompression.codeplex.com/) is used to compress the data, licensed under the BSD license


## License
The Data Transfer Protocol is licensed under the [Microsoft Public License (MS-PL)](LICENSE)
