
window.onload = function() {
  var delay = 800;
  var first = true;
  var ready = true;
  var previousGuess;
  var matched = 0;
  var tiles = document.querySelectorAll('.tile');
  var images = ['bird.jpg', 'carrot.png', 'cherry.png', 'citron.png', 'elephant.png', 'frog.png', 'juice.png',
                'kiwi.png', 'orange.jpg', 'pineapple.png', 'pumpkin.png', 'radish.png', 'sandwich.png',
                'sea-dog.png', 'shark.png', 'tomato.png', 'tortoise.png', 'watermelon.png'];
  function shuffle(array) {
    var currentIndex = array.length;
    while (0 !== currentIndex) {
      var randomIndex = Math.floor(Math.random() * currentIndex);
      currentIndex -= 1;
      var temporaryValue = array[currentIndex];
      array[currentIndex] = array[randomIndex];
      array[randomIndex] = temporaryValue;
    }
    return array;
  }
  function hideAll() {
    for (tile of tiles) {
      tile.classList.add('hidden');
    }
    ready = true;
    first = true;
    previousGuess = undefined;
  }
  function solve(firstTile, secondTile) {
    firstTile.classList.add('solved');
    secondTile.classList.add('solved');
    ready = true;
    first = true;
    previousGuess = undefined;
    for (var i = 0; i < tiles.length; i++) {
      if (tiles[i].classList.contains('solved') === false) {
        return;
      }
    }
    var board = document.getElementById('game-board');
    var mainView = document.querySelector('main');
    board.style.display = 'none';
    mainView.style.paddingTop = '300px';
    mainView.innerHTML = 'Congratulations! :)';
  }
  function handleClick() {
    if (ready === false || this === previousGuess) {
      return;
    }
    this.classList.remove('hidden');
    var thisImage = this.style.backgroundImage;
    if (first === true) {
      previousGuess = this;
      first = false;
      return;
    }
    else if (previousGuess.style.backgroundImage === thisImage) {
      ready = false;
      thisGuess = this;
      window.setTimeout(function() {
        solve(previousGuess, thisGuess)
      }, delay);
    }
    else {
      ready = false;
      window.setTimeout(hideAll, delay);
    }
  }
  images = images.concat(images);
  images = shuffle(images);
  for (var i = 0; i < tiles.length; i++) {
    tiles[i].style.backgroundImage = 'url("images/' + images[i] + '")';
    tiles[i].addEventListener('click', handleClick);
  }
}
