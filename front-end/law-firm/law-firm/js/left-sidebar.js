jQuery(document).ready(function($) {
  function MobileSidebar(menuButton, menu) {
    this.menuButton = menuButton;
    this.menu = menu;
    this.active = false;
    this.buttonReady = true;
    this.pageType = $(".active_nav_link:first a:first").html();
    this.initialize = function() {
      if ($(window).width() > 755) {
        this.menu.slideDown(0);
        this.buttonReady = true;
      } else {
        if (this.active === false) {
          this.menu.slideUp(0);
          this.menuButton.html("Navigate " + this.pageType);
          this.buttonReady = true;
        }
      }
    }.bind(this)
    this.menuButton.html("Navigate " + this.pageType);
    this.initialize();
    this.menuButton.click(function() {
      if (this.buttonReady === true) {
        if (this.active === false) {
          this.buttonReady = false;
          this.menu.slideDown(500, function() {
            this.menuButton.html("Close Navigation");
            this.buttonReady = true;
          }.bind(this));
          this.active = true;
        } else {
          this.buttonReady = false;
          this.menu.slideUp(500, function() {
            this.menuButton.html("Navigate " + this.pageType);
            this.buttonReady = true;
          }.bind(this));
          this.active = false;
        }
      }
    }.bind(this));
  }
  var mobileSidebar = new MobileSidebar($(".sidebar_toggle_button"), $(".left_sidebar_inner"));
  $(window).resize(function() {
    mobileSidebar.initialize();
  });
});
