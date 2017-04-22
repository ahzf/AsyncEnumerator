﻿using System;
using System.Threading.Tasks;
using AsyncEnumerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AsyncEnumeratorTests
{
    [TestClass]
    public class AsyncEnumeratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Awaiting on failed taskk did not throw.")]
        public async Task ThrowsOnMoveNext()
        {
            var iter = ExceptionTest();
            await iter.MoveNext();
        }

        [TestMethod]
        public async Task EnumerationAdvancesCorrectlyAndCompletes1()
        {
            var iter = Test1();

            await iter.MoveNext();
            Assert.AreEqual(iter.Current, 1, $"First call to {nameof(iter.MoveNext)} did not advance the enumeration correctly.");

            await iter.MoveNext();
            Assert.AreEqual(iter.Current, 2, $"Call to {nameof(iter.MoveNext)} did not advance the enumeration correctly.");

            await iter.MoveNext();
            Assert.AreEqual(iter.Current, 3, $"Call to {nameof(iter.MoveNext)} did not advance the enumeration correctly.");

            Assert.IsFalse(await iter.MoveNext(), $"Call to {nameof(iter.MoveNext)} did not return false after enumeration completed.");

            Assert.IsTrue(iter.IsCompleted, "Enumeration did not complete after return.");
        }

        [TestMethod]
        public async Task EnumerationAdvancesCorrectlyAndCompletes2()
        {
            var iter = Test2();

            await iter.MoveNext();
            Assert.AreEqual(iter.Current, 1, $"First call to {nameof(iter.MoveNext)} did not advance the enumeration correctly.");

            await iter.MoveNext();
            Assert.AreEqual(iter.Current, 2, $"Call to {nameof(iter.MoveNext)} did not advance the enumeration correctly.");

            await iter.MoveNext();
            Assert.AreEqual(iter.Current, 3, $"Call to {nameof(iter.MoveNext)} did not advance the enumeration correctly.");

            Assert.IsFalse(await iter.MoveNext(), $"Call to {nameof(iter.MoveNext)} did not return false after enumeration completed.");

            Assert.IsTrue(iter.IsCompleted, "Enumeration did not complete after return.");
        }


        private static async AsyncEnumerator<int> ExceptionTest()
        {
            throw new Exception();

            await Task.Yield();

            return 1;
        }

        private static async AsyncEnumerator<int> Test1()
        {
            var iter = await AsyncEnumerator<int>.Capture();

            await iter.Yield(1);

            await iter.Yield(2);

            await iter.Yield(3);

            return iter.YieldReturn();
        }

        private static async AsyncEnumerator<int> Test2()
        {
            var iter = await AsyncEnumerator<int>.Capture();

            for (var i = 1; i <= 3; i++)
            {
                await iter.Yield(i);
            }

            return iter.YieldReturn();
        }

    }
}
