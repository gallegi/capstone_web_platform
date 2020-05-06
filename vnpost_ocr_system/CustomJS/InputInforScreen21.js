(function ($) {

    $.fn.imagePicker = function (options) {

        // Define plugin options
        var settings = $.extend({
            // Input name attribute
            name: "",
            // Classes for styling the input
            class: "",
            // Icon which displays in center of input
            icon: ""
            
        }, options);

        // Create an input inside each matched element
        return this.each(function () {
            $(this).html(create_btn(this, settings));
        });

    };

    // Private function for creating the input element
    function create_btn(that, settings) {
        // The input icon element
        var picker_btn_icon = $('<i class=""></i>');

        // The actual file input which stays hidden
        var picker_btn_input = $('<input  type="file" name="' + settings.name + '" />');

        // The actual element displayed
        var picker_btn = $('<div class="' + settings.class + '"></div>')
            .append(picker_btn_icon)
            .append(picker_btn_input);

        // File load listener
        picker_btn_input.change(function () {
            if ($(this).prop('files')[0]) {
                // Use FileReader to get file
                var reader = new FileReader();

                // Create a preview once image has loaded
                reader.onload = function (e) {
                    var preview = create_preview(that, e.target.result, settings);
                    $(that).html(preview);
                }

                // Load image
                reader.readAsDataURL(picker_btn_input.prop('files')[0]);
            }
        });

        return picker_btn
    };

    // Private function for creating a preview element
    function create_preview(that, src, settings) {

        // The preview image
        var picker_preview_image = $('<img src="' + src + '" class="test" />');

        // The remove image button
        var picker_preview_remove = $('<button class="btn bt-color-common  btn-link"><small>Remove</small></button>');

        // The preview element
        var picker_preview = $('<div class="text-center"></div>')
            .append(picker_preview_image)
            .append(picker_preview_remove);

        // Remove image listener
        picker_preview_remove.click(function () {
            var btn = create_btn(that, settings);
            $(that).html(btn);
        });

        return picker_preview;
    };

}(jQuery));

$(document).ready(function () {
    $('.img-picker').imagePicker({ name: 'images' });
})

//function checkFilter() {
//    var item1 = document.getElementById("item1").value;
//    var item2 = document.getElementById("item2").value;
//    var item3 = document.getElementById("item3").value;
//    var item4 = document.getElementById("item4").value;
    
   
    
//    if (item1 !== "Tỉnh/Thành phố" && item2 !== "Quận/Huyện" && item3 !== "Tên CQHC" && item4 !== "Tên thủ tục") {
//        //alert(item1 + "-" + item2 + "-" + item3 + "-" + item4);
//        document.getElementById("contact").style.display = "block";
//    }
//}