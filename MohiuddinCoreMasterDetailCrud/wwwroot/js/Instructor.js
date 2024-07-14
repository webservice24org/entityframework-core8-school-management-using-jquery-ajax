$(document).ready(function () {

    loadInstructors();
    $('#addInstructorBtn').click(function () {
        $('#instructorModalLabel').text("Insert Instructor's Information");
        $('#saveInstructor').show();
        $('#updateInstructor').hide();
        $('#instructorModal').modal('show');
        loadCourses();
    });

    $('#saveInstructor').on('click', function () {
        var formData = new FormData($('#instructorForm')[0]);

        $.ajax({
            url: '/Instructors/InsertInstructor',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    $('#instructorModal').modal('hide');
                    loadInstructors(); 
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('An error occurred while saving the instructor.');
            }
        });
    });


    function loadInstructors() {
        $.ajax({
            url: '/Instructors/GetInstructors',
            type: 'GET',
            success: function (response) {
                var instructorBody = $('#InstructorBody');
                instructorBody.empty();

                var instructorsTbl = $('<div id="InstructorsTbl"></div>');
                var table = $('<table id="instructorsTable" class="table table-striped table-bordered table-hover"></table>');
                var thead = $('<thead><tr><th>InstructorID</th><th>InstructorName</th><th>JoinDate</th><th>InstructorPicture</th><th>Actions</th></tr></thead>');
                var tbody = $('<tbody></tbody>');

                table.append(thead);

                $.each(response, function (index, instructor) {
                    var tr = $('<tr></tr>');
                    tr.append('<td>' + instructor.instructorID + '</td>');
                    tr.append('<td>' + instructor.instructorName + '</td>');
                    tr.append('<td>' + new Date(instructor.joinDate).toLocaleDateString() + '</td>');
                    tr.append('<td><img src="/Images/' + instructor.instructorPicture + '" alt="' + instructor.instructorName + '" style="max-width: 100px; max-height: 100px;"></td>');
                    tr.append('<td><a href="#" class="btn btn-info InstructorDetailsBtn" data-id="' + instructor.instructorID + '">Details</a><a href="#" class="btn btn-primary InstructorEditBtn" data-id="' + instructor.instructorID + '">Edit</a><a href="#" class="btn btn-danger InstructorDeleteBtn" data-id="' + instructor.instructorID + '">Delete</a></td>');

                    tbody.append(tr);
                });

                table.append(tbody);

                instructorsTbl.append(table);
                instructorBody.append(instructorsTbl);

                $('#instructorsTable').DataTable();
            },
            error: function () {
                toastr.error('Unable to fetch instructors.');
            }
        });
    }

    

    function loadCourses() {
        $.ajax({
            url: '/Instructors/GetAllCourses',
            type: 'GET',
            success: function (response) {
                var coursesDiv = $('#instructorModal #courses');
                coursesDiv.empty();
                var rowDiv = $('<div class="row"></div>');

                $.each(response, function (index, course) {
                    var colDiv = $(
                        '<div class="col-md-4">' +
                        '<div class="form-check">' +
                        '<input class="form-check-input" name="SelectedCourseIDs" type="checkbox" value="' + course.courseId + '" id="course_' + course.courseId + '">' +
                        '<label class="form-check-label" for="course_' + course.courseId + '">' +
                        course.courseName +
                        '</label>' +
                        '</div>' +
                        '</div>'
                    );
                    rowDiv.append(colDiv);
                });

                coursesDiv.append(rowDiv);
            },
            error: function () {
                toastr.error('Unable to fetch courses.');
            }
        });
    }


    
});

$(document).on('click', '.InstructorDetailsBtn', function () {
    var instructorId = $(this).data('id');
    $.ajax({
        url: '/Instructors/GetInstructorDetails?id=' + instructorId,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                var instructorDetails = response.instructorDetails;

                $('#InstructordetailsModalLabelName').text(instructorDetails.instructor.firstName);

                var modalBody = '<div class="row">';

                modalBody += '<div class="col-md-8">';
                modalBody += '<div class="table-responsive">';
                modalBody += '<table class="table table-bordered">';
                modalBody += '<thead><tr><th colspan="2">Instructor Details</th></tr></thead>';
                modalBody += '<tbody>';
                modalBody += '<tr><td><strong>Instructor ID</strong></td><td>' + instructorDetails.instructor.id + '</td></tr>';
                modalBody += '<tr><td><strong>Date of Birth</strong></td><td>' + new Date(instructorDetails.instructorDetails.dob).toLocaleDateString() + '</td></tr>';
                modalBody += '<tr><td><strong>Mobile No</strong></td><td>' + instructorDetails.instructor.mobile + '</td></tr>';
                modalBody += '<tr><td><strong>Join Date</strong></td><td>' + new Date(instructorDetails.instructor.joinDate).toLocaleDateString() + '</td></tr>';
                modalBody += '<tr><td><strong>Present Address</strong></td><td>' + instructorDetails.instructorDetails.presentAddress + '</td></tr>';
                modalBody += '<tr><td><strong>Permanent Address</strong></td><td>' + instructorDetails.instructorDetails.permanentAddress + '</td></tr>';
                modalBody += '<tr><td><strong>Salary</strong></td><td>' + instructorDetails.instructorDetails.salary + '</td></tr>';
                modalBody += '</tbody>';
                modalBody += '</table>';
                modalBody += '</div>';
                modalBody += '</div>';

                modalBody += '<div class="col-md-4">';
                modalBody += '<p><strong>Profile Picture:</strong><br><img src="/Images/' + instructorDetails.instructorDetails.instructorPicture + '" alt="' + instructorDetails.instructor.firstName + '" class="mt-2 img-thumbnail"></p>';
                modalBody += '</div>';

                modalBody += '</div>';

                modalBody += '<h5>Assigned Courses</h5>';
                if (instructorDetails.courses && instructorDetails.courses.length > 0) {
                    modalBody += '<div class="table-responsive">';
                    modalBody += '<table class="table table-bordered">';
                    modalBody += '<thead><tr><th>Course ID</th><th>Course Name</th><th>Department</th></tr></thead>';
                    modalBody += '<tbody>';
                    $.each(instructorDetails.courses, function (index, course) {
                        modalBody += '<tr>';
                        modalBody += '<td>' + course.courseId + '</td>';
                        modalBody += '<td>' + course.courseName + '</td>';
                        modalBody += '<td>' + course.departmentName + '</td>'; // Display DepartmentName
                        modalBody += '</tr>';
                    });
                    modalBody += '</tbody>';
                    modalBody += '</table>';
                    modalBody += '</div>';
                } else {
                    modalBody += '<p>No courses assigned.</p>';
                }

                $('#InstructordetailsModal .modal-body').html(modalBody);
                $('#InstructordetailsModal').modal('show');
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error('Unable to fetch Instructor details.');
        }
    });
});


