function screenAdjust() {
  var topPadding = 20;
  var header = document.querySelector('.header');
  var pageContent = document.querySelector('.page-content');
  var pageMiddle = document.querySelector('.page-middle');
  var footer = document.querySelector('.footer');
  if (document.body.clientHeight <= window.innerHeight) {
    pageMiddle.style.height = (window.innerHeight - header.clientHeight - footer.clientHeight - topPadding) + 'px';
  }
}
window.onload = function() {
  screenAdjust();
}
window.onresize = function() {
  screenAdjust();
}
