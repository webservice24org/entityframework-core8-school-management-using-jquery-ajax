$(document).ready(function () {
    $('#addInstructorBtn').click(function () {
        $('#instructorModalLabel').text("Insert Instructor's Information");
        $('#saveInstructor').show();
        $('#updateInstructor').hide();
        $('#instructorModal').modal('show');
        loadCourses();
    });

    $('#saveInstructor').click(function () {
        saveInstructor();
    });
    function saveInstructor() {
        var formData = new FormData($('#instructorForm')[0]);

        var selectedCourseIDs = [];
        $('#instructorModal input[type="checkbox"]:checked').each(function () {
            selectedCourseIDs.push($(this).val());
        });

        // Append the selected course IDs to formData
        formData.append('SelectedCourseIDs', JSON.stringify(selectedCourseIDs));

        $.ajax({
            url: '/Instructors/InsertInstructor',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    // Additional actions on success
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Error while saving instructor.');
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