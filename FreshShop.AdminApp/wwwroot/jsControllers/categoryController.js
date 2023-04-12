const category = {
    init: function () {
        category.registerEvents();
        category.deleteCategory();
    },
    registerEvents: function () {
        $('.category-link-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = $(this).data('id');

            $.ajax({
                url: "http://localhost:5002/Category/UpdateShowOnHome",
                data: { categoryId: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('Hiển thị');
                    }
                    else {
                        btn.text('Khóa');
                    }
                }
            });
        });

    },
    deleteCategory: function () {
        $('.category-link-delete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Xóa danh mục',
                    text: "Bạn có chắc chắn muốn xóa danh mục này?",
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
                            url: 'http://localhost:5002/Category/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    window.location.href = "/Category/Index";
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa danh mục thành công!',
                                        'success'
                                    );
                                }
                                else {
                                    window.location.href = "/Category/Index";
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa danh mục thất bại",
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
category.init();