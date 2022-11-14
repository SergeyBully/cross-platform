using System.Dynamic;

namespace Lab3_35
{
    public class Program
    {
        public static string InputFilePath = @"..\..\input.txt";
        public static string OutputFilePath = @"..\..\output.txt";

        static void Main(string[] args)
        {
            FileInfo outputFileInfo = new FileInfo(OutputFilePath);
            var inputPath = File.ReadLines(InputFilePath).First();

            using (StreamWriter streamWriter = outputFileInfo.CreateText())
            {
                if (String.IsNullOrEmpty(inputPath) || inputPath.Length > 200)
                {
                    streamWriter.WriteLine("Out of range exception!");
                }
                else
                {
                    streamWriter.WriteLine(GetSmallestPath(inputPath));
                }
            }
        }

        /// <summary>
        /// Отримати найкоротший шлях
        /// </summary>
        public static string GetSmallestPath(string inputPath)
        {
            int currRow = 0, currCol = 0, minRow = 0, minCol = 0, maxRow = 0, maxCol = 0;
            foreach(var dir in inputPath)
            {
                switch (dir)
                {
                    case 'N':
                        currRow--;
                        minRow = minRow > currRow ? currRow : minRow;
                        break;
                    case 'S':
                        currRow++;
                        maxRow = maxRow < currRow ? currRow : maxRow;
                        break;
                    case 'W':
                        currCol--;
                        minCol = minCol > currCol ? currCol : minCol;
                        break;
                    case 'E':
                        currCol++;
                        maxCol = maxCol < currCol ? currCol : maxCol;
                        break;
                }
            }

            int rowCount = maxRow - minRow + 1;
            int colCount = maxCol - minCol + 1;
            (int row, int col) startPos = (rowCount - maxRow - 1, colCount - maxCol - 1);
            int[,] arr = new int[rowCount, colCount];
            var currPos = startPos;
            arr[currPos.row, currPos.col] = 1;
            foreach(var dir in inputPath)
            {
                switch (dir)
                {
                    case 'N':
                        currPos.row--;
                        break;
                    case 'S':
                        currPos.row++;
                        break;
                    case 'W':
                        currPos.col--;
                        break;
                    case 'E':
                        currPos.col++;
                        break;
                }
                arr[currPos.row, currPos.col] = 1;
            }
            var finPos = currPos;

            return getSmallestPath(startPos, finPos, arr);
        }

        private static string getSmallestPath((int row, int col) finPos, (int row, int col) startPos, int[,] arr)
        {
            if (finPos == startPos)
            {
                return string.Empty;
            }
            arr[startPos.row, startPos.col] = 2;
            List<string> pathList = new List<string>();
            foreach(var newPos in newPosList(arr, startPos))
            {
                int[,] buf = arr.Clone() as int[,];
                pathList.Add(newPos.dir + getSmallestPath(finPos, (newPos.row, newPos.col), buf));
            }
            pathList = pathList.Where(p => !p.Contains('-')).OrderBy(p => p.Length).ToList();
            if (!pathList.Any())
            {
                return "-";
            }
            pathList = pathList.Where(p => p.Length == pathList[0].Length).ToList();
            return pathList.Count > 1 ? getBetterPathWithPriority(pathList) : pathList.First(); ;
        }

        private static string getBetterPathWithPriority(List<string> pathList)
        {
            for (int i = 0; i < pathList[0].Length; i++)
            {
                bool isEqual = true;
                var iLetters = pathList.Select(p => p[i]).ToList();
                for (int j = 1; j < iLetters.Count; j++)
                {
                    if (iLetters[j] != iLetters[0])
                    {
                        isEqual = false;
                        break;
                    }
                }
                if (!isEqual)
                {
                    if (iLetters.Contains('N'))
                    {
                        pathList = pathList.Where(p => p[i] == 'N').ToList();
                    }
                    else if (iLetters.Contains('E'))
                    {
                        pathList = pathList.Where(p => p[i] == 'E').ToList();
                    }
                    else
                    {
                        pathList = pathList.Where(p => p[i] == 'S').ToList();
                    }
                    if (pathList.Count == 1)
                    {
                        break;
                    }
                }
            }
            return pathList.First();
        }

        private static List<(int row, int col, char dir)> newPosList(int[,] arr, (int row, int col) curPos)
        {
            List<(int row, int col, char dir)> result = new List<(int row, int col, char dir)>();
            if (curPos.row != 0)
            {
                if (arr[curPos.row - 1, curPos.col] == 1)
                {
                    result.Add((curPos.row - 1, curPos.col, 'N'));
                }
            }
            if (curPos.col != 0)
            {
                if (arr[curPos.row, curPos.col - 1] == 1)
                {
                    result.Add((curPos.row, curPos.col - 1, 'W'));
                }
            }
            if (curPos.row != arr.GetLength(0)-1)
            {
                if (arr[curPos.row + 1, curPos.col] == 1)
                {
                    result.Add((curPos.row + 1, curPos.col, 'S'));
                }
            }
            if (curPos.col != arr.GetLength(1) - 1)
            {
                if (arr[curPos.row, curPos.col + 1] == 1)
                {
                    result.Add((curPos.row, curPos.col + 1, 'E'));
                }
            }

            return result;
        }
    }
}