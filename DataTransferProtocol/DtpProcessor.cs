using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTransferProtocol.Compression;
using DataTransferProtocol.NetSerializer;

namespace DataTransferProtocol
{
    /// <summary>
    ///     Receives data from <see cref="DtpFactory" /> executes the command and responses
    /// </summary>
    public class DtpProcessor
    {
        /// <summary>
        ///     A function responses with an object.
        /// </summary>
        /// <param name="parameters">Parameters given by the <see cref="DtpFactory" /></param>
        /// <returns>Returns an object which should get sent back to the <see cref="DtpFactory" /></returns>
        public delegate object DtpFunction(DtpParameters parameters);

        /// <summary>
        ///     A method just does some stuff
        /// </summary>
        /// <param name="parameters">Parameters given by the <see cref="DtpFactory" /></param>
        public delegate void DtpMethod(DtpParameters parameters);

        private readonly Dictionary<string, DtpFunction> _functions;
        private readonly Dictionary<string, DtpMethod> _methods;
        private readonly Dictionary<string, Type[]> _specialTypes;

        /// <summary>
        ///     The <see cref="DtpProcessor" /> is the processing module of the connection. This should be populated at the
        ///     server's side
        /// </summary>
        public DtpProcessor()
        {
            _methods = new Dictionary<string, DtpMethod>();
            _functions = new Dictionary<string, DtpFunction>();
            _specialTypes = new Dictionary<string, Type[]>();
        }

        public event EventHandler<UnhandledExceptionEventArgs> ExceptionOccured;

        /// <summary>
        ///     Register a new method
        /// </summary>
        /// <param name="methodName">The name of the method. It must be unique, else an exception gets thrown</param>
        /// <param name="dtpMethod">The delegate which gets executed when a request is received</param>
        public void RegisterMethod(string methodName, DtpMethod dtpMethod)
        {
            if (_methods.ContainsKey(methodName) || _functions.ContainsKey(methodName))
                throw new InvalidOperationException();

            _methods.Add(methodName, dtpMethod);
        }

        /// <summary>
        ///     Register a new function
        /// </summary>
        /// <param name="functionName">The name of the function. It must be unique, else an exception gets thrown</param>
        /// <param name="dtpFunction">The delegate which gets executed when a request is received</param>
        /// <param name="specialTypes">
        ///     Types which aren't obvious (e. g. if abstract classes are used) for the return value in
        ///     order to serialize it correctly
        /// </param>
        public void RegisterFunction(string functionName, DtpFunction dtpFunction, params Type[] specialTypes)
        {
            if (_functions.ContainsKey(functionName) || _methods.ContainsKey(functionName))
                throw new InvalidOperationException();

            _functions.Add(functionName, dtpFunction);
            if (specialTypes != null && specialTypes.Length > 0)
                _specialTypes.Add(functionName, specialTypes);
        }

        /// <summary>
        ///     Process the received data from a <see cref="DtpFactory" />
        /// </summary>
        /// <param name="data">The data which was given using the <see cref="DtpFactory.SendData" /> delegate</param>
        /// <returns>Returns the response which must get processed in <see cref="DtpFactory.Receive" /></returns>
        public byte[] Receive(byte[] data)
        {
            data = LZF.Decompress(data, 0);
            var functionNameLength = BitConverter.ToInt32(data, 16);
            var functionName = Encoding.UTF8.GetString(data, 20, functionNameLength);
            if (!_methods.ContainsKey(functionName) && !_functions.ContainsKey(functionName))
            {
                ExceptionOccured?.Invoke(this,
                    new UnhandledExceptionEventArgs(
                        new InvalidOperationException($"Method or function with name {functionName} not found")));

                var errorResponse = new byte[16 + functionNameLength];
                Array.Copy(DtpFactory.FunctionNotFoundExceptionGuid.ToByteArray(), errorResponse, 16);
                Array.Copy(data, 20, errorResponse, 16, functionNameLength);
                return errorResponse;
            }

            var parameterCount = BitConverter.ToInt32(data, 20 + functionNameLength);

            var parameterLengths = new List<int>();
            var parameters = new Dictionary<int, byte[]>();

            for (int i = 0; i < parameterCount; i++)
                parameterLengths.Add(BitConverter.ToInt32(data, 24 + functionNameLength + i*4));

            var offset = 0;
            for (int i = 0; i < parameterCount; i++)
            {
                var parameterData = new byte[parameterLengths[i]];
                Array.Copy(data, 24 + functionNameLength + parameterCount*4 + offset, parameterData, 0,
                    parameterData.Length);
                parameters.Add(i, parameterData);
                offset += parameterData.Length;
            }

            var dtpParameters = new DtpParameters(parameters);
            byte[] result = null;

            try
            {
                DtpMethod method;
                if (_methods.TryGetValue(functionName, out method))
                {
                    method.Invoke(dtpParameters);
                }
                else
                {
                    DtpFunction function;
                    if (_functions.TryGetValue(functionName, out function))
                    {
                        var returnedObject = function.Invoke(dtpParameters);

                        if (returnedObject != null)
                        {
                            var typeList = new List<Type> { returnedObject.GetType() };
                            Type[] specialTypes;
                            if (_specialTypes.TryGetValue(functionName, out specialTypes))
                                typeList.AddRange(specialTypes);

                            result = new Serializer(typeList).Serialize(returnedObject);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var exception = new DtpException
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    FunctionName = functionName,
                    ParameterInformation = string.Join(", ",
                        parameters.Select(x => x.Key + " - " + x.Value.Length + " B").ToArray()),
                    SessionGuid = new Guid(data.Take(16).ToArray())
                };

                var exceptionData = new Serializer(typeof(DtpException)).Serialize(exception);
                var errorResponse = new byte[16 + exceptionData.Length];
                Array.Copy(DtpFactory.ExceptionGuid.ToByteArray(), errorResponse, 16);
                Array.Copy(exceptionData, 0, errorResponse, 16, exceptionData.Length);

                ExceptionOccured?.Invoke(this, new UnhandledExceptionEventArgs(ex));
                return LZF.Compress(errorResponse, 0);
            }

            var response = new byte[16 + 4 + (result?.Length ?? 0)];
            //Protocol
            //HEAD  - 16 Bytes      - Guid
            //HEAD  - 4 Bytes       - Response Length
            //DATA  - result.Length - Result Length
            Array.Copy(data, 0, response, 0, 16); //copy guid
            Array.Copy(BitConverter.GetBytes(result?.Length ?? 0), 0, response, 16, 4);
            if (result != null)
                Array.Copy(result, 0, response, 20, result.Length);

            return LZF.Compress(response, 0);
        }
    }
}