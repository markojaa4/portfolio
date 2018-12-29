jQuery(document).ready(function($) {
  if ($.browser.msie && $.browser.version == 9) {
    $("html").addClass("ie9");
  }
  function Slider(leftArrow, rightArrow, pagerButtons, sliderItems) {
    this.leftArrow = leftArrow;
    this.rightArrow = rightArrow;
    this.pagerButtons = pagerButtons;
    this.sliderItems = sliderItems;
    this.slideCounter = 0;
    this.sliderReady = true;
    this.makeReady = function() {
      this.sliderReady = true;
    }.bind(this)
    this.sliderItems.fadeOut(0);
    $(this.sliderItems.get(0)).fadeIn(0);
    this.slideFwd = function() {
      if (this.slideCounter < this.sliderItems.length - 1) {
        this.slideCounter++;
      } else {
        this.slideCounter = 0;
      }
      for (var i = 0; i < this.pagerButtons.length; i++) {
        if (i === this.slideCounter) {
          this.pagerButtons.get(i).setAttribute("data-slide-active", "1");
        } else {
          this.pagerButtons.get(i).setAttribute("data-slide-active", "0");
        }
      }
      this.sliderItems.fadeOut(1000);
      $(this.sliderItems.get(this.slideCounter)).fadeIn(600);
      this.pagerButtons.removeClass("active-bullet-point");
      $(this.pagerButtons.get(this.slideCounter)).addClass("active-bullet-point");
      clearInterval(this.slideTimer);
      this.slideTimer = setInterval(function() {
        this.slideFwd();
      }.bind(this), 10000);
    }.bind(this)
    this.slideBwd = function() {
      if (this.slideCounter > 0) {
        this.slideCounter--;
      } else {
        this.slideCounter = this.pagerButtons.length - 1;
      }
      for (var i = 0; i < this.pagerButtons.length; i++) {
        if (i === this.slideCounter) {
          this.pagerButtons.get(i).setAttribute("data-slide-active", "1");
        } else {
          this.pagerButtons.get(i).setAttribute("data-slide-active", "0");
        }
      }
      this.sliderItems.fadeOut(1000);
      $(this.sliderItems.get(this.slideCounter)).fadeIn(600);
      this.pagerButtons.removeClass("active-bullet-point");
      $(this.pagerButtons.get(this.slideCounter)).addClass("active-bullet-point");
      clearInterval(this.slideTimer);
      this.slideTimer = setInterval(function() {
        this.slideFwd();
      }.bind(this), 10000);
    }.bind(this)
    this.pagerHandler = function(e) {
      e.currentTarget.setAttribute("data-slide-active", "1");
      this.slideCounter = parseInt(e.currentTarget.getAttribute("data-slide-index"));
      for (var i = 0; i < this.pagerButtons.length; i++) {
        if (i === this.slideCounter) {
          this.pagerButtons.get(i).setAttribute("data-slide-active", "1");
        } else {
          this.pagerButtons.get(i).setAttribute("data-slide-active", "0");
        }
      }
      this.sliderItems.fadeOut(1000);
      $(this.sliderItems.get(this.slideCounter)).fadeIn(600);
      this.pagerButtons.removeClass("active-bullet-point");
      $(e.currentTarget).addClass("active-bullet-point");
      clearInterval(this.slideTimer);
      this.slideTimer = setInterval(function() {
        this.slideFwd();
      }.bind(this), 10000);
    }.bind(this)
    this.slideTimer = setInterval(function() {
      this.slideFwd();
    }.bind(this), 10000);
    this.rightArrow.click(function() {
      if (this.sliderReady) {
        clearTimeout(this.standbyTimeout);
        this.sliderReady = false;
        this.standbyTimeout = setTimeout(function() {
          this.makeReady();
        }.bind(this), 700);
        this.slideFwd();
      }
    }.bind(this));
    this.leftArrow.click(function() {
      if (this.sliderReady) {
        clearTimeout(this.standbyTimeout);
        this.sliderReady = false;
        this.standbyTimeout = setTimeout(function() {
          this.makeReady();
        }.bind(this), 700);
        this.slideBwd();
      }
    }.bind(this));
    this.pagerButtons.click(function(e) {
      if (this.sliderReady && e.currentTarget.getAttribute("data-slide-active") === "0") {
        clearTimeout(this.standbyTimeout);
        this.sliderReady = false;
        this.standbyTimeout = setTimeout(function() {
          this.makeReady();
        }.bind(this), 700);
        this.pagerHandler(e);
      }
    }.bind(this));
  }
  function toggleMenu(e) {
    var classList = $(e.currentTarget).attr("class").split(/\s+/);
    if (classList.indexOf("active") < 0) {
      $(e.currentTarget).addClass("active");
      $("nav .menu").css("display", "");
    } else {
      $(e.currentTarget).removeClass("active");
      $("nav .menu").css("display", "none");
    }
  }
  $(".hamburger-button").click(function(e) {
    toggleMenu(e);
  });
  function Search(searchButton, searchParent, searchForm, search, searchSubmit, menu, finalWidth) {
    this.searchButton = searchButton;
    this.searchParent = searchParent;
    this.searchForm = searchForm;
    this.search = search;
    this.searchSubmit = searchSubmit;
    this.menu = menu;
    this.finalWidth = finalWidth;
    this.isOpen = false;
    this.clickedInside = false;
    this.openSearch = function() {
      this.search.addClass("search-active");
      this.search.trigger("focus");
      this.menu.css("visibility", "hidden");
      this.searchSubmit.css("display", "block");
      this.searchParent.css("width", "100%");
      this.searchForm.animate({width: this.finalWidth}, 500, function() {
        this.search.attr("placeholder", "Enter your search query...")
        this.isOpen = true;
      }.bind(this));
    }.bind(this)
    this.closeSearch = function() {
      this.search.attr("placeholder", "")
      this.searchForm.animate({width: "0"}, 500, function() {
        this.search.removeClass("search-active");
        this.menu.css("visibility", "visible");
        this.searchSubmit.css("display", "none");
        this.searchParent.css("width", "");
        this.search.val("");
        this.search.trigger("blur");
      }.bind(this));
    }.bind(this)
    this.searchSubmit.click(function(e) {
      if (this.search.val() === "") {
        e.preventDefault();
      }
    }.bind(this));
    this.searchSubmit.mousedown(function() {
      this.clickedInside = true;
    }.bind(this))
    this.searchButton.click(function() {
      this.openSearch();
    }.bind(this));
    this.search.mousedown(function() {
      this.clickedInside = true;
    }.bind(this));
    $(document).mouseup(function() {
      if (this.isOpen && this.clickedInside === false) {
        this.closeSearch();
        this.isOpen = false;
      }
      this.clickedInside = false;
    }.bind(this));
  }
  var hpSearch;
  if ($(window).width() >= 479) {
    hpSearch = new Search($(".search-icon"), $(".search-parent"), $(".search-form"), $(".search"), $(".search-submit"), $(".header-menu"), "68%");
  } else {
    hpSearch = new Search($(".search-icon"), $(".search-parent"), $(".search-form"), $(".search"), $(".search-submit"), $(".header-menu"), "100%");
  }
  var hpSlider = new Slider($(".slider-button-left"), $(".slider-button-right"), $(".pager-button"), $(".hp-slider-item"));
  $(".hp-slider").css("height", ($(window).height() * 65 / 100) + "px");
  $(".static-container").css("min-height", ($(window).height() - $("header").outerHeight() - $("footer").outerHeight()) + "px");
  $(window).resize(function() {
    if ($(window).width() >= 479) {
      hpSearch.finalWidth = "68%";
    } else {
      hpSearch.finalWidth = "100%";
    }
    $(".hp-slider").css("height", ($(window).height() * 65 / 100) + "px");
    $(".static-container").css("min-height", ($(window).height() - $("header").outerHeight() - $("footer").outerHeight()) + "px");
  });
});
