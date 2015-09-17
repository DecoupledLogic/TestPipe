namespace TestPipe.Assertions
{
    using System;
    using System.Collections;
    using System.Drawing.Imaging;
    using System.IO;
    using TestPipe.Core.Exceptions;
    using TestPipe.Core.Interfaces;
    using TestPipe.Core.Session;

    public class StepAsserts : IAsserts
    {
        private IBrowser browser;
        private string featureTitle = string.Empty;
        private string scenarioTitle = string.Empty;

        public StepAsserts(IBrowser browser, string featureTitle, string scenarioTitle)
        {
            this.browser = browser;
            this.featureTitle = featureTitle;
            this.scenarioTitle = scenarioTitle;
            this.Result = new Result();
        }

        public Result Result { get; set; }

        public void AssignableFrom(object arg1, Type arg2, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is not assignable from {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg1.GetType().IsAssignableFrom(arg2), failMessage, parameters);
        }

        public void Empty(string arg1, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is not an empty string.", arg1);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg1.Length == 0, failMessage, parameters);
        }

        public void Empty(object[] arg1, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is not an empty string.", arg1);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg1.Length == 0, failMessage, parameters);
        }

        public void Empty(ICollection arg1, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is not an empty collection.", arg1);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg1.Count == 0, failMessage, parameters);
        }

        public void Equal(object arg1, object arg2, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is less than or equal to {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg1.Equals(arg2), failMessage, parameters);
        }

        public void Equal(string arg1, string arg2, bool ignoreCase = false, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is not equal to {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            if (ignoreCase)
            {
                bool result = string.Compare(arg1, arg2, StringComparison.CurrentCultureIgnoreCase) == 0;
                this.IsTrue(result, failMessage, parameters);
                return;
            }

            this.IsTrue(string.Compare(arg1, arg2) == 0, failMessage, parameters);
        }

        public void Equal<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
        {
            string reason = string.Format("{0} is not equal to {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(((IComparable)arg1).CompareTo(arg2) == 0, failMessage, parameters);
        }

        public void Fail(string message = "", params object[] parameters)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Assert bombed with unknown reason.";
            }

            if (this.browser != null)
            {
                Directory.CreateDirectory(string.Format("{0}{1}", @"Features\", this.featureTitle));
                this.browser.TakeScreenshot(string.Format("{0}{1}{2}{3}{4}{5}", @"Features\", this.featureTitle, "\\", this.scenarioTitle, DateTime.Now.Ticks.ToString(), ".png"), ImageFormat.Png);
            }

            AssertBombedException ex =new AssertBombedException(message, parameters);

            this.Result.AssertStatus = Core.Enums.AssertStatusEnum.Fail;
            this.Result.Exception = ex;

            throw ex;
        }

        public void Greater<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
        {
            string reason = string.Format("{0} is not greater than {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(((IComparable)arg1).CompareTo(arg2) > 0, failMessage, parameters);
        }

        public void GreaterOrEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
        {
            string reason = string.Format("{0} is not greater than or equal to {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(((IComparable)arg1).CompareTo(arg2) >= 0, failMessage, parameters);
        }

        public void Ignored()
        {
            throw new IgnoreException();
        }

        public void InstanceOfType(object arg1, Type arg2, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is not an instance of Type {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg2.IsInstanceOfType(arg1), failMessage, parameters);
        }

        public void IsFalse(bool condition, string message = "", params object[] parameters)
        {
            if (!condition)
            {
                return;
            }

            string failMessage = this.GetFailMessage("Expected false. Actual true.", message);

            this.Fail(failMessage, parameters);
        }

        public void IsTrue(bool condition, string message = "", params object[] parameters)
        {
            if (condition)
            {
                return;
            }

            string failMessage = "Expected true. Actual false.";

            if (!string.IsNullOrWhiteSpace(message))
            {
                failMessage = message;
            }

            this.Fail(failMessage, parameters);
        }

        public void Less<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
        {
            string reason = string.Format("{0} is not less than {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(((IComparable)arg1).CompareTo(arg2) < 0, failMessage, parameters);
        }

        public void LessOrEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
        {
            string reason = string.Format("{0} is not less than or equal to {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(((IComparable)arg1).CompareTo(arg2) <= 0, failMessage, parameters);
        }

        public void Manual()
        {
            throw new ManualOnlyException();
        }

        public void NaN(double value, string message = "", params object[] parameters)
        {
            string reason = string.Format("Expected {0} but was {1}.", double.NaN, value);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(double.IsNaN(value), failMessage, parameters);
        }

        public void NotAssignableFrom(object arg1, Type arg2, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is assignable from {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsFalse(arg1.GetType().IsAssignableFrom(arg2), failMessage, parameters);
        }

        public void NotEmpty(string arg1, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is an empty string.", arg1);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg1.Length > 0, failMessage, parameters);
        }

        public void NotEmpty(ICollection arg1, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is an empty collection.", arg1);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg1.Count > 0, failMessage, parameters);
        }

        public void NotEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
        {
            string reason = string.Format("{0} is equal to {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(((IComparable)arg1).CompareTo(arg2) != 0, failMessage, parameters);
        }

        public void NotInstanceOfType(object arg1, Type arg2, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is an instance of Type {1}.", arg1, arg2);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsFalse(arg2.IsInstanceOfType(arg1), failMessage, parameters);
        }

        public void NotNaN(double value, string message = "", params object[] parameters)
        {
            string reason = string.Format("Expected {0} but was {1}.", double.NaN, value);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(!double.IsNaN(value), failMessage, parameters);
        }

        public void NotNull(object arg1, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is null.", arg1);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsFalse(arg1 != null, failMessage, parameters);
        }

        public void Null(object arg1, string message = "", params object[] parameters)
        {
            string reason = string.Format("{0} is not null.", arg1);

            string failMessage = this.GetFailMessage(reason, message);

            this.IsTrue(arg1 == null, failMessage, parameters);
        }

        /// <summary>
        /// Checks to make sure that the input delegate throws a exception of type exceptionType.
        /// </summary>
        /// <typeparam name="TException">The type of exception expected.</typeparam>
        /// <param name="action">The action to execute to generate the exception.</param>
        public void Throws<TException>(Action action) where TException : System.Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                this.InstanceOfType(ex, typeof(TException), "Expected exception was not thrown. ");
                return;
            }

            this.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
        }

        /// <summary>
        /// Checks to make sure that the input delegate throws a exception of type exceptionType.
        /// </summary>
        /// <typeparam name="TException">The type of exception expected.</typeparam>
        /// <param name="expectedMessage">The expected message text.</param>
        /// <param name="action">The action to execute to generate the exception.</param>
        public void Throws<TException>(string expectedMessage, Action action) where TException : System.Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                this.InstanceOfType(ex, typeof(TException), "Expected exception was not thrown. ");
                this.Equal(expectedMessage, ex.Message, "Expected exception with a message of '" + expectedMessage + "' but exception with message of '" + ex.Message + "' was thrown instead.");
                return;
            }

            this.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
        }

        private string GetFailMessage(string message, string message2 = "")
        {
            if (string.IsNullOrEmpty(message2))
            {
                return message;
            }

            return string.Format("{0} {1}", message, message2);
        }
    }
}