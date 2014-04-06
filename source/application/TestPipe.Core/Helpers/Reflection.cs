namespace TestPipe.Core.Helpers
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class Reflection
    {
        public static MethodInfo FindMethodWithOneParameterOfType(Type typeToSearch, Type paramType)
        {
            //find handler by reflection
            MethodInfo method = null;
            var t = typeToSearch;

            while (t != null && method == null)
            {
                var methods = from m in t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                              where
                                m.GetParameters().Length == 1
                                && m.GetParameters().First().ParameterType.Equals(paramType)
                              select m;

                method = methods.FirstOrDefault();
                t = t.BaseType;
            }
            return method;
        }
    }
}