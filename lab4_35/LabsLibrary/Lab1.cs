namespace LabsLibrary
{
	public static class Lab1
	{
		public static string StartLab(string input = "INPUT.TXT")
		{
			var inputData = File.ReadLines(input).First().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();
			if (inputData[1] > 100)
			{
				return "Out of range exception!";
			}
			else
			{
				return GetAnswerFromSecondSatellite(inputData[0], inputData[1]);
			}
		}

		private static string GetAnswerFromSecondSatellite(int sumOfDigits, int countOfDigits)
		{
			int firstNumber = 0, sumForFirst = sumOfDigits, numberOfDigitForFirst = 1;
			int secondNumber = Convert.ToInt32(Math.Pow(10, countOfDigits - 1)), sumForSecond = sumOfDigits - 1, numberOfDigitForSecond = countOfDigits;
			while (sumForFirst > 0)
			{
				int fDigit = sumForFirst >= 10 ? 9 : sumForFirst;
				sumForFirst -= fDigit;
				firstNumber += fDigit * Convert.ToInt32(Math.Pow(10, countOfDigits - numberOfDigitForFirst));
				numberOfDigitForFirst++;
			}
			while (sumForSecond > 0)
			{
				int sDigit = sumForSecond >= 10 ? 9 : sumForSecond;
				sumForSecond -= sDigit;
				secondNumber += sDigit * Convert.ToInt32(Math.Pow(10, countOfDigits - numberOfDigitForSecond));
				numberOfDigitForSecond--;
			}
			return $"{firstNumber} {secondNumber}";
		}
	}
}
