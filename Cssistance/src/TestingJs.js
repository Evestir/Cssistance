(function () {
  const divs = document.getElementsByTagName('div'); 
  const searchString = 'bp';
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
      var pattern = new RegExp("piece " + searchString + " square-");
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