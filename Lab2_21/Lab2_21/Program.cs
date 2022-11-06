namespace Lab2_21
{
    public class Program
    {
        public static string InputFilePath = @"..\..\input.txt";
        public static string OutputFilePath = @"..\..\output.txt";

        static void Main(string[] args)
        {
            FileInfo outputFileInfo = new FileInfo(OutputFilePath);
            var inputData = File.ReadLines(InputFilePath).ToList();
            int countOfWatches = Convert.ToInt32(inputData.First().Trim());
            List<Time> timeList = inputData.Skip(1).Select(t => new Time(t.Trim())).ToList();
            using (StreamWriter streamWriter = outputFileInfo.CreateText())
            {
                if (countOfWatches > 50000 || !timeList.Any() || timeList.Any(t => !t.IsNormalTime))
                {
                    streamWriter.WriteLine("Incorrect data!");
                }
                else
                {
                    streamWriter.WriteLine(GetOptimalTime(timeList));
                }
            }
        }

        public static Time GetOptimalTime(List<Time> timeList)
        {
            List<Tuple<Time, int>> buf = new List<Tuple<Time, int>>();
            for (int i = 0; i < timeList.Count; i++)
            {
                int fullTimeToSet = 0;
                for (int j = 0; j < timeList.Count; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    fullTimeToSet += timeList[i] - timeList[j];
                }
                buf.Add(new Tuple<Time, int>(timeList[i], fullTimeToSet));
            }
            return buf.OrderBy(t => t.Item2).ThenBy(t => t.Item1).First().Item1;
        }
    }
}
