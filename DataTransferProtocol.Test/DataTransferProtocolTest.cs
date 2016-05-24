using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTransferProtocol.Test
{
    [Serializable]
    public abstract class TestClassBase
    {
        public abstract string TestString { get; set; }
    }

    [Serializable]
    public class TestClassImplementation : TestClassBase
    {
        public override string TestString { get; set; } = "test123";
    }

    [TestClass]
    public class DataTransferProtocolTest
    {
        private DtpFactory _dtpFactory;
        private DtpProcessor _dtpProcessor;

        [TestMethod, TestCategory("DataTransferProtocol")]
        public void TestDataTransferMethods()
        {
            _dtpFactory = new DtpFactory(SendData);
            _dtpProcessor = new DtpProcessor();

            _dtpProcessor.RegisterMethod("TestMethod1",
                parameters => Trace.WriteLine("Parameterless method successfully executed"));
            _dtpProcessor.RegisterMethod("TestMethod2", parameters =>
            {
                Assert.AreEqual(parameters.GetInt32(0), 19);
                Trace.WriteLine("Method with int parameter successfully executed");
            });
            _dtpProcessor.RegisterMethod("TestMethod3",
                parameters =>
                {
                    var result = parameters.GetValue<List<TestClassBase>>(0, typeof (TestClassImplementation));
                    Assert.IsNotNull(result);
                    Assert.AreEqual(result.Count, 3);
                    Assert.AreEqual(result[1].TestString, "test");
                    Trace.WriteLine("Method with complex parameter successfully executed");
                });

            Trace.WriteLine("Executing parameterless method");
            _dtpFactory.ExecuteMethod("TestMethod1");
            Trace.WriteLine("Executing method with in parameter");
            _dtpFactory.ExecuteMethod("TestMethod2", 19);
            Trace.WriteLine("Executing method with complex parameter");
            _dtpFactory.ExecuteMethod("TestMethod3",
                new List<Type> {typeof (TestClassImplementation)}, null,
                new List<TestClassBase>
                {
                    new TestClassImplementation {TestString = "garcon"},
                    new TestClassImplementation {TestString = "test"},
                    new TestClassImplementation {TestString = "d'allemand"}
                });
        }

        [TestMethod, TestCategory("DataTransferProtocol")]
        public void TestDataTransferFunctions()
        {
            _dtpFactory = new DtpFactory(SendData);
            _dtpProcessor = new DtpProcessor();

            _dtpProcessor.RegisterFunction("TestFunction1",
                parameters =>
                {
                    Trace.WriteLine("Parameterless function with int returning type successfully executed");
                    return 0;
                });

            _dtpProcessor.RegisterFunction("TestFunction2", parameters =>
            {
                Assert.AreEqual(parameters.GetInt32(0), 19);
                Trace.WriteLine("Function with int parameter and string returning type successfully executed");
                return "wtf";
            });

            _dtpProcessor.RegisterFunction("TestFunction3",
                parameters =>
                {
                    var result = parameters.GetValue<List<TestClassBase>>(0, typeof (TestClassImplementation));
                    Assert.IsNotNull(result);
                    Assert.AreEqual(result.Count, 3);
                    Assert.AreEqual(result[1].TestString, "test");
                    Trace.WriteLine("Function with complex parameter and complex returning successfully executed");
                    return new List<TestClassBase>
                    {
                        new TestClassImplementation(),
                        new TestClassImplementation(),
                        new TestClassImplementation()
                    };
                }, typeof (TestClassImplementation));


            Trace.WriteLine("Executing parameterless method");
            Assert.AreEqual(_dtpFactory.ExecuteFunction<int>("TestFunction1"), 0);

            Trace.WriteLine("Executing method with in parameter");
            Assert.AreEqual(_dtpFactory.ExecuteFunction<string>("TestFunction2", 19), "wtf");

            Trace.WriteLine("Executing method with complex parameter");
            Assert.AreEqual(_dtpFactory.ExecuteFunction<List<TestClassBase>>("TestFunction3",
                new List<Type> {typeof (TestClassImplementation)},
                new List<Type> {typeof (TestClassImplementation)},
                new List<TestClassBase>
                {
                    new TestClassImplementation(),
                    new TestClassImplementation {TestString = "test"},
                    new TestClassImplementation()
                }).Count, 3);
        }

        private void SendData(byte[] data)
        {
            _dtpFactory.Receive(_dtpProcessor.Receive(data));
        }
    }
}