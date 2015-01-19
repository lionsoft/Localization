using System.Globalization;

namespace Lion.Localization
{
	public static class DefaultCulture
	{
		private static readonly CultureInfo _culture = new CultureInfo(1033);
		public static CultureInfo Value
		{
			get { return _culture; }
		}

		public static bool IsDefault(this string name)
		{
			return name == Value.Name;
		}

		public static bool IsDefault(this CultureInfo culture)
		{
			return culture.Name == Value.Name;
		}
	}
}