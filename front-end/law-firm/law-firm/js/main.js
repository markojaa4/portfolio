jQuery(document).ready(function($) {
  function MobileMenu(menuButton, menu, overlay, topLink, menuWidth) {
    this.menuButton = menuButton;
    this.menu = menu;
    this.overlay = overlay;
    this.topLink = topLink;
    this.buttonReady = true;
    this.menuWidth = menuWidth;
    this.initialize = function() {
      this.startPosition = "-" + this.menuWidth;
      this.hideMenu = function() {
        this.buttonReady = false;
        this.overlay.fadeTo(300, 0);
        this.menu.animate({left: this.startPosition}, 300, function() {
          this.menu.removeClass("active");
          this.overlay.removeClass("active");
          $("li.active > ul", this.menu).slideUp(0);
          this.topLink.removeClass("active");
          this.buttonReady = true;
        }.bind(this));
      }.bind(this)
    }.bind(this)
    this.initialize();
    this.menuButton.click(function() {
      if (this.buttonReady) {
        this.overlay.addClass("active");
        this.menu.addClass("active");
        this.overlay.fadeTo(300, 0.5);
        this.menu.animate({left: "0"}, 300);
      }
    }.bind(this));
    this.overlay.click(function() {
      if (this.buttonReady) {
        this.hideMenu();
      }
    }.bind(this));
    this.topLink.click(function(e) {
      var active;
      var current;
      var hasChildren;
      if ($(e.currentTarget).attr("class")) {
        if ($(e.currentTarget).attr("class").split(/\s+/).indexOf("active") >= 0) {
          active = true;
        } else {
          active = false;
        }
      }
      if ($(e.currentTarget).attr("class")) {
        if ($(e.currentTarget).attr("class").split(/\s+/).indexOf("active_nav_link") >= 0) {
          current = true;
        } else {
          current = false;
        }
      }
      if ($("ul", e.currentTarget).length > 0) {
        hasChildren = true;
      } else {
        hasChildren = false;
      }
      if (!active && !current && hasChildren) {
        e.preventDefault();
        $("li.active > ul", this.menu).slideUp(300);
        $("li.active", this.menu).removeClass("active");
        $(e.currentTarget).addClass("active");
        $("ul", e.currentTarget).slideDown(300);
      }
    }.bind(this));
  }
  var extraMargin = 0;
  var mobileMenu = new MobileMenu($("#top_menu_hamburger_button"), $(".mobile_menu"), $(".mobile_menu_overlay"), $(".mobile_menu nav > div > ul > li"), "40%");
  if ($.browser.msie && $.browser.version == 9) {
    $("html").addClass("ie9");
  } else if ($.browser.mozilla) {
    $("html").addClass("mozilla");
  }
  if ($(".pages_container").length > 0) {
    extraMargin = 3;
  }
  $(".main_content").css("min-height", ($(window).height() - $("header").outerHeight() - $("footer").outerHeight() - extraMargin) + "px");
  if ($(window).width() <= 545) {
    mobileMenu.menuWidth = "60%";
    mobileMenu.initialize();
  }
  $(window).resize(function() {
    if ($(window).width() <= 545) {
      mobileMenu.menuWidth = "60%";
      mobileMenu.initialize();
    }
    $(".main_content").css("min-height", ($(window).height() - $("header").outerHeight() - $("footer").outerHeight() - extraMargin) + "px");
  });
});
