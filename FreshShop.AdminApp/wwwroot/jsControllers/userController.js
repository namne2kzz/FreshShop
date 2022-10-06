const user = {
    init: function () {
        user.registerEvents();
        user.deleteUser();
    },
    registerEvents: function () {
                    
    },
    deleteUser: function () {

        $('.user-link-delete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Xóa người dùng',
                    text: "Bạn có chắc chắn muốn xóa tài khoản này?",
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
                            url: 'http://localhost:5002/User/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {                                   
                                    window.location.href = "/User/Index";
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa tài khoản thành công!',
                                        'success'
                                    );
                                }
                                else {                                 
                                    window.location.href = "/User/Index";
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa tài khoản thất bại",
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
user.init();