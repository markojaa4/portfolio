jQuery(document).ready(function($) {
  function Slider(sliderItems, pagerParent, sliderBox) {
    this.pagerParent = pagerParent;
    this.sliderItems = sliderItems;
    this.sliderBox = sliderBox;
    this.slideCounter = 0;
    this.sliderReady = true;
    for (var i = 0; i < sliderItems.length; i++) {
      var $div = this.pagerParent.append($('<div class="pager_button" data-slide-index="' + i + '" data-slide-active="0"><div class="pager_button_inner">' + (i + 1) + '</div></div>'));
    }
    this.pagerButtons = $(".pager_button", this.pagerParent);
    this.pagerButtons.get(0).setAttribute("data-slide-active", "1");
    $(this.pagerButtons.get(0)).addClass("active_bullet_point");
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
      this.pagerButtons.removeClass("active_bullet_point");
      $(this.pagerButtons.get(this.slideCounter)).addClass("active_bullet_point");
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
      this.pagerButtons.removeClass("active_bullet_point");
      $(this.pagerButtons.get(this.slideCounter)).addClass("active_bullet_point");
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
      this.pagerButtons.removeClass("active_bullet_point");
      $(e.currentTarget).addClass("active_bullet_point");
      clearInterval(this.slideTimer);
      this.slideTimer = setInterval(function() {
        this.slideFwd();
      }.bind(this), 10000);
    }.bind(this)
    this.slideTimer = setInterval(function() {
      this.slideFwd();
    }.bind(this), 10000);
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
    if (this.sliderBox.length > 0) {
      Hammer(this.sliderBox.get(0)).on("swipeleft", function() {
        if (this.sliderReady) {
          clearTimeout(this.standbyTimeout);
          this.sliderReady = false;
          this.standbyTimeout = setTimeout(function() {
            this.makeReady();
          }.bind(this), 700);
          this.slideFwd();
        }
      }.bind(this));
      Hammer(this.sliderBox.get(0)).on("swiperight", function() {
        if (this.sliderReady) {
          clearTimeout(this.standbyTimeout);
          this.sliderReady = false;
          this.standbyTimeout = setTimeout(function() {
            this.makeReady();
          }.bind(this), 700);
          this.slideBwd();
        }
      }.bind(this));
    }
  }
  function ArticleSlider(sliderItemSelector, buttonLeft, buttonRight, slotNumber, sliderBox, sliderItemsParent) {
    this.sliderItemSelector = sliderItemSelector;
    this.sliderItems = $(sliderItemSelector);
    this.buttonLeft = buttonLeft;
    this.buttonRight = buttonRight;
    this.sliderBox = sliderBox;
    this.sliderItemsParent = sliderItemsParent;
    this.slotNumber = slotNumber;
    this.sliderReady = true;
    this.initialize = function () {
      if (slotNumber > this.sliderItems.length) {
        this.slotNumber = this.sliderItems.length;
      }
      this.sliderItemsParent.css("width", $(this.sliderItems.get(0)).width() * (this.sliderItems.length + this.slotNumber) + "px");
      this.boxWidth = $(this.sliderItems.get(0)).width() * this.slotNumber;
      var targetPosition = -this.boxWidth + "px";
      $(this.sliderBox).css("width", (this.boxWidth - 11) + "px");
      this.slideBwd = function() {
        if (this.sliderReady) {
          this.sliderReady = false;
          this.sliderItemsParent.css("left", targetPosition);
          for (var i = -1; i >= -this.slotNumber; i--) {
            this.sliderItemsParent.prepend($(this.sliderItems.get(this.sliderItems.length + i)).clone());
          }
          this.sliderItems = $(this.sliderItemSelector);
          if (!($.browser.msie && $.browser.version == 9)) {
            this.sliderItemsParent.css("left");
            this.sliderItemsParent.css("-webkit-transition", "left 0.9s");
            this.sliderItemsParent.css("transition", "left 0.9s");
            this.sliderItemsParent.css("left", "0");
            setTimeout(function() {
              this.sliderItemsParent.css("-webkit-transition", "initial");
              this.sliderItemsParent.css("transition", "initial");
              for (var i = -1; i >= -this.slotNumber; i--) {
                $(this.sliderItems.get(this.sliderItems.length + i)).remove();
              }
              this.sliderItems = $(this.sliderItemSelector);
              this.sliderReady = true;
            }.bind(this), 900);
          } else {
            this.sliderItemsParent.animate({left: "0"}, 800, function() {
              for (var i = -1; i >= -this.slotNumber; i--) {
                $(this.sliderItems.get(this.sliderItems.length + i)).remove();
              }
              this.sliderItems = $(this.sliderItemSelector);
              this.sliderReady = true;
            }.bind(this));
          }
        }
      }.bind(this)
      this.slideFwd = function() {
        if (this.sliderReady) {
          this.sliderReady = false;
          for (var i = 0; i < this.slotNumber; i++) {
            this.sliderItemsParent.append($(this.sliderItems.get(i)).clone());
          }
          this.sliderItems = $(this.sliderItemSelector);
          if (!($.browser.msie && $.browser.version == 9)) {
            this.sliderItemsParent.css("-webkit-transition", "left 0.9s");
            this.sliderItemsParent.css("transition", "left 0.9s");
            this.sliderItemsParent.css("left", targetPosition);
            setTimeout(function() {
              this.sliderItemsParent.css("-webkit-transition", "initial");
              this.sliderItemsParent.css("transition", "initial");
              this.sliderItemsParent.css("left", "0");
              for (var i = 0; i < this.slotNumber; i++) {
                $(this.sliderItems.get(i)).remove();
              }
              this.sliderItems = $(this.sliderItemSelector);
              this.sliderReady = true;
            }.bind(this), 900);
          } else {
            this.sliderItemsParent.animate({left: targetPosition}, 800, function() {
              this.sliderItemsParent.css("left", "0");
              for (var i = 0; i < this.slotNumber; i++) {
                $(this.sliderItems.get(i)).remove();
              }
              this.sliderItems = $(this.sliderItemSelector);
              this.sliderReady = true;
            }.bind(this));
          }
        }
      }.bind(this)
    }.bind(this)
    this.initialize();
    this.buttonLeft.click(function() {
      this.slideBwd();
    }.bind(this));
    this.buttonRight.click(function() {
      this.slideFwd();
    }.bind(this));
    if (this.sliderBox.length > 0 && /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
      Hammer(this.sliderBox.get(0)).on("swipeleft", function() {
        this.slideFwd();
      }.bind(this));
      Hammer(this.sliderBox.get(0)).on("swiperight", function() {
        this.slideBwd();
      }.bind(this));
    }
  }
  var hpSlider = new Slider($(".top_slider_item"), $(".top_slider .pager"), $(".top_slider_inner"));
  var serviceSlider = new ArticleSlider(".service_slider_item", $(".service_slider .arrow_left"), $(".service_slider .arrow_right"), 3, $(".service_slider_items_wrapper"), $(".service_slider_items"));
  if ($(window).width() <= 479) {
    serviceSlider.slotNumber = 1;
  } else if ($(window).width() < 975) {
    serviceSlider.slotNumber = 2;
  }
  serviceSlider.initialize();
  $(window).resize(function() {
    if ($(window).width() <= 479) {
      serviceSlider.slotNumber = 1;
    } else if ($(window).width() < 975) {
      serviceSlider.slotNumber = 2;
    } else if ($(window).width() >= 975) {
      serviceSlider.slotNumber = 3;
    }
    serviceSlider.initialize();
  });
});
