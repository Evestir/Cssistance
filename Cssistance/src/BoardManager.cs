using CefSharp.DevTools.Browser;
using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Logging;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Cssistance.src
{
    internal class BoardManager {
        public static string GameStatus = "GameNotInit";

        private static object? EvaluateJSInternal(string javaScript, object? defaultResult)
        {
            var task = MainWindow.BrowserIns.EvaluateScriptAsync(javaScript);
            if (!task.Wait(TimeSpan.FromSeconds(5)))
                throw new TimeoutException("Javascript execution timed out!");
            return task.Result.Result ?? defaultResult;
        }

        public static string EvaluateJS(string javaScript, string defaultResult = " ", string? errorMessage = null)
        {
            try
            {
                return EvaluateJSInternal(javaScript, defaultResult)?.ToString() ?? defaultResult;
            }
            catch (NullReferenceException)
            {
                return defaultResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(errorMessage ?? "Failed to run script on site.");
                return defaultResult;
            }
        }

        private static string FindPieceName(string query)
        {
            string FoundPieceName = EvaluateJS(@"(function () {
              const divs = document.getElementsByTagName('div');
              const searchString = '"+ query + @"';
              let foundDiv = null;
  
              for (const div of divs) {
                  if (div.classList.contains(searchString)) {
                    if (foundDiv != null) {
                      foundDiv += ' ' + div.className;
                    }
                    else{
                      foundDiv = div.className;
                    }
                  }
              }
  
              if (foundDiv) {
                  var pattern = new RegExp(""piece "" + searchString + "" square-"", ""g"");
                  return foundDiv.replace(pattern, '');
              } else {
                  return ""none"";
              }
            })();");

            return FoundPieceName;
        }

        private static string[] ChessComPieceNames = new string[] {
            "br",
            "bn",
            "bb",
            "bk",
            "bq",
            "bp",
            "wp",
            "wr",
            "wn",
            "wb",
            "wk",
            "wq",
        };

        private static Dictionary<string, int> ChessComToNative = new Dictionary<string, int>
        {
            { "br", Piece.Black.Rook },
            { "bn", Piece.Black.Knight },
            { "bq", Piece.Black.Queen },
            { "bk", Piece.Black.King },
            { "bb", Piece.Black.Bishop },
            { "bp", Piece.Black.Pawn },
            { "wr", Piece.White.Rook },
            { "wn", Piece.White.Knight },
            { "wq", Piece.White.Queen },
            { "wk", Piece.White.King },
            { "wb", Piece.White.Bishop },
            { "wp", Piece.White.Pawn }
        };

        private static int Convert1to10(int receivedNum)
        {
            int tens = receivedNum / 10;
            int ones = receivedNum - (10 * tens);

            return ones * 10 + tens;
        }

        public static void FindPieces()
        {
            for (int i = 0; i < ChessComPieceNames.Count(); i++)
            {
                string Position = FindPieceName(ChessComPieceNames[i]);
                Console.WriteLine(Position);
                if (Position == "none")
                {
                    continue;
                }

                // Assuming you already have the FullName and the ExtractNumbersFromString method defined

                if (ChessComToNative.TryGetValue(ChessComPieceNames[i], out int nativeName))
                {
                    // Split the string into an array of substrings using the space character as the delimiter
                    string[] numbersArray = Position.Split(' ');

                    // Now, 'numbersArray' contains the individual numbers as separate elements
                    foreach (string number in numbersArray)
                    {
                        int PositionN = ExtractNumbersFromString(number);
                        Board.Coords.Add(Convert1to10(PositionN), nativeName);
                    }
                }
            }
            // Logging all key-value pairs in the Coords dictionary
            DisplayChessBoard();
        }

        public static void DisplayChessBoard()
        {
            // Logging all key-value pairs in the Coords dictionary
            foreach (var kvp in Board.Coords)
            {
                Console.WriteLine($"Key: {kvp.Key}, Value: {ChessComToNative.FirstOrDefault(x => x.Value == kvp.Value).Key}");
            }
        }

        public static async void SetMySide()
        {
            var scriptt = @"(function () {
                const textElements = document.querySelectorAll('text.coordinate-light');
  
                if (textElements.length == 0) {
                    return ""GameNotInit"";
                }

                for (let i = 0; i < textElements.length; i++) {
                  const textElement = textElements[i];
  
                  if (textElement.textContent === 'h') {
                    const xCoords = textElement.getAttribute('x');
                    if (xCoords == 97.5) {
                      return true;
                    }
                  }
                }
  
                return false;
            })();";

            string Side = EvaluateJS(scriptt);

            if (Side == "True")
            {
                Console.WriteLine("Ah! I see you're on white side!");
                GameStatus = "White";          
            }
            else if (Side == "False")
            {
                Console.WriteLine("Ah! I see you're on black side!");
                GameStatus = "Black";
            }
            else if (Side == "GameNotInit")
            {
                Console.WriteLine("Waiting for you to join a game!");
                GameStatus = "Waiting For The Game";
            }
        }

        static int ExtractNumbersFromString(string input)
        {
            // Define a regular expression pattern to match digits
            string pattern = @"\d+";

            // Create a regular expression object with the pattern
            Regex regex = new Regex(pattern);

            // Use the Matches method to find all matches in the input string
            MatchCollection matches = regex.Matches(input);

            // Join the matched numbers into a single string
            string result = string.Join("", matches);

            return int.Parse(result);
        }
    }
}
