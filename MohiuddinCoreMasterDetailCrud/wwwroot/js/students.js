$(document).ready(function () {
    resetStudentModal();
    GetStudents();

    $('#addStudentBtn').click(function () {
        loadCourses();
        $('#StudentModalLabel').text("Insert Student Information");
        $('#saveStudent').show();
        $('#updateStudent').hide();
        resetStudentModal();
        $('#StudentModal').modal('show');
    });

    $(document).on('click', '.editStudentBtn', function () {
        var studentId = $(this).data('id');
        $.ajax({
            url: '/Students/EditStudent',
            type: 'GET',
            data: { id: studentId },
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var student = response.data;
                    $('#StudentId').val(student.studentId);
                    $('#StudentName').val(student.studentName);
                    var dobDate = new Date(student.dob);
                    var formattedDob = dobDate.getFullYear() + '-' + ('0' + (dobDate.getMonth() + 1)).slice(-2) + '-' + ('0' + dobDate.getDate()).slice(-2);
                    $('#Dob').val(formattedDob);
                    $('#MobileNo').val(student.mobile);
                    $('#IsEnrolled').prop('checked', student.isEnrolled);
                    loadCourses(student.courseId, function () {
                        $('#CourseId').val(student.courseId);
                    });

                    if (student.imageUrl) {
                        $('#imageFile').attr('src', '/Images/' + student.imageUrl).show();
                    } else {
                        $('#imageFile').attr('src', '').hide();
                    }

                    $('#PresentAddress').val(student.presentAddress);
                    $('#PermanentAddress').val(student.permanentAddress);
                    $('#GuardianName').val(student.guardianName);
                    $('#RelationWithGuardian').val(student.relationWithGuardian);
                    $('#GuardianMobile').val(student.guardianMobile);

                    $('#StudentModalLabel').text('Edit Student');
                    $('#saveStudent').hide();
                    $('#updateStudent').show();
                    $('#StudentModal').modal('show');
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Unable to fetch student data.');
            }
        });
    });

    $('#StudentModal').on('hidden.bs.modal', function () {
        resetStudentModal();
    });

    $(document).on('click', '.detailsStudentBtn', function () {
        var studentId = $(this).data('id');
        $.ajax({
            url: '/Students/GetStudentDetails',
            type: 'GET',
            data: { id: studentId },
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var student = response.data;

                    $('#stdDetailsModalName').text(student.studentName);
                    var modalBody = '<div class="row">';

                    modalBody += '<div class="col-md-8">';
                    modalBody += '<div class="table-responsive">';
                    modalBody += '<table class="table table-bordered">';
                    modalBody += '<thead>';
                    modalBody += '<tr><th colspan="2">Student Details</th></tr>';
                    modalBody += '</thead>';
                    modalBody += '<tbody>';
                    modalBody += '<tr><td><strong>Student ID</strong></td><td>' + student.studentId + '</td></tr>';
                    modalBody += '<tr><td><strong>Date of Birth</strong></td><td>' + new Date(student.dob).toLocaleDateString() + '</td></tr>';
                    modalBody += '<tr><td><strong>Mobile No</strong></td><td>' + student.mobile + '</td></tr>';
                    modalBody += '<tr><td><strong>Present Address</strong></td><td>' + student.studentDetails.presentAddress + '</td></tr>';
                    modalBody += '<tr><td><strong>Permanent Address</strong></td><td>' + student.studentDetails.permanentAddress + '</td></tr>';
                    modalBody += '<tr><td><strong>Guardian Name</strong></td><td>' + student.studentDetails.guardianName + '</td></tr>';
                    modalBody += '<tr><td><strong>Relation With Guardian</strong></td><td>' + student.studentDetails.relationWithGuardian + '</td></tr>';
                    modalBody += '<tr><td><strong>Guardian Mobile</strong></td><td>' + student.studentDetails.guardianMobile + '</td></tr>';
                    modalBody += '</tbody>';
                    modalBody += '</table>';
                    modalBody += '</div>';
                    modalBody += '</div>';

                    modalBody += '<div class="col-md-4">';
                    modalBody += '<p><strong>Profile Picture:</strong><br><img src="/Images/' + student.imageUrl + '" alt="' + student.studentName + '" class="mt-2 img-thumbnail"></p>';
                    modalBody += '</div>';

                    modalBody += '</div>';

                    modalBody += '<h5>Enrolled Courses and Modules</h5>';
                    if (student.enrollments && student.enrollments.length > 0) {
                        $.each(student.enrollments, function (index, enrollment) {
                            modalBody += '<h5> Course Name: ' + enrollment.courseName + '</h5>';
                            if (enrollment.modules && enrollment.modules.length > 0) {
                                modalBody += '<div class="table-responsive">';
                                modalBody += '<table class="table table-bordered">';
                                modalBody += '<thead>';
                                modalBody += '<tr>';
                                modalBody += '<th>Module Name</th>';
                                modalBody += '<th>Duration (hours)</th>';
                                modalBody += '</tr>';
                                modalBody += '</thead>';
                                modalBody += '<tbody>';
                                $.each(enrollment.modules, function (index, module) {
                                    modalBody += '<tr>';
                                    modalBody += '<td>' + module.moduleName + '</td>';
                                    modalBody += '<td>' + module.duration + '</td>';
                                    modalBody += '</tr>';
                                });
                                modalBody += '</tbody>';
                                modalBody += '</table>';
                                modalBody += '</div>';
                            } else {
                                modalBody += '<p>No modules available for this course.</p>';
                            }
                        });
                    } else {
                        modalBody += '<p>No courses enrolled.</p>';
                    }

                    $('#stdDetailsModal .modal-body').html(modalBody);
                    $('#stdDetailsModal').modal('show');

                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Unable to fetch student details.');
            }
        });
    });

    $('#updateStudent').click(function () {
        saveStudent();
    });
    $('#saveStudent').click(function () {
        saveStudent();
    });

    function saveStudent() {
        var formData = new FormData($('#studentForm')[0]);
        $.ajax({
            url: $('#StudentId').val() == '0' ? '/Students/InsertStudent' : '/Students/UpdateStudent',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    GetStudents();
                    $('#StudentModal').modal('hide');
                    resetStudentModal();
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Error occurred while saving student.');
            }
        });
    }

    function loadCourses(selectedCourseId = 0, callback) {
        $.ajax({
            url: '/Students/GetCourses',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var options = '<option value="0">---Select Course---</option>';
                $.each(response, function (index, item) {
                    options += `<option value="${item.value}" ${item.value == selectedCourseId ? 'selected' : ''}>${item.text}</option>`;
                });
                $('#CourseId').html(options);

                if (callback) {
                    callback();
                }
            },
            error: function () {
                toastr.error('Unable to fetch courses.');
            }
        });
    }

    $(document).on('click', '.deleteStudentBtn', function () {
        var studentId = $(this).data('id');
        Swal.fire({
            title: 'Are you sure?',
            text: 'You will not be able to recover this student and associated modules!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, cancel',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Students/DeleteStudent',
                    type: 'POST',
                    data: { id: studentId },
                    dataType: 'json',
                    success: function (response) {
                        if (response.success) {
                            Swal.fire(
                                'Deleted!',
                                response.message,
                                'success'
                            ).then(() => {
                                GetStudents();
                                toastr.warning("Student data deleted!");
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
                            'Unable to delete student.',
                            'error'
                        );
                    }
                });
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire(
                    'Cancelled',
                    'Student deletion cancelled.',
                    'info'
                );
            }
        });
    });
    function GetStudents() {
        $.ajax({
            url: '/Students/GetStudents',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var object = '';
                if (!response.success || response.data.length === 0) {
                    object += '<p class="text-center">No students available</p>';
                } else {
                    object += '<table id="studentTable" class="table table-striped table-bordered table-hover">';
                    object += '<thead><tr><th>Student ID</th><th>Student Name</th><th>Mobile No</th><th>Date of Birth</th><th>Image</th><th>Action</th></tr></thead>';
                    object += '<tbody id="studentTblBody">';
                    $.each(response.data, function (index, item) {
                        object += '<tr id="studentRow_' + item.studentId + '">';
                        object += '<td>' + item.studentId + '</td>';
                        object += '<td>' + item.studentName + '</td>';
                        object += '<td>' + item.mobileNo + '</td>';
                        object += '<td>' + new Date(item.dob).toLocaleDateString() + '</td>';
                        object += '<td><img src="/Images/' + item.imageUrl + '" alt="' + item.studentName + '" style="max-width: 100px; max-height: 100px;"></td>';
                        object += '<td>';
                        object += '<a href="#" class="btn btn-info detailsStudentBtn" data-id="' + item.studentId + '">Details</a> ';
                        object += '<a href="#" class="btn btn-primary editStudentBtn" data-id="' + item.studentId + '">Edit</a> ';
                        object += '<a href="#" class="btn btn-danger deleteStudentBtn" data-id="' + item.studentId + '">Delete</a>';
                        object += '</td>';
                        object += '</tr>';
                    });
                    object += '</tbody></table>';
                }

                $('#studentBody').html(object);

                $('#studentTable').DataTable({
                    "paging": true,
                    "ordering": true,
                    "info": true,
                    "responsive": true,
                    "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
                    "columnDefs": [
                        { "orderable": false, "targets": -1 }
                    ]
                });
            },
            error: function () {
                toastr.error('Unable to fetch student data.');
            }
        });
    }

    function resetStudentModal() {
        $('#studentForm')[0].reset();
        $('#StudentId').val('0');
        $('#CourseId').val('0');
        $('#ProfileFile').val('');

    }
    

});

function readUrl(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imageFile').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}