$(document).ready(function() {
  /*
  String.prototype.indexOfAll = function(query) {
    var target = this;
    var indices = [];
    for (var i = 0; i < target.length; i++) {
      if (target[i] === query)
      {
        indices.push(i);
      }
    }
    return indices;
  }
  String.prototype.replaceAt = function(index, replacement) {
    var before = this.substring(0, index);
    var after = this.substring(index + 1);
    return before + replacement + after;
  }
  function fixChars(element, origChar, fix)
  {
    elementContent = $(element).html();
    var indices = elementContent.indexOfAll(origChar);
    for (var i = 0; i < indices.length; i++) {
      elementContent = elementContent.replaceAt(indices[i], fix);
      $(element).html(elementContent);
      for (var j = 0; j < indices.length; j++) {
        indices[j] += (fix.length - 1);
      }
    }
  }
  */
  $.ajax({
    type: "GET",
    dataType: "json",
    url: "json/proizvod.json",
    mimeType: "application/json",
    data: {},
    error: function() {
      console.log("json error");
    },
    success: function(data) {
      data = data[0];
      $("h2").text(data["proizvod"]);
      $(".price .value").text(data["cena"]);
      $(".description").text(data["opis"]);
      $("table caption").text("Tehnički podaci");
      $("thead").html("");
      $("tbody").html("");
      $.each(data["tehnički-podaci"][0], function(key, value) {
        var keyNorm = key.toLowerCase();
        keyNorm = keyNorm.trim();
        switch (keyNorm) {
          case "šifra":
            key = "šifra artikla";
            break;
          case "unos":
            key = "način unosa podataka";
            break;
          case "jezgara":
            key = "broj jezgara";
            break;
          case "grafička":
            key = "grafička kartica";
            break;
          case "ekran":
            key = "rezolucija ekrana";
            break;
        }
        if ($("thead").html() === "") {
          $("thead").append($('<tr>' +
                                '<th scope="row">' + key + '</th>' +
                                '<th scope="col">' + value + '</th>' +
                              '</tr>'));
        }
        else {
          $("tbody").append($('<tr>' +
                                '<th scope="row">' + key + '</th>' +
                                '<td>' + value + '</td>' +
                              '</tr>'));
        }
      });
      //fixChars("tbody", "®", "<sup>&reg;</sup>");
      //fixChars("tbody", "™", "<sup>&trade;</sup>");
    }
  });
});
