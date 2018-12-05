$(function () {
    oTable = $('#jhxmtable').dataTable();

    var nEditing = null;

    $('#jhxmtable').on('click', 'a.edit', function (e) {
        e.preventDefault();
        var nRow = $(this).parents('tr')[0];
        if (nEditing !== null && nEditing != nRow) {
            rollbackRow(oTable, nEditing);
            editRow(oTable, nRow);
            nEditing = nRow;
        } else if (nEditing == nRow && this.innerHTML == "保存") {
            saveRow(oTable, nEditing);
            nEditing = null;
        } else {
            editRow(oTable, nRow);
            nEditing = nRow;
        }
    });

    function editRow(oTable, nRow) {
        var aData = oTable.fnGetData(nRow);
        var rTds = $('>td', nRow);
        rTds[0].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[0] + '">';
        rTds[1].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[1] + '">';
        rTds[2].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[2] + '">';
        rTds[3].innerHTML = '<a class="edit" href="">保存</a><a class="cancel" href="">取消</a>';
        rTds[4].innerHTML = '';
    }

    function saveRow(oTable, nRow) {
        var rInputs = $('input', nRow);
        oTable.fnUpdate(rInputs[0].value, nRow, 0, false);
        oTable.fnUpdate(rInputs[1].value, nRow, 1, false);
        oTable.fnUpdate(rInputs[2].value, nRow, 2, false); 
        oTable.fnUpdate('<a class="edit" href="">编辑</a>', nRow, 3, false);
        oTable.fnUpdate('<a class="delete" href="">删除</a>', nRow, 4, false);
        oTable.fnDraw();
    }

    $('#jhxmtable').on('click', 'a.delete', function (e) {
        e.preventDefault();
        if (confirm("你确定要删除这条记录?") == false) {
            return;
        }
        var nRow = $(this).parents('tr')[0];
        oTable.fnDeleteRow(nRow);
    });
    $('#jhxmtable').on('click', 'a.cancel', function (e) {
        e.preventDefault();
        if ($(this).attr("data-mode") == "new") {
            var nRow = $(this).parents('tr')[0];
            oTable.fnDeleteRow(nRow);
        } else {
            rollbackRow(oTable, nEditing);
            nEditing = null;
        }
    });
    function rollbackRow(oTable, nRow) {
        var aData = oTable.fnGetData(nRow);
        var rTds = $('>td', nRow);
        for (var i = 0, iLen = rTds.length; i < iLen; i++) {
            oTable.fnUpdate(aData[i], nRow, i, false);
        }
        oTable.fnDraw();
    }
});


