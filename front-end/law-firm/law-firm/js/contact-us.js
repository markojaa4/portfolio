function initMap() {
  if (document.querySelector(".embed_map")) {
    var latLngObj = {lat: 35.913453, lng: 14.493235};
    var map = new google.maps.Map(document.querySelector(".embed_map"), {
      center: latLngObj,
      zoom: 17
    });
    var marker = new google.maps.Marker({
      position: latLngObj,
      map: map,
      title: 'M&R.K.; Advocates'
    });
  }
}
jQuery(document).ready(function($) {
  function handleContact() {
    var fullName = $("#contacter_name").val();
    var email = $("#contacter_mail").val();
    var subject = $("#contacter_subject").val();
    var message = $("#contacter_message").val();
    $("form[name='contact'] input[type='text']").removeClass("invalid_field");
    $("form[name='contact'] textarea").removeClass("invalid_field");
    if (fullName == "" || subject == "" || message == "" || !emailRegExp.test(email) || email == "") {
      if (fullName == "") {
        $("#contacter_name").addClass("invalid_field");
      }
      if (subject == "") {
        $("#contacter_subject").addClass("invalid_field");
      }
      if (message == "") {
        $("#contacter_message").addClass("invalid_field");
      }
      if (!emailRegExp.test(email) || email == "") {
        $("#contacter_mail").addClass("invalid_field");
      }
    } else {
      $.ajax({
        type: "POST",
        url: "/wp-admin/admin-ajax.php",
        data: {
          action: "mrk_process_form",
          fullName: fullName,
          email: email,
          subject: subject,
          message: message
        },
        cache: false,
        success: function(result) {
          $("input[type='text']").val("");
          $("#contacter_message").val("");
          $("form[name='contact'] .form_result p").html(result);
          var formTimeout = setTimeout(function() {
            $("form[name='contact'] .form_result").fadeIn(500);
          }, 500);
          $("form[name='contact'] .form_input").fadeOut(500);
        },
        error: function(err) {
          alert(err);
          console.error(err);
        }
      });
    }
  }
  var emailRegExp = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  $("#contact_submit").click(function(e) {
    handleContact();
  });
  $("form[name='contact'] .regular_field").keypress(function(e) {
    $(e.currentTarget).removeClass("invalid_field");
  });
  $("form[name='contact'] .email_field").keypress(function(e) {
    if (emailRegExp.test($(e.currentTarget).val())) {
      $(e.currentTarget).removeClass("invalid_field");
    }
  });
});
