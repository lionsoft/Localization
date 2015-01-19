namespace Lion.Localization
{
	public static class LocalizedObjectExtension
	{
		private const string Mark = "[x]";

		public static bool IsDisabled(this ILocalizedObject obj)
		{
			return !string.IsNullOrEmpty(obj.Translation) && obj.Translation.StartsWith(Mark);
		}

		public static void Disable(this ILocalizedObject obj)
		{
			if (!string.IsNullOrEmpty(obj.Translation))
			{
				if (!obj.Translation.StartsWith(Mark))
					obj.Translation = Mark + obj.Translation;
			}
			else
				obj.Translation = Mark;
		}
	}
}
