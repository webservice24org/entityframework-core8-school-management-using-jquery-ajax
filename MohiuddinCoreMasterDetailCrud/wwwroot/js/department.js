$(document).ready(function () {
    loadDepartments();

    $('#saveDepartment').click(function () {
        saveDepartment();
    });
    $('#updateDepartment').click(function () {
        saveDepartment();
    });
});

$('#addDepartmentBtn').click(function () {
    $('#DepartmentModallLabel').text("Insert Department Information");
    $('#saveDepartment').show();
    $('#updateDepartment').hide();
    $('#DepartmentModal').modal('show');
    $('#DepartmentID').val(0);  // Set the hidden field for department ID to 0 for new records
    loadInstructors();
});


function getDepartment(id) {
    $.ajax({
        url: '/Departments/GetDepartment',
        type: 'GET',
        data: { id: id },
        success: function (response) {
            if (response.success) {
                // Populate your form with the department data
                $('#DepartmentID').val(response.data.DepartmentID);
                $('#DepartmentName').val(response.data.DepartmentName);
                $('#Budget').val(response.data.Budget);
                $('#StartDate').val(response.data.StartDate);
                $('#InstructorID').val(response.data.InstructorID);
                // Show your modal or form
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error('Unable to fetch department.');
        }
    });
}

function saveDepartment() {
    var formData = new FormData($('#DepartmentForm')[0]);

    $.ajax({
        url: '/Departments/CreateDepartment',
        type: 'POST',
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                $('#DepartmentForm')[0].reset();
                $('#DepartmentModal').modal('hide');  // Hide the modal after saving
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

function deleteDepartment(id) {
    $.ajax({
        url: '/Departments/DeleteDepartment',
        type: 'DELETE',
        data: { id: id },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                loadDepartments();
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error('Unable to delete department.');
        }
    });
}

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
                    object += '<td><a href="" class="btn btn-primary editDepartment" data-id="' + item.DepartmentID + '">Edit</a> ';
                    object += '<a href="" class="btn btn-danger deleteDepartment" data-id="' + item.DepartmentID + '">Delete</a></td>';
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