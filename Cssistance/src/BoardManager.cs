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
              const searchString = '" + query + @"';
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

        private static Dictionary<char, int> Alpha2Num = new Dictionary<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
            { 'c', 3 },
            { 'd', 4 },
            { 'e', 5 },
            { 'f', 6 },
            { 'g', 7 },
            { 'h', 8 }
        };
            

        public static void DrawIndication(string Notation)
        {
            string TargetPiece = "square-" + Alpha2Num[Notation[0]].ToString() + Notation[1].ToString();
            int RelX = Alpha2Num[Notation[2]] - Alpha2Num[Notation[0]];
            int RelY = Notation[3] - Notation[1];
            if (GameStatus == "Black")
            {
                RelY = -RelY; 
            }

            Console.WriteLine(TargetPiece);
            string DrawFirstOne = @"(function () {
              // Get the aboveDiv element
              var TargetPiece = (function() {
                var divs = document.getElementsByTagName(""div"");
                for(var i = 0; i < divs.length; i++){
                  if (divs[i].classList.contains('" + TargetPiece + @"')){
                    return divs[i];
                  }
                }
                return null;
              })();
              var sdsdsdf = document.getElementById('board-layout-chessboard');

              // Get its computed style (including padding and border)
              // Get the width and height of aboveDiv including padding and border
              var ComputedStyle = window.getComputedStyle(TargetPiece);

              // Create the squareDiv dynamically
              const TargetPieceAbove = document.createElement(""div"");
              TargetPieceAbove.id = ""squareDiv"";
              TargetPieceAbove.style.opacity = ""0.4""
              TargetPieceAbove.style.position = 'absolute';
              TargetPieceAbove.style.left = '33px';
              TargetPieceAbove.style.transform = ComputedStyle.transform;
              TargetPieceAbove.style.width = ComputedStyle.width;
              TargetPieceAbove.style.height = ComputedStyle.height;
              TargetPieceAbove.style.backgroundColor = ""red"";
              TargetPieceAbove.style.zIndex = ""9999"";

              const TargetBlock = document.createElement(""div"");
              TargetBlock.id = ""squareDiv"";
              TargetBlock.style.opacity = ""0.4""
              TargetBlock.style.position = 'absolute';
              TargetBlock.style.left = (33 + (parseFloat(ComputedStyle.width)*" + RelX + @")) + 'px';
              TargetBlock.style.top = -(parseFloat(ComputedStyle.width)*" + RelY + @") + 'px';
              TargetBlock.style.transform = ComputedStyle.transform;
              TargetBlock.style.width = ComputedStyle.width;
              TargetBlock.style.height = ComputedStyle.height;
              TargetBlock.style.backgroundColor = ""red"";
              TargetBlock.style.zIndex = ""9999"";
              TargetPieceAbove.style.pointerEvents = 'none';
              TargetBlock.style.pointerEvents = 'none';

              sdsdsdf.appendChild(TargetBlock);
              sdsdsdf.appendChild(TargetPieceAbove);
            })(); ";
            
            EvaluateJS(DrawFirstOne);
        }

        public static void ClearIndications()
        {
            string Script = @"(function () {
              while (document.getElementById('squareDiv') != null){
                document.getElementById('squareDiv').remove();
              }
            })();";

            EvaluateJS(Script);
        }
    }
}
