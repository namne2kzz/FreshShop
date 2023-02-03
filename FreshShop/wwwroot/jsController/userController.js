let user = {
    init: function () {
        user.registerEvent();
        user.deleteAddress();
    },
    registerEvent: function () {
        $("#sllProvince").off('change').on('change', function () {
            var id = $(this).val();

            if (id != '') {
                user.loadDistrict(id);
            }
            else {
                $('#sllDistrict').html('');
            }

        });

        $('.address-link-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            if (btn.attr('id')) {
                Swal.fire(
                    'Cảnh báo',
                    "Không thể thao tác với địa chỉ mặc định",
                    'error'
                );
            }

            var id = $(this).data('id');

            $.ajax({
                url: "http://localhost:5000/vi/User/ChangeDefaultAddress",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        $('#address-link-default').text('Phụ');
                        $('#address-link-default').removeAttr('id');

                        btn.text('Mặc định');
                        btn.attr('id', 'address-link-default');
                    }
                }
            });
        });
    },
    deleteAddress: function () {
        $('.address-link-delete').each(function () {
            $(this).off('click').on('click', function (e) {
                let tr = $(this).closest('tr');
                e.preventDefault();
                Swal.fire({
                    title: 'Xóa địa chỉ',
                    text: "Bạn có chắc chắn muốn xóa địa chỉ này?",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'Cancle',
                    // customClass: 'swal-wide'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            url: 'http://localhost:5000/vi/User/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {                                                                     
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa địa chỉ thành công!',
                                        'success'
                                    );
                                    tr.remove();
                                }
                                else {
                                    //window.location.reload();
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa địa chỉ thất bại",
                                        'error'
                                    );
                                }

                            }
                        })

                    }
                })
            })
        })
    },
    loadDistrict: function (id) {
        $.ajax({
            url: 'http://localhost:5000/vi/User/LoadDistrict',
            data: { id: id },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == true) {
                    var html = '<option value="" selected="true" disabled="disabled">---Chọn Quận/Huyện---</option>';
                    var data = res.data;
                    console.log(data);
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.code + '">' + item.name + '</option>'
                    })
                    $('#sllDistrict').html(html);
                }

            }
        })
    },
}
user.init();