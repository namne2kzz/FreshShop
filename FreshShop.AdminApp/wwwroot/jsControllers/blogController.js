const blog = {
    init: function () {
        blog.registerEvents();
        blog.deleteBlog();
    },
    registerEvents: function () {
        $('.blog-link-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = $(this).data('id');

            $.ajax({
                url: "http://localhost:5002/Blog/ChangeStatus",
                data: { id: id },
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
    deleteBlog: function () {
        $('.blog-link-delete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Xóa bài viết',
                    text: "Bạn có chắc chắn muốn xóa bài viết này?",
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
                            url: 'http://localhost:5002/Blog/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    window.location.href = "/Blog/Index";
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa bài viết thành công!',
                                        'success'
                                    );
                                }
                                else {
                                    window.location.href = "/Blog/Index";
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa bài viết thất bại",
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
blog.init();