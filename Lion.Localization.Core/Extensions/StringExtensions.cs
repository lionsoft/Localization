using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lion.Localization
{
	public static class StringExtensions
	{
	    internal static string Scope()
	    {
	        var stackFrames = new StackTrace().GetFrames();
            var type = stackFrames == null ? null : stackFrames.Select(f => f.GetMethod().DeclaringType).FirstOrDefault(t => t.Assembly != Assembly.GetExecutingAssembly());
            return LocalizationManager.Instance.FormatScope(type);
        }

	    internal static string Scope(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return LocalizationManager.Instance.FormatScope(type);
        }

	    internal static string Scope(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return Scope(obj.GetType());
        }

	    private static string Localize(this string value, string scope)
        {
            if (scope == null)
                throw new ArgumentNullException();

            LocalizationManager.Instance.InsertScope(scope.ToLower());

            if (CultureInfo.CurrentCulture.IsDefault())
                return value;

            if (LocalizationManager.Repository == null)
                return value;

            var translation = LocalizationManager.Instance.Translate(scope, value);
            return string.IsNullOrEmpty(translation)
                    ? value
                    : translation;
        }

        public static string L(this string value)
        {
            return value.Localize(Scope());
        }

	    public static string L(this string value, Type type)
	    {
	        if (type == null) throw new ArgumentNullException("type");
	        return value.Localize(Scope(type));
	    }

	    public static string L(this string value, object obj)
	    {
	        if (obj == null) throw new ArgumentNullException("obj");
	        return value.L(obj.GetType());
	    }
	}
}
