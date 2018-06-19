$(document).ready(function () {
    //Change the font color name 
    $(".ReadPostSection").css("color", $("#Color_Name").val())  // ReadPostSection --> This is a div, where I placed the content of the blogposts.

    $(".ReadPostSection").css("background-color", $("#Color1_Name").val())

    $(".ReadPostSection").css("font-size", $("#FontSize").val() + "px")

});