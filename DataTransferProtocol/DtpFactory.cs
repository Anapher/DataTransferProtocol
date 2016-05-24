using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DataTransferProtocol.Compression;
using DataTransferProtocol.NetSerializer;

namespace DataTransferProtocol
{
    public class DtpFactory
    {
        /// <summary>
        ///     Send the given data to the server
        /// </summary>
        /// <param name="data">The data to send. Do not modify it without restoring the orginal data</param>
        public delegate void SendData(byte[] data);

        private readonly Dictionary<Guid, AutoResetEvent> _methodDictionary;
        private readonly Dictionary<Guid, byte[]> _methodResponses;
        private readonly Dictionary<Guid, Exception> _exceptionResponses;
        private readonly SendData _sendData;

        internal static readonly Guid ExceptionGuid = new Guid("37F5CC7B-E3D7-4BE5-806C-CBD761FAA01B");
        internal static readonly Guid FunctionNotFoundExceptionGuid = new Guid("510DE836-A1A1-4025-8534-43DF64E86ADB");

        /// <summary>
        ///     The <see cref="DtpFactory" /> is the execution module of the connection. This should be populated at the client's
        ///     side
        /// </summary>
        /// <param name="sendData">The delegate to send data to a <see cref="DtpProcessor" /></param>
        public DtpFactory(SendData sendData)
        {
            _sendData = sendData;
            _methodDictionary = new Dictionary<Guid, AutoResetEvent>();
            _methodResponses = new Dictionary<Guid, byte[]>();
            _exceptionResponses = new Dictionary<Guid, Exception>();
        }

        /// <summary>
        ///     Receive data from the server and process it
        /// </summary>
        /// <param name="data">The data received by the server</param>
        public void Receive(byte[] data)
        {
            data = LZF.Decompress(data, 0);
            var guid = new Guid(data.Take(16).ToArray());

            AutoResetEvent autoResetEvent;

            if (guid == FunctionNotFoundExceptionGuid)
            {
                var sessionGuid = new Guid(data.Skip(16).Take(16).ToArray());
                if (!_methodDictionary.TryGetValue(sessionGuid, out autoResetEvent))
                    throw new InvalidOperationException("Session was not registered");

                _exceptionResponses.Add(sessionGuid, new InvalidOperationException(
                    $"Method or function with name {Encoding.UTF8.GetString(data, 32, data.Length - 32)} not found"));
                autoResetEvent.Set();
                return;
            }

            if (guid == ExceptionGuid)
            {
                var errorReport = new Serializer(typeof (DtpException)).Deserialize<DtpException>(data, 16);

                if (!_methodDictionary.TryGetValue(errorReport.SessionGuid, out autoResetEvent))
                    throw new InvalidOperationException("Session was not registered");

                _exceptionResponses.Add(errorReport.SessionGuid, new ServerException(errorReport));
                autoResetEvent.Set();
                return;
            }

            if (!_methodDictionary.TryGetValue(guid, out autoResetEvent))
                throw new InvalidOperationException("Session was not registered");

            var valueLength = BitConverter.ToInt32(data, 16);
            if (valueLength > 0)
            {
                var buffer = new byte[valueLength];
                Array.Copy(data, 20, buffer, 0, valueLength);
                _methodResponses.Add(guid, buffer);
            }

            autoResetEvent.Set();
        }

        /// <summary>
        ///     Execute a function on the server
        /// </summary>
        /// <typeparam name="T">The response type</typeparam>
        /// <param name="functionName">The name of the function</param>
        /// <param name="parameters">The parameters of the function</param>
        /// <returns>Returns the response from the server</returns>
        public T ExecuteFunction<T>(string functionName, params object[] parameters)
        {
            return ExecuteFunction<T>(functionName, null, null, parameters);
        }

        /// <summary>
        ///     Execute a method on the server
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        /// <param name="specialParameterTypes">
        ///     Types which aren't obvious (e. g. if abstract classes are used) for the parameter
        ///     in order to serialize it correctly
        /// </param>
        /// <param name="specialReturnTypes">
        ///     Types which aren't obvious (e. g. if abstract classes are used) for the response in
        ///     order to deserialize it correctly
        /// </param>
        /// <param name="parameters">The parameters of the method</param>
        public void ExecuteMethod(string methodName, List<Type> specialParameterTypes, List<Type> specialReturnTypes,
            params object[] parameters)
        {
            ExecuteFunction<object>(methodName, specialParameterTypes, specialReturnTypes, parameters);
        }

        /// <summary>
        ///     Execute a method on the server
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        /// <param name="parameters">The parameters of the method</param>
        public void ExecuteMethod(string methodName, params object[] parameters)
        {
            ExecuteFunction<object>(methodName, null, null, parameters);
        }

        /// <summary>
        ///     Execute a function on the server
        /// </summary>
        /// <typeparam name="T">The response type</typeparam>
        /// <param name="functionName">The name of the function</param>
        /// <param name="specialParameterTypes">
        ///     Types which aren't obvious (e. g. if abstract classes are used) for the parameter
        ///     in order to serialize it correctly
        /// </param>
        /// <param name="specialReturnTypes">
        ///     Types which aren't obvious (e. g. if abstract classes are used) for the response in
        ///     order to deserialize it correctly
        /// </param>
        /// <param name="parameters">The parameters of the function</param>
        /// <returns>Returns the response from the server</returns>
        public T ExecuteFunction<T>(string functionName, List<Type> specialParameterTypes, List<Type> specialReturnTypes,
            params object[] parameters)
        {
            var methodGuid = Guid.NewGuid();
            while (methodGuid == ExceptionGuid || methodGuid == FunctionNotFoundExceptionGuid) //possibilities are everywhere
                methodGuid = Guid.NewGuid();

            var parameterData = new List<byte[]>();

            foreach (var parameter in parameters)
            {
                var types = new List<Type> {parameter.GetType()};
                if (specialParameterTypes != null && specialParameterTypes.Count > 0)
                    types.AddRange(specialParameterTypes);
                parameterData.Add(new Serializer(types).Serialize(parameter));
            }

            var functionNameData = Encoding.UTF8.GetBytes(functionName);
            var data =
                new byte[16 + 4 + functionNameData.Length + 4 + parameterData.Count*4 + parameterData.Sum(x => x.Length)
                    ];
            //Protocol
            //HEAD  - 16 Bytes                  - Guid
            //HEAD  - 4 Bytes                   - Function Name Length
            //HEAD  - UTF8(FunctionName).Length - Function Name
            //INFO  - 4 Bytes                   - Parameter count
            //PINF  - COUNT(Paramters)*4        - Information about the length of the parameters
            //DATA  - SUM(Parameters.Length)    - The parameter data

            Array.Copy(methodGuid.ToByteArray(), data, 16);
            Array.Copy(BitConverter.GetBytes(functionNameData.Length), 0, data, 16, 4);
            Array.Copy(functionNameData, 0, data, 20, functionNameData.Length);
            Array.Copy(BitConverter.GetBytes(parameters.Length), 0, data, 20 + functionNameData.Length, 4);
            for (int i = 0; i < parameterData.Count; i++)
            {
                Array.Copy(BitConverter.GetBytes(parameterData[i].Length), 0, data, 24 + functionNameData.Length + i*4,
                    4);
            }

            int offset = 0;
            foreach (var parameter in parameterData)
            {
                Array.Copy(parameter, 0, data, 24 + functionNameData.Length + parameterData.Count*4 + offset,
                    parameter.Length);
                offset += parameter.Length;
            }

            using (var autoResetEvent = new AutoResetEvent(false))
            {
                _methodDictionary.Add(methodGuid, autoResetEvent);
                _sendData.Invoke(LZF.Compress(data, 0));
                if (!autoResetEvent.WaitOne(15000))
                    throw new InvalidOperationException("Timeout");

                _methodDictionary.Remove(methodGuid);

                Exception exception;
                if (_exceptionResponses.TryGetValue(methodGuid, out exception))
                {
                    _exceptionResponses.Remove(methodGuid);
                    throw exception;
                }

                byte[] response;
                if (_methodResponses.TryGetValue(methodGuid, out response))
                {
                    _methodResponses.Remove(methodGuid);
                    var types = new List<Type> {typeof (T)};
                    if (specialReturnTypes != null && specialReturnTypes.Count > 0)
                        types.AddRange(specialReturnTypes);
                    return new Serializer(types).Deserialize<T>(response);
                }

                return default(T);
            }
        }
    }
}