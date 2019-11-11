

$("#mainTable").on("mousedown", ".bt-add", function () {
    // var nextStt = curStt + 1;
    var newRow = $("<tr>");
    var cols = "";
    cols += '<td class="stt"></td>';
    cols += '<td></td>';
    cols += '<td></td>';
    cols += '<td></td>';
    newRow.append(cols);
    newRow.insertAfter($(this).closest('tr'));
});

$("#mainTable").on("mouseup", ".bt-add", function () {
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