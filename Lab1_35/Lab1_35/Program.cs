namespace Lab1_35
{
    public class Program
    {
        public static string InputFilePath = @"..\..\input.txt";
        public static string OutputFilePath = @"..\..\output.txt";

        static void Main(string[] args)
        {
            FileInfo outputFileInfo = new FileInfo(OutputFilePath);
            var inputData = File.ReadLines(InputFilePath).First().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();
            using (StreamWriter streamWriter = outputFileInfo.CreateText())
            {
                if (inputData[1] > 100)
                {
                    streamWriter.WriteLine("Out of range exception!");
                }
                else
                {
                    streamWriter.WriteLine(GetAnswerFromSecondSatellite(inputData[0], inputData[1]));
                }
            }
        }

        private static string GetAnswerFromSecondSatellite(int sumOfDigits, int countOfDigits)
        {
            int firstNumber = 0, sumForFirst = sumOfDigits, numberOfDigitForFirst = 1;
            int secondNumber = Convert.ToInt32(Math.Pow(10, countOfDigits-1)), sumForSecond = sumOfDigits-1, numberOfDigitForSecond = countOfDigits;
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