$(document).ready(function () {
    loadDepartments();
    
    $('#saveDepartment').click(function () {
        saveDepartment(true); 
    });
    $(document).on('click', '.editDepartment', function (event) {
        event.preventDefault(); 

        var departmentId = $(this).data('id');
        getDepartment(departmentId); 
    });

    $('#updateDepartment').click(function () {
        saveDepartment(false); 
    });

});

$('#addDepartmentBtn').click(function () {
    $('#DepartmentModallLabel').text("Insert Department Information");
    $('#saveDepartment').show();
    $('#updateDepartment').hide();
    $('#DepartmentModal').modal('show');
    $('#DepartmentID').val(0);  
    loadInstructors();
});

function getDepartment(id) {
    $.ajax({
        url: '/Departments/GetDepartment',
        type: 'GET',
        data: { id: id },
        success: function (response) {
            if (response.success) {
                
                $('#DepartmentID').val(response.data.departmentID);
                $('#DepartmentName').val(response.data.departmentName);
                $('#Budget').val(response.data.budget);
                var startDate = new Date(response.data.startDate);
                var formattedDate = startDate.getFullYear() + '-' + ('0' + (startDate.getMonth() + 1)).slice(-2) + '-' + ('0' + startDate.getDate()).slice(-2);
                $('#StartDate').val(formattedDate);
                loadInstructors(function () {
                    $('#InstructorID').val(response.data.instructorID);
                });
                $('#DepartmentModallLabel').text("Edit Department Information");
                $('#saveDepartment').hide();
                $('#updateDepartment').show();
                $('#DepartmentModal').modal('show');
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error('Unable to fetch department.');
        }
    });
}

function saveDepartment(isCreate) {
    var formData = new FormData($('#DepartmentForm')[0]);
    var url = isCreate ? '/Departments/CreateDepartment' : '/Departments/UpdateDepartment';

    $.ajax({
        url: url,
        type: isCreate ? 'POST' : 'PUT',
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                $('#DepartmentForm')[0].reset();
                $('#DepartmentModal').modal('hide'); 
                loadDepartments();
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error('Unable to save department.');
        }
    });
}
$(document).on('click', '.deleteDepartment', function (event) {
    event.preventDefault();
    var departmentId = $(this).data('id');
    Swal.fire({
        title: 'Are you sure?',
        text: 'You will not be able to recover this Department!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Departments/DeleteDepartment',
                type: 'POST',
                data: { id: departmentId },
                success: function (response) {
                    if (response.success) {
                        Swal.fire(
                            'Deleted!',
                            response.message,
                            'success'
                        ).then(() => {
                            loadDepartments();
                            toastr.warning("Department data Deleted!");
                        });
                    } else {
                        Swal.fire(
                            'Error!',
                            response.message,
                            'error'
                        );
                    }
                },
                error: function () {
                    Swal.fire(
                        'Error!',
                        'Unable to delete Department.',
                        'error'
                    );
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            Swal.fire(
                'Cancelled',
                'Department deletion cancelled.',
                'info'
            );
        }
    });
});

function loadDepartments() {
    $.ajax({
        url: '/Departments/GetDepartments',
        type: 'GET',
        success: function (response) {
            var object = '';
            if (!response || response.length === 0) {
                object += '<p class="text-center">Department Not Available</p>';
            } else {
                object += '<table id="departmentTable" class="table table-striped table-bordered table-hover">';
                object += '<thead><tr><th>Department Name</th><th>Department Budget</th><th>Start Date</th><th>Administrator</th><th>Action</th></tr></thead>';
                object += '<tbody id="departmentTblBody">';
                $.each(response, function (index, item) {
                    object += '<tr id="departmentRow_' + item.departmentID + '">';
                    object += '<td>' + item.departmentName + '</td>';
                    object += '<td>' + item.budget + '</td>';
                    object += '<td>' + new Date(item.startDate).toLocaleDateString() + '</td>';
                    object += '<td>' + item.instructorName + '</td>';
                    object += '<td><a href="#" class="btn btn-primary editDepartment" data-id="' + item.departmentID + '">Edit</a> ';
                    object += '<a href="#" class="btn btn-danger deleteDepartment" data-id="' + item.departmentID + '">Delete</a></td>';
                    object += '</tr>';
                });
                object += '</tbody></table>';
            }

            $('#DepartmentSection').html(object);

            $('#departmentTable').DataTable({
                "paging": true,
                "ordering": true,
                "info": true,
            });
        },
        error: function () {
            toastr.error('Unable to fetch departments.');
        }
    });
}

function loadInstructors(callback) {
    $.ajax({
        url: '/Departments/GetInstructors',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            var dropdown = $('#InstructorID');
            dropdown.empty();
            dropdown.append($('<option>').val('').text('Select Instructor'));
            $.each(response, function (index, item) {
                dropdown.append($('<option>').val(item.instructorID).text(item.administrator));
            });

            if (callback) {
                callback();
            }
        },
        error: function () {
            toastr.error('Unable to fetch instructors.');
        }
    });
}