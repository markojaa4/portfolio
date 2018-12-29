var collapseButtons = document.querySelectorAll('.collapse-button');
var leaguesArticles = document.querySelectorAll('.leagues .article');
var tables = document.querySelectorAll('.leagues table');
function collapseAllTables() {
  for (var i = 0; i < tables.length; ++i) {
    tables[i].style.display = 'none';
    collapseButtons[i].innerHTML = 'Prikaži trenutni plasman';
  }
}
function collapseTable() {
  var currentIndex = event.currentTarget.dataset.indexNumber;
  for (var i = 0; i < leaguesArticles.length; ++i) {
    if (i == currentIndex) {
      var tableStyle = leaguesArticles[i].querySelector('table').style;
      if (tableStyle.display === 'none') {
        collapseAllTables();
        tableStyle.display = 'table';
        event.currentTarget.innerHTML = 'Sakrij trenutni plasman';
      }
      else {
        tableStyle.display = 'none';
        event.currentTarget.innerHTML = 'Prikaži trenutni plasman';
      }
    }
  }
}
for (var i = 0; i < collapseButtons.length; ++i) {
  collapseButtons[i].addEventListener('click', collapseTable);
}
collapseAllTables();
