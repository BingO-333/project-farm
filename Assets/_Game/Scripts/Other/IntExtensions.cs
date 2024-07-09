namespace Game
{
	public static class IntExtensions
	{    		
		public static string ToStringWithAbbreviations(this int number)
        {
			if (number >= 1000000000)
				return (number / 1000000000f).ToString("F1") + "b";
			else if (number >= 1000000)
				return (number / 1000000f).ToString("F1") + "m";
			else if (number >= 1000)
				return (number / 1000f).ToString("F1") + "k";
			else
				return number.ToString();
		}
	}
}