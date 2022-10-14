const role = {
    init: function () {
        role.registerEvents();
        role.deleteCategory();
    },
    registerEvents: function () {     

    },
    deleteCategory: function () {
        $('.role-link-delete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Xóa nhóm quyền',
                    text: "Bạn có chắc chắn muốn xóa nhóm quyền này?",
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
                            url: 'http://localhost:5002/Role/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    window.location.href = "/Role/Index";
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa nhóm quyền thành công!',
                                        'success'
                                    );
                                }
                                else {
                                    window.location.href = "/Category/Index";
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa nhóm quyền thất bại",
                                        'error'
                                    );
                                }

                            }
                        })

                    }
                })
            })
        })
    }
}
role.init();