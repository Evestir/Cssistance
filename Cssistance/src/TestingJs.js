(function () {
  const divs = document.getElementsByTagName('div'); 
  const searchString = {};
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
      var pattern = new RegExp("piece " + searchString + " square-", "g");
      return foundDiv.replace(pattern, '');
  } else {
      return "none";
  }
})(); 



if (document.getElementsByClassName('board').length > 0) {
    console.log(document.getElementsByClassName('board').length);
}

(function () {if (document.getElementsByClassName('board').length > 0) {
    return true;
}
})();

if (document.getElementById('board-layout-pieces').className == "board"){
    console.log("fuckyou")
}


(function () {
    const textElements = document.querySelectorAll('text.coordinate-light');
  
    if (textElements.length == 0) {
        return "GameNotInit";
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
})();


// Get all <text> elements with the class "coordinate-light"
const textElements = document.querySelectorAll('text.coordinate-light');

if (textElements.length > 0) {
  // Loop through each <text> element
  textElements.forEach((textElement, index) => {
    // Get the x and y coordinates
    const xCoordinate = textElement.getAttribute('x');
    const yCoordinate = textElement.getAttribute('y');

    // Get the text content
    const textContent = textElement.textContent;

    // Output the results for each element
    console.log(`Text ${index + 1}:`);
    console.log(`x-coordinate: ${xCoordinate}`);
    console.log(`y-coordinate: ${yCoordinate}`);
    console.log(`Text content: ${textContent}`);
    console.log('-----------------------');

    // Check if the text content is 'h'
    if (textContent === 'h') {
      console.log('Text content is "h".');
    } else {
      console.log('Text content is not "h".');
    }
  });
} else {
  console.log('No elements found with the class "coordinate-light".');
}

(function () {
  // Get the aboveDiv element
  var aboveDiv = (function() {
    var divs = document.getElementsByTagName("div");
    for(var i = 0; i < divs.length; i++){
      if (divs[i].classList.contains("square-88")){
        return divs[i];
      }
    }
    return null;
  })();
  var sdsdsdf = document.getElementById('board-layout-chessboard');

  // Get its computed style (including padding and border)
  // Get the width and height of aboveDiv including padding and border
  var ComputedStyle = window.getComputedStyle(aboveDiv);

  // Create the squareDiv dynamically
  const squareDiv = document.createElement("div");
  squareDiv.id = "squareDiv";
  squareDiv.style.opacity = "0.4"
  squareDiv.style.position = 'absolute';
  squareDiv.style.left = '30px';
  squareDiv.style.transform = ComputedStyle.transform;
  squareDiv.style.width = ComputedStyle.width;
  squareDiv.style.height = ComputedStyle.height;
  squareDiv.style.backgroundColor = "red";
  squareDiv.style.zIndex = "9999";

  sdsdsdf.appendChild(squareDiv);
})(); 

(function () {
  // Get the aboveDiv element
  while (document.getElementById('squareDiv') != null){
    document.getElementById('squareDiv').remove();
  }
})(); 


(function() {
  var divs = document.getElementsByTagName("div");
  for(var i = 0; i < divs.length; i++){
    if (divs[i].classList.contains("square-88")){
      return divs[i];
    }
  }
  return null;
})();

(function () {
  // Get the aboveDiv element
  var TargetPiece = (function() {
    var divs = document.getElementsByTagName("div");
    for(var i = 0; i < divs.length; i++){
      if (divs[i].classList.contains('square-42')){
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
  const TargetPieceAbove = document.createElement("div");
  TargetPieceAbove.id = "squareDiv";
  TargetPieceAbove.style.pointerEvents = "none";
  TargetPieceAbove.style.opacity = "0.4"
  TargetPieceAbove.style.position = 'absolute';
  TargetPieceAbove.style.left = '30px';
  TargetPieceAbove.style.transform = ComputedStyle.transform;
  TargetPieceAbove.style.width = ComputedStyle.width;
  TargetPieceAbove.style.height = ComputedStyle.height;
  TargetPieceAbove.style.backgroundColor = "red";
  TargetPieceAbove.style.zIndex = "9999";

  const TargetBlock = document.createElement("div");
  TargetBlock.id = "squareDiv";
  TargetBlock.style.pointerEvents = "none";
  TargetBlock.style.opacity = "0.4"
  TargetBlock.style.position = 'absolute';
  TargetBlock.style.left = 30 + (parseFloat(ComputedStyle.width)*0) + 'px';
  TargetBlock.style.top = -(parseFloat(ComputedStyle.width)*2) + 'px';
  TargetBlock.style.transform = ComputedStyle.transform;
  TargetBlock.style.width = ComputedStyle.width;
  TargetBlock.style.height = ComputedStyle.height;
  TargetBlock.style.backgroundColor = "red";
  TargetBlock.style.zIndex = "9999";

  sdsdsdf.appendChild(TargetBlock);
  sdsdsdf.appendChild(TargetPieceAbove);
})(); 