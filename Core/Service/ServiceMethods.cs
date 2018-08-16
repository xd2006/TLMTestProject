namespace Core.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// The service methods.
    /// </summary>
    public class ServiceMethods
    {
        /// <summary>
        /// Get random numbers.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="toCount">
        /// The to count.
        /// </param>
        /// <param name="numberOfElements">
        /// The number of elements.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<int> GetRandomNumbers(int from, int toCount, int numberOfElements)
        {
            Random random = new Random();
            return Enumerable.Range(from, toCount).OrderBy(n => random.Next()).Take(numberOfElements).ToList();
        }


        public static T GetRandom<T>(List<T> list)
        {
            return GetRandom(list, 1)[0];
        }
            
        public static List<T> GetRandom<T>(List<T> list, int numberOfElements)
        {
            List<T> resultsList = new List<T>();
            Random random = new Random();

            numberOfElements = numberOfElements <= list.Count ? numberOfElements : list.Count;
            
                var elementsNumbers = Enumerable.Range(0, list.Count).OrderBy(n => random.Next()).Take(numberOfElements)
                    .ToList();

            foreach (var e in elementsNumbers)
            {
                resultsList.Add(list[e]);
            }

            return resultsList;

        }

        /// <summary>
        /// The list to string converter.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The string result <see cref="string"/>.
        /// </returns>
        public static string ListToString<T>(List<T> list)
        {
            StringBuilder itemsString = new StringBuilder();
            foreach (var item in list)
            {
                itemsString.Append(Environment.NewLine + item);
            }

            return itemsString.ToString();
        }

        /// <summary>
        /// The compare lists.
        /// </summary>
        /// <param name="list1">
        /// The list 1.
        /// </param>
        /// <param name="list2">
        /// The list 2.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// If lists are equal disregarding seqience<see cref="bool"/>.
        /// </returns>
        public static bool CompareLists<T>(List<T> list1, List<T> list2)
        {
            var hashSet1 = new HashSet<T>(list1);
            var hashSet2 = new HashSet<T>(list2);
            return hashSet1.SetEquals(hashSet2) && list1.Count.Equals(list2.Count);
        }

        public static string RemoveMultipleInnerWhitespaces(string text)
        {
            return ReplaceStringInString(text, "\\s+", " ");
        }

        public static string RemoveInnerWhitespaces(string text)
        {
            return ReplaceStringInString(text, "\\s+", string.Empty);
        }

        public static string ReplaceStringInString(string text, string regexp, string toReplace)
        {
            return new Regex(regexp).Replace(text, toReplace);
        }

        public static string RemoveStringInString(string text, string regexp)
        {
            return ReplaceStringInString(text, regexp, string.Empty);
        }

        public static string ListToString(List<string> list)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in list)
            {
                builder.AppendLine(item);
            }

            return builder.ToString();
        }

        public static List<string> RemoveStringsInList(IEnumerable<string> elements, List<string> elementsToRemove)
        {
            List<string> processedElements = new List<string>();
            List<string> tempElements = elements.ToList();
            foreach (var eR in elementsToRemove)
            {
                processedElements = new List<string>();
                tempElements.ForEach(e => processedElements.Add(Regex.Replace(e, eR, string.Empty).Trim()));
                tempElements = new List<string>(processedElements);
            }

            return processedElements;
         }

        public static List<string> RemoveWhiteSpacesInList(IEnumerable<string> elements)
        {
            return RemoveStringsInList(elements, new List<string>() { "\\s+" });
        }


        public static List<string> StringListToUpper(List<string> list)
        {
            var listToUpper = new List<string>();
            list.ForEach(e => listToUpper.Add(e.ToUpper()));
            return listToUpper;
        }

        public static void KillAllEdgeDriversForCurrentUser()
        {
            KillAllProcessesForCurrentUser("MicrosoftWebDriver");
        }

        public static void KillAllEdgeBrowsersForCurrentUser()
        {
            KillAllProcessesForCurrentUser("MicrosoftEdge");
        }



        public static void KillAllProcessesForCurrentUser(string processName)
        {
            // get the current user session id
            Process currentProcess = Process.GetCurrentProcess();
            int currentSessionID = currentProcess.SessionId;

            // get the list of processes
            Process[] processlist1 = Process.GetProcesses();

            // kill all "iedriver" processes for the current user
            try
            {

                foreach (Process theprocess in processlist1)
                    if (theprocess.ProcessName.Equals(processName) && (theprocess.SessionId == currentSessionID))
                    {
                        theprocess.Kill();
                        theprocess.Dispose();
                    }
            }
            catch (Exception)
            {
                //supress exception
            }

        }

        public static string ConvertEdgeDateToUtf(string text)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);
            return Encoding.UTF8.GetString(bytes).Replace("?", string.Empty);
        }

        public static int ConvertDurationFromTimeFormatToSeconds(string duration)
        {
            var durationData = duration.Split(':');
            int timeMinutes = 0;
            timeMinutes += int.Parse(durationData[0]) * 3600; //seconds in a hour
            timeMinutes += int.Parse(durationData[1]) * 60; //seconds in a minute
            timeMinutes += int.Parse(durationData[2].Replace("h", string.Empty)); //seconds themselves

            return timeMinutes;
        }

        public static void WaitForOperationPositive(Func<bool> operation, int timeoutSec = 10)
        {
            int counter = 0;
            while (!operation.Invoke() && counter++ < timeoutSec)
            {
                Thread.Sleep(1000);
            }

            if (counter >= timeoutSec)
            {
                throw new Exception($"Timeout error. Was being waiting for {timeoutSec} sec");
            }
        }

        public static void WaitForOperationNegative(Func<bool> operation, int timeoutSec = 10)
        {
            int counter = 0;
            while (operation.Invoke() && counter++ < timeoutSec)
            {
                Thread.Sleep(1000);
            }

            if (counter >= timeoutSec)
            {
                throw new Exception($"Timeout error. Was being waiting for {timeoutSec} sec");
            }
        }
    }
}
