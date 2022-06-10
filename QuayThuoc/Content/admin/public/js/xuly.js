
    var socket = io.connect('http://localhost:9000');
    if (socket.connected) {
        console.log('socket connected successfull');
    } else {
        console.log('socket connected failed');
    }

    socket.on("server-sent-user", function (data) {
        $('#user').html("");
        $('#user').append('Số người truy cập: ' + data);
    });  
    
    

    function confirmDelete(id) {
        var ask = confirm('Bạn có chắc chắn muốn xóa không?');
        if (ask) {
            window.location.href = "index.php?m=booking&a=delete&id=" + id;
        } else {
            return false;
        }
    }

    jQuery(document).ready(function () {
        jQuery('#chk_all' ).click( function () {
            jQuery('input[type="checkbox"]' ).prop('checked', this.checked)
        });

    });

    $(document).ready(function() {
        $('#content1').show();
        $('#dangki').hide();    

        $('#hide-btn').click(function() {
            $('#content1').hide(1500);
            $('#dangki').show(1000);
        });
        // $('#registration-room').click(function() {
        //     $('#content1').show(1500);
        //     $('#dangki').hide(1000);
        // });
        $('#out').click(function() {
            $('#content1').show(1500);
            $('#dangki').hide(1000);
        });
    });


    $('#registration-room').click(function () {

        if ( ($('#user_id').val() != (-1) ) && ($('#content').val() != '') ){
            var data = {
                user_id: $('#user_id').val(),
                user_name: 'Đang Cập Nhật',
                time_from: $('#time_start').val(),
                time_to: $('#time_end').val(),
                content: $('#content').val(),
                notes: '',
                status: 1
            };
            if (socket.connected) {
                socket.emit("send-booking-room", data);

                // var tr = '<tr>'+
                //     '<td></td>'+
                //     '<td></td>'+
                //     '<td>' + data.user_name + '</td>'+
                //     '<td>' + data.content + '</td>'+
                //     '<td class="text-center">' + data.status + '</td>'+
                //     '<td></td>'+
                //     '<td></td>'+'</tr>';
                // $('#booking-table').prepend(tr);
            } else {
                console.log('socket connected failed');
            }
        } else {
            socket.emit("send-room-error");
        }

    });

    socket.on('response-booking-room', function(){
        alert("đăng kí thành công");
        $('#content1').show(1500);
        $('#dangki').hide(1000);

    });
    
    socket.on('response-room-error', function(){
        alert("đăng kí không thành công thành công! Vui lòng điền đầy đủ thông tin!!!");
        $('#content1').hide();
        $('#dangki').show();
    });

    // $('#btn-add-booking').click(function () {
    //     var data = {
    //         user_id: 4,
    //         user_name: 'Phạm Văn Đoan',
    //         time_from: '2020-02-27 10:00:00',
    //         time_to: '2020-02-27 11:00:00',
    //         content: 'Họp triển khai dự án mới',
    //         notes: '',
    //         status: 0
    //     };

    //     if (socket.connected) {
    //         socket.emit("send-booking", data);

    //         var tr = '<tr>'+
    //             '<td></td>'+
    //             '<td></td>'+
    //             '<td>' + data.user_name + '</td>'+
    //             '<td>' + data.content + '</td>'+
    //             '<td class="text-center">' + data.status + '</td>'+
    //             '<td></td>'+
    //             '<td></td>'+'</tr>';
    //         $('#booking-table').prepend(tr);
    //     } else {
    //         console.log('socket connected failed');
    //     }
    // });

    socket.on('response-booking', function(data) {
        console.log(data);
        var tr = '<tr>'+
            '<td></td>'+
            '<td></td>'+
            '<td>' + data.user_name + '</td>'+
            '<td>' + data.content + '</td>'+
            '<td>' + data.time_from + '</td>'+
            '<td>' + data.time_to + '</td>'+
            '<td class="text-center">' + data.status + '</td>'+
            '<td></td>'+
            '<td></td>'+'</tr>';
        $('#booking-table').prepend(tr);
    });
