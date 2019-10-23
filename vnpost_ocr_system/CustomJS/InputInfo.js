$("#mainTable").on("mousedown", ".bt-add", function () {
    // var nextStt = curStt + 1;
    var newRow = $("<tr>");
    var cols = "";
    cols += '<td colspan="1" class="stt"></td>';
    cols += '<td class="hover-caret my-p-l-20 my-left-align" contenteditable="true" placeholder="Nhấp để chỉnh sửa nội dung" style="text-align: left !important;"></td>';
    cols += '<td class="hover-caret" contenteditable="true" placeholder="Số lượng" style="text-align: left;"></td>';
    cols += '<td class="no-collapse-m-p"> <div style="text-align: center;"> <button class="btn waves-effect waves-light bt-color-common m-r-10 bt-add my-bt" type="button"> + </button> <button class="btn waves-effect waves-light red bt-del my-bt" type="button"> -</button> </div> </td>';
    newRow.append(cols);
    newRow.insertAfter($(this).closest('tr'));
});

$("#mainTable").on("mouseup", ".bt-add", function () {
    resetStt();
});

$("#mainTable").on("click", ".bt-del", function () {
    var sttArr = $('.stt');
    var last = sttArr.length + 1;
    if (last == 2) return;
    $(this).closest("tr").remove();
    resetStt();
});

function resetStt() {
    var table = document.getElementById('mainTable');

    var rowLength = table.rows.length;

    for (var i = 1; i < rowLength; i++) {
        var row = table.rows[i];
        //your code goes here, looping over every row.
        //cells are accessed as easy
        row.cells[0].innerHTML = i + '';
    }
}