$(document).ready(function () {
    $('#addInstructorBtn').click(function () {
        $('#instructorModalLabel').text("Insert Instructor's Information");
        $('#saveInstructor').show();
        $('#updateInstructor').hide();
        $('#instructorModal').modal('show');
        loadCourses();
    });
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
                        '<input class="form-check-input" type="checkbox" value="' + course.courseId + '" id="course_' + course.courseId + '">' +
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