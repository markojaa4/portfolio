function mainScript() {
// Other variables
  var mainColor = "rgba(235, 94, 33, 0.8)";
  var currentNum = 0; // for carousel
// Node selectors
  var nav = document.querySelector("nav");
  var hamburger = document.getElementById("hamburger-button");
  var logo = document.querySelector("nav a:first-of-type");
  var menu = document.querySelector("nav > ul");
  var arrow = document.querySelector(".hamburger-menu-arrow");
  var collapseButtons = document.querySelectorAll("nav ul button");
  var topMenuItems = document.querySelectorAll(".top-menu > li div a");
  var submenus = document.querySelectorAll("nav ul li ul");
  var carouselButtons = document.querySelectorAll(".carousel button");
  var carouselContent = document.querySelectorAll(".carousel-content > div");
  var portfolioPrevious = document.querySelector(".portfolio-slider button:first-of-type");
  var portfolioNext = document.querySelector(".portfolio-slider button:last-of-type");
  var portfolioItems = document.querySelectorAll(".portfolio-slider .slider-item");
  var portfolioItemsParent = document.querySelector(".portfolio-slider > div");
  var servicesPrevious = document.querySelector(".services .slide-left-button");
  var servicesNext = document.querySelector(".services .slide-right-button");
  var servicesParent = document.querySelector(".services div");
  var testimonialsPrevious = document.querySelector(".testimonials .slide-left-button");
  var testimonialsNext = document.querySelector(".testimonials .slide-right-button");
  var testimonialsParent = document.querySelector(".testimonials div");
// Custom function definitions
  function addClass(elementNode, newClass) {
    if (elementNode) {
      if (elementNode.className) {
        elementNode.className += " " + newClass;
      } else {
        elementNode.className = newClass;
      }
    }
  }

  function removeClass(elementNode, classToRemove) {
    if (elementNode) {
      if (elementNode.className.indexOf(" ") >= 0) {
        elementNode.className = elementNode.className.replace(
          " " + classToRemove,
          ""
        );
      } else {
        elementNode.className = elementNode.className.replace(
          classToRemove,
          ""
        );
      }
    }
  }

  function showMenu() {
    hamburger.style.borderColor = mainColor;
    hamburger.style.backgroundColor = mainColor;
    addClass(logo, "display-none");
    addClass(menu, "display-block");
    addClass(arrow, "display-block");
  }

  function hideMenu() {
    hamburger.style.borderColor = "rgb(133, 133, 133)";
    hamburger.style.backgroundColor = "inherit";
    removeClass(logo, "display-none");
    removeClass(menu, "display-block");
    removeClass(arrow, "display-block");
  }
// Constructors
  function Submenu(collapseButton, topItem, submenu) {
    this.collapseButton = collapseButton;
    this.topItem = topItem;
    this.submenu = submenu;
    this.subItems = this.submenu.querySelectorAll("a");

    this.displayIt = function() {
      removeClass(this.submenu, "display-none");
    }
    this.undisplay = function() {
      if (this.submenu.className.indexOf("display-none") < 0) {
        addClass(this.submenu, "display-none");
      }
    }

    this.displayIt = this.displayIt.bind(this);
    this.undisplay = this.undisplay.bind(this);

    this.collapseButton.onclick = function(e) {
      var arrow = e.currentTarget.querySelector("div");
      if (arrow.className.indexOf("menu-arrow-right") >= 0) {
        this.topItem.style.color = "rgb(235, 94, 33)";
        removeClass(arrow, "menu-arrow-right");
        addClass(arrow, "menu-arrow-down");
        removeClass(submenu, "display-none");
      } else {
        this.topItem.style.color = "rgb(69, 69, 69)";
        removeClass(arrow, "menu-arrow-down");
        addClass(arrow, "menu-arrow-right");
        addClass(submenu, "display-none");
      }
    }.bind(this)
    // responsive menu
    if (document.body.clientWidth >= 750) {
      this.topItem.onmouseover = function() {
        this.displayIt();
      }.bind(this)
      this.topItem.onmouseout = function() {
        this.undisplay();
      }.bind(this)
      this.submenu.onmouseover = function() {
        this.displayIt();
      }.bind(this)
      this.submenu.onmouseout = function() {
        this.undisplay();
      }.bind(this)
      this.submenu.style.width = this.topItem.clientWidth + "px";
    } else {
    // for when switching back
      this.topItem.onmouseover = function() {};
      this.topItem.onmouseout = function() {};
      this.submenu.onmouseover = function() {};
      this.submenu.onmouseout = function() {};
      this.submenu.style.width = "";
    }
  }

  function CarouselItem(ordinalNum) {
    this.ordinalNum = ordinalNum;
    this.carouselButton = carouselButtons[ordinalNum];
    this.carouselContent = carouselContent[ordinalNum];
    this.carouselChange = function() {
      var animationPercent = 70;
      var interval = setInterval(function() {
        if (animationPercent > 0) {
          animationPercent-= 5;
          this.carouselContent.style.left = animationPercent + "%";
        } else {
          clearInterval(interval);
        }
      }.bind(this), 10);
      for (var i = 0; i < carouselButtons.length; i++) {
        carouselButtons[i].style.backgroundColor = "white";
        if (carouselContent[i].className.indexOf("display-none") < 0) {
          addClass(carouselContent[i], "display-none");
        }
      }
      removeClass(this.carouselContent, "display-none");
      this.carouselButton.style.backgroundColor = "rgb(235, 94, 33)";
    }
    this.carouselChange = this.carouselChange.bind(this);
    this.carouselButton.onclick = function() {
      this.carouselChange();
      currentNum = this.ordinalNum;
    }.bind(this)
  };

  function Slider(fwdBtn, bwdBtn, parentElement, slots) {
    this.slots = slots;
    this.fwdBtn = fwdBtn;
    this.bwdBtn = bwdBtn;
    this.parentElement = parentElement;
    this.content = this.parentElement.querySelectorAll(".slider-item");
    this.fwdBtn.onclick = function() {
      for (var i = 0; i < this.content.length; i++) {
        this.content[i].style.marginRight = "";
      }
      this.content[0].style.display = "none";
      var removedContent = this.parentElement.removeChild(this.content[0]);
      this.parentElement.appendChild(removedContent);
      this.content = this.parentElement.querySelectorAll(".slider-item");
      this.content[slots - 1].style.display = "inline-block";
      this.content[slots - 1].style.marginRight = "0";
    }.bind(this)
    this.bwdBtn.onclick = function() {
      for (var i = 0; i < this.content.length; i++) {
        this.content[i].style.display = "none";
        this.content[i].style.marginRight = "";
      }
      for (var i = 0; i < this.slots; i++) {
        var removedContent = this.parentElement.removeChild(this.content[0]);
        this.parentElement.appendChild(removedContent);
        this.content = this.parentElement.querySelectorAll(".slider-item");
      }
      for (var i = 0; i < this.slots; i++) {
        this.content[i].style.display = "inline-block";
      }
      this.content[slots - 1].style.marginRight = "0";
    }.bind(this)
  }

  function PortfolioItem(ordinalNum) {
    this.item = portfolioItems[ordinalNum];
    this.anchor = this.item.querySelector("a");
    this.item.onmouseover = function() {
      this.anchor.style.display = "inline-block";
    }.bind(this)
    this.item.onmouseout = function() {
      this.anchor.style.display = "none";
    }.bind(this)
  }
// hamburger click listener
  hamburger.onclick = function() {
    if (logo.className.indexOf("display-none") < 0) {
      showMenu();
    } else {
      hideMenu();
    }
  }
// Iterate Submenu objects into an array
  var submenuArr = [];
  for (var i = 0; i < topMenuItems.length; i++) {
    var submenu = new Submenu(collapseButtons[i], topMenuItems[i], submenus[i]);
    submenuArr.push(submenu);
  }
// Reiterate on resize for when switching screen size back and forth
  window.addEventListener("resize", function() {
    submenuArr = [];
    for (var i = 0; i < topMenuItems.length; i++) {
      var submenu = new Submenu(collapseButtons[i], topMenuItems[i], submenus[i]);
      submenuArr.push(submenu);
    }
  });
// Iterate CarouselButton objects into array
  var carouselObjectArr = [];
  for (var i = 0; i < carouselButtons.length; i++) {
    var carouselItem = new CarouselItem(i);
    carouselObjectArr.push(carouselItem);
  }
  var carouselInterval = window.setInterval(function() {
    if (currentNum < carouselButtons.length - 1) {
      currentNum++;
    } else {
      currentNum = 0;
    }
    carouselObjectArr[currentNum].carouselChange();
  }, 20000);
// Iterate Portfolio objects
  var portfolioObjectArr = [];
  for (var i = 0; i < portfolioItems.length; i++) {
    var portfolioItem = new PortfolioItem(i);
    portfolioObjectArr.push(portfolioItem);
  }
  var portfolioSlider = new Slider(portfolioNext, portfolioPrevious, portfolioItemsParent, 3);
/*
  if (document.body.clientWidth < 400) {
    portfolioSlider = new Slider(portfolioNext, portfolioPrevious, portfolioItemsParent, 1);
  } else if (document.body.clientWidth < 800) {
    portfolioSlider = new Slider(portfolioNext, portfolioPrevious, portfolioItemsParent, 2);
  } else {
    portfolioSlider = new Slider(portfolioNext, portfolioPrevious, portfolioItemsParent, 3);
  }
  if (document.body.clientWidth > 1366) {
      portfolioSlider.content[i].style.display = "none";
  } else {
    if (document.body.clientWidth > 399) {
      for (var i = 1; i < portfolioSlider.content.length; i++) {
        portfolioSlider.content[i].style.display = "inline-block";
      }
    } else if (document.body.clientWidth > 799) {
        for (var i = 2; i < portfolioSlider.content.length; i++) {
        portfolioSlider.content[i].style.display = "inline-block";
      }       
    } else {
      for (var i = 1; i < portfolioSlider.content.length; i++) {
        portfolioSlider.content[i].style.display = "none";
      }
    }
  }
*/
  var servicesSlider;
  if (document.body.clientWidth < 800) {
    servicesSlider = new Slider(servicesNext, servicesPrevious, servicesParent, 1);
  } else {
    servicesSlider = new Slider(servicesNext, servicesPrevious, servicesParent, 2);
  }
  if (document.body.clientWidth > 1366) {
    if (servicesNext.className.indexOf("display-none") < 0) {
      addClass(servicesNext, "display-none");
    }
    if (servicesPrevious.className.indexOf("display-none") < 0) {
      addClass(servicesPrevious, "display-none");
    }
    for (var i = 0; i < servicesSlider.content.length; i++) {
      removeClass(servicesSlider.content[i], "display-none");
    }
    servicesSlider.content[1].style.marginRight = "";
  } else {
    removeClass(servicesNext, "display-none");
    removeClass(servicesPrevious, "display-none");
    if (document.body.clientWidth > 799) {
      for (var i = 2; i < servicesSlider.content.length; i++) {
        if (servicesSlider.content[i].className.indexOf("display-none")) {
          addClass(servicesSlider.content[i], "display-none");
        }
      }
      servicesSlider.content[1].style.marginRight = "0";
    } else {
      for (var i = 1; i < servicesSlider.content.length; i++) {
        if (servicesSlider.content[i].className.indexOf("display-none")) {
          addClass(servicesSlider.content[i], "display-none");
        }
      }
      servicesSlider.content[0].style.marginRight = "0";
    }
  }

  var testimonialsSlider;
  if (document.body.clientWidth < 1200) {
    testimonialsSlider = new Slider(testimonialsNext, testimonialsPrevious, testimonialsParent, 1);
  } else {
    testimonialsSlider = new Slider(testimonialsNext, testimonialsPrevious, testimonialsParent, 2);
  }
  if (document.body.clientWidth > 1366) {
    if (testimonialsNext.className.indexOf("display-none") < 0) {
      addClass(testimonialsNext, "display-none");
    }
    if (testimonialsPrevious.className.indexOf("display-none") < 0) {
      addClass(testimonialsPrevious, "display-none");
    }
    for (var i = 0; i < testimonialsSlider.content.length; i++) {
      removeClass(testimonialsSlider.content[i], "display-none");
    }
    testimonialsSlider.content[1].style.marginRight = "";
  } else {
    removeClass(testimonialsNext, "display-none");
    removeClass(testimonialsPrevious, "display-none");
    if (document.body.clientWidth > 1199) {
      for (var i = 2; i < testimonialsSlider.content.length; i++) {
        if (testimonialsSlider.content[i].className.indexOf("display-none")) {
          addClass(testimonialsSlider.content[i], "display-none");
        }
      }
      testimonialsSlider.content[1].style.marginRight = "0";
    } else {
      for (var i = 1; i < testimonialsSlider.content.length; i++) {
        if (testimonialsSlider.content[i].className.indexOf("display-none")) {
          addClass(testimonialsSlider.content[i], "display-none");
        }
      }
      testimonialsSlider.content[0].style.marginRight = "0";
    }
  }
}

window.addEventListener("load", function() {
  mainScript();
});
