﻿namespace TestPipe.Assertions.Properties 
{
    using System;
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
    internal class Resources 
    {
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() 
        {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager 
        {
            get 
            {
                if (object.ReferenceEquals(resourceMan, null)) 
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TestPipe.Assertions.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture 
        {
            get 
            {
                return resourceCulture;
            }
            set 
            {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} cannot contain a null (Nothing in Visual Basic) element..
        /// </summary>
        internal static string Argument_NullElement 
        {
            get 
            {
                return ResourceManager.GetString("Argument_NullElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} cannot be an empty string..
        /// </summary>
        internal static string ArgumentException_EmptyString 
        {
            get 
            {
                return ResourceManager.GetString("ArgumentException_EmptyString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected a collection containing &lt;0&gt; items but actual was &lt;{0}&gt; items..
        /// </summary>
        internal static string Assertion_CollectionFailure 
        {
            get 
            {
                return ResourceManager.GetString("Assertion_CollectionFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected &lt;{0}&gt; but actual was &lt;{1}&gt;..
        /// </summary>
        internal static string Assertion_GenericFailure 
        {
            get 
            {
                return ResourceManager.GetString("Assertion_GenericFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Assumption failed..
        /// </summary>
        internal static string AssumptionException_EmptyMessage 
        {
            get 
            {
                return ResourceManager.GetString("AssumptionException_EmptyMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Assumption failed. {0}.
        /// </summary>
        internal static string AssumptionException_Message 
        {
            get 
            {
                return ResourceManager.GetString("AssumptionException_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} member must be overridden by a derived class..
        /// </summary>
        internal static string NotImplemented_NotOverriddenByDerived 
        {
            get 
            {
                return ResourceManager.GetString("NotImplemented_NotOverriddenByDerived", resourceCulture);
            }
        }
    }
}
