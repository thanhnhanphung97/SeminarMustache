var support = {
    init: function () {
        support.LoadData();
        support.registerEvent();
    },
    registerEvent: function () {
        $('.btn-edit').off('click').on('click', function () {
            var id = $(this).data('id');
            support.LoadDetail(parseInt(id));
            $('#myModal').modal('show');
        });
        $('#btnCreate').on('click', function () {
            $('#myModal').modal('show');
        });
        $('#btnSave').on('click', function () {
            if ($('#masv').val() != "" && $('#hoten').val() != "" && $('#tenlop').val() != "") {
                support.SaveData();
                setTimeout(function () { support.LoadData(); }, 200);
            }
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            if(confirm("Are you sure?")) support.DeleteData(parseInt(id));
        });
    },
    LoadData: function() {
        $.ajax({
            url: '/Home/LoadData',
            data: {},
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                var date = response.date;
                var html = "";
                var infoData = $('#info-data').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(infoData, {
                        Id: item.Id,
                        HoTen: item.HoTen,
                        NgaySinh: date[i],
                        TenLop: item.TenLop,
                        GioiTinh: item.GioiTinh,
                        HocBong: item.HocBong
                    });
                })
                $('#tblInfo').html(html);
                support.registerEvent();
            }
        })    
    },
    LoadDetail: function(id) {
        $.ajax({
            url: '/Home/LoadDetail',
            data: {
                Id: id,
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                $('#masv').val(data.Id);
                $('#hoten').val(data.HoTen);
                var val = data.TenLop == "Công Nghệ Thông Tin" ? 0 : 1;
                $('#tenlop').val(val);
                $('#ngaysinh').val(response.date);
                $('#hocbong').val(data.HocBong);
                var checked = data.GioiTinh == "Nam" ? true : false;
                $('#gioitinh').prop('checked', checked);
            }
        })
    },
    SaveData: function () {
        var id = $('#masv').val();
        var hoten = $('#hoten').val();
        var tenlop = $('#tenlop').val() == 0 ? "Công Nghệ Thông Tin" : "Kinh Tế Học";
        var date = $('#ngaysinh').val();
        var hocbong = $('#hocbong').val();
        var checked = $('#gioitinh').prop('checked') == true? "Nam" : "Nữ";
        var sv = {
            Id: id,
            HoTen: hoten,
            TenLop: tenlop,
            NgaySinh: date,
            HocBong: hocbong,
            GioiTinh: checked
        }
        $.ajax({
            url: '/Home/CreateEdit',
            data: {
                strSV: JSON.stringify(sv)
            },
            type: 'POST',
            dateType: 'json',
            success: function (response) {
                if (response.mes == "success") alert("Success!");
                else alert("Fail!");
            }
        })
    },
    DeleteData: function (id) {
        $.ajax({
            url: '/Home/DeleteData',
            data: {
                Id: id,
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.mes == "success") {
                    alert("Success!");
                    support.LoadData();
                }
                else alert("Fail!");
            }
        })
    }
}

support.init();