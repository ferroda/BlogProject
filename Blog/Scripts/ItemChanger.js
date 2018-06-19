$(document).ready(function () {
    // Changing the size of the text
    $("#FontSize").on("input", function () {
        $("#Text").css("font-size", $(this).val() + "px");
    });

    // Changing the color of the font.
    $("#FontColorId").on("change", function () {
        $("#Text").css("color", $("#FontColorId option[value='" + $(this).val() + "']").text());
    });

    // Changing the color of the background, by jQuery action.
    $("#BackgroundColorId").on("change", function () {
        $("#Text").css("background-color", $("#FontColorId option[value='" + $(this).val() + "']").text());
    });

});







