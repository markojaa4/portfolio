$(document).ready(function() {

  // canvas
  function draw() {
    var bodyElement = document.querySelector('body');
    var canvas = document.getElementById('canvas');
    var ctx = canvas.getContext('2d');
    canvas.width = bodyElement.scrollWidth;
    canvas.height = bodyElement.scrollHeight;

    function Star() {
      this.positionX = Math.floor(Math.random() * canvas.width);
      this.positionY = Math.floor(Math.random() * canvas.height);
      this.brightness = Math.floor(Math.random() * (3 - 1) + 1);
    }
    function BrightStar() {
      this.positionX = Math.floor(Math.random() * canvas.width);
      this.positionY = Math.floor(Math.random() * canvas.height);
      this.brightness = 2.5;
    }

    // generate dim stars; density proportional to screen size
    for (var i = 1; i <= canvas.width * canvas.height / 10000 + 50; i++) {
      var star = new Star;
      var radgrad = ctx.createRadialGradient(star.positionX, star.positionY, 0.5, star.positionX, star.positionY, star.brightness);
      radgrad.addColorStop(0, 'white');
      radgrad.addColorStop(1, 'black');
      ctx.fillStyle = radgrad;
      ctx.beginPath();
      ctx.arc(star.positionX, star.positionY, star.brightness, 0, Math.PI * 2, true);
      ctx.fill();
    }

    // generate bright stars; density proportional to screen size
    for (var i = 1; i <= canvas.width * canvas.height / 10000 - 120; i++) {
      var star = new BrightStar;
      var radgrad = ctx.createRadialGradient(star.positionX, star.positionY, 0.5, star.positionX, star.positionY, star.brightness);
      radgrad.addColorStop(0, 'white');
      radgrad.addColorStop(1, 'black');
      ctx.fillStyle = radgrad;
      ctx.beginPath();
      ctx.arc(star.positionX, star.positionY, star.brightness, 0, Math.PI * 2, true);
      ctx.fill();
    }
  }

  var winWidth = $(window).width();
  function resizeAndDraw() {
    if ($(window).width() != winWidth) {
      draw();
      winWidth = $(window).width();
    }
  }

  if(canvas.getContext) {
    draw();
    window.addEventListener('resize', resizeAndDraw, false);
  }

  // mute button
  var muteButton = document.getElementById('mute');
  var bgMusic = document.getElementById('bg-music');
  var isMute = true;
  var isPlaying = false;
  muteButton.onclick = function() {
    muteButton.blur();
    if (isPlaying === false) {
      bgMusic.play();
      isPlaying = true;
    }
    if (isMute === false) {
      isMute = true;
      bgMusic.muted = true;
      muteButton.innerHTML = '<span class="glyphicon glyphicon-volume-off" aria-hidden="true"></span>';
    } else {
      isMute = false;
      bgMusic.muted = false;
      muteButton.innerHTML = '<span class="glyphicon glyphicon-volume-up" aria-hidden="true"></span>';
    }
  }

  // AJAX
  var planetButtons = document.querySelectorAll('.solar-nav ul button');

  function buildPage(button) {
    if (button.innerHTML.indexOf('(current)') < 0) {
      for (var j = 0; j < planetButtons.length; j++) {
        if (planetButtons[j].innerHTML.indexOf('(current)') >= 0) {
          planetButtons[j].innerHTML = planetButtons[j].innerHTML.substring(0, planetButtons[j].innerHTML.indexOf('<span'));
        }
      }
      draw();
      var planetCase = button.textContent;
      var planet = planetCase.toLowerCase();
      var planetImage = document.getElementById('planet-img');
      planetImage.setAttribute('src', 'assets/' + planet + '.png');
      planetImage.setAttribute('alt', 'Planet ' + planetCase);
      var statusBarSubdiv = document.querySelector('.status-bar div');
      statusBarSubdiv.innerHTML = '<p>Currently orbiting ' + planetCase + '<span>...</span></p>';

      var headElement = document.querySelector('head');
      var styleLink = document.getElementById('specific-style');
      if (styleLink) {
        headElement.removeChild(styleLink);
      }
      var newLink = document.createElement('link');
      newLink.setAttribute('id', 'specific-style');
      newLink.setAttribute('rel', 'stylesheet');
      newLink.setAttribute('href', 'style/' + planet + '.css');
      headElement.appendChild(newLink);

      // json
      $.ajaxSetup({
        beforeSend: function(xhr) {
          if (xhr.overrideMimeType) {
            xhr.overrideMimeType('application/json');
          }
        }
      });
      $.getJSON('json/' + planet + '.json', function(json) {
        var planetData = json;
        var listTitles = document.querySelectorAll('.planet-info dt');
        var listDescriptions = document.querySelectorAll('.planet-info dd');
        for (var j = 0; j < listTitles.length; j++) {
          listTitles[j].innerHTML = Object.keys(planetData)[j] + ':';
        }
        for (var j = 0; j < listDescriptions.length; j++) {
          listDescriptions[j].innerHTML = Object.values(planetData)[j];
        }
      });

      button.innerHTML += '<span class="sr-only">(current)</span>';
    }
  }

  for (var i = 0; i < planetButtons.length; i++) {
    planetButtons[i].onclick = function() {
      for (var j = 0; j < planetButtons.length; j++) {
        planetButtons[j].removeAttribute('class');
      }
      this.setAttribute('class', 'active');
      buildPage(this);
      window.scrollTo(0, 0);
    };
  }
});
