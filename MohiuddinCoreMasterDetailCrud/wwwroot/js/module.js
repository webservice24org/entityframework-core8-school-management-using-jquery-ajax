$(document).ready(function () {
    fetchAndDisplayModules();

    $('#addModulesBtn').click(function () {
        loadCourses();
        $('#ModulesModalLabel').text("Insert Course Modules");
        $('#saveModule').show();
        $('#updateModule').hide();
        $('#ModulesModal').modal('show');

    });

    $("#saveModule").click(function () {
        var modules = [];

        $("#modulesTable tbody tr").each(function () {
            var moduleName = $(this).find("input[name^='Modules'][name$='ModuleName']").val();
            var duration = $(this).find("input[name^='Modules'][name$='Duration']").val();

            if (moduleName && duration) {
                modules.push({
                    ModuleName: moduleName,
                    Duration: duration
                });
            }
        });

        var model = {
            CourseId: $("#CourseId").val(),
            Modules: modules
        };

        $.ajax({
            url: '/Modules/InsertModules',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                if (response.success) {
                    fetchAndDisplayModules();
                    resetForm();
                    toastr.success(response.message);
                    $('#ModulesModal').modal('hide');
                } else {
                    toastr.error(response.message);
                
                }
            },
            error: function (xhr, status, error) {
                toastr.error(xhr.responseText);
            }
        });
    });

    $(document).on('click', '#updateModule', function (e) {
        e.preventDefault();

        var modules = [];
        $('#modulesTable tbody tr').each(function () {
            var module = {
                ModuleName: $(this).find('input[name*="ModuleName"]').val(),
                Duration: $(this).find('input[name*="Duration"]').val()
            };
            modules.push(module);
        });

        var model = {
            CourseId: $('#CourseId').val(),
            Modules: modules
        };

        $.ajax({
            url: '/Modules/UpdateModules',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    $('#ModulesModal').modal('hide');
                    resetForm();
                    fetchAndDisplayModules()
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (error) {
                toastr.error(response.message);
            }
        });
    });

    function resetForm() {
        $('#ModulesForm')[0].reset();
        $('#CourseId').val('0');
        $('#modulesTable tbody').empty();
    }

    $(document).on('click', '.deleteAllModules', function (e) {
        e.preventDefault();
        const courseId = $(this).data('course-id');

        Swal.fire({
            title: 'Are you sure?',
            text: 'You are about to delete all modules for this course!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, cancel!',
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Modules/DeleteAllModules',
                    type: 'POST',
                    data: { courseId: courseId },
                    success: function (response) {
                        if (response.success) {
                            Swal.fire(
                                'Deleted!',
                                response.message,
                                'success'
                            ).then(() => {
                                fetchAndDisplayModules(); 
                                toastr.success('All modules deleted!');
                            });
                        } else {
                            Swal.fire(
                                'Error!',
                                response.message,
                                'error'
                            );
                        }
                    },
                    error: function (error) {
                        Swal.fire(
                            'Error!',
                            'Error deleting modules: ' + error.responseText,
                            'error'
                        );
                    }
                });
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire(
                    'Cancelled',
                    'Deletion cancelled.',
                    'error'
                );
            }
        });
    });

    $(document).on('click', '.editAllModules', function (e) {
        e.preventDefault();
        const courseId = $(this).data('course-id');

        $.ajax({
            url: '/Modules/GetModulesByCourseId',
            type: 'GET',
            data: { courseId: courseId },
            success: function (response) {
                $.when(loadCourses(courseId)).done(function () {
                    populateModulesModal(response);

                    $('#CourseId').val(courseId);
                    $('#existingCourseId').val(courseId);

                    $('#ModulesModalLabel').text("Edit Course's Modules");
                    $('#saveModule').hide();
                    $('#updateModule').show();
                    $('#ModulesModal').modal('show');
                });
            },
            error: function (error) {
                alert('Error fetching modules: ' + error.responseText);
            }
        });
    });

    function populateModulesModal(modules) {
        $('#modulesTable tbody').empty();

        modules.forEach((module, index) => {
            addModuleRow(module);
        });
    }


    function fetchAndDisplayModules() {
        $.ajax({
            url: '/Modules/GetAllModules',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                displayModules(data);
            },
            error: function (error) {
                console.log("Error fetching modules: ", error);
            }
        });
    }

    function displayModules(data) {
        const groupedModules = groupBy(data, 'courseName');
        let html = '<div class="accordion" id="modulesAccordion">';

        for (const courseName in groupedModules) {
            const accordionId = courseName.replace(/\s+/g, '') + 'Accordion';
            const headingId = courseName.replace(/\s+/g, '') + 'Heading';
            const courseId = groupedModules[courseName][0].courseId; // assuming all modules have the same courseId

            html += `
            <div class="accordion-item">
                <h2 class="accordion-header" id="${headingId}">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#${accordionId}" aria-expanded="false" aria-controls="${accordionId}">
                        ${courseName}
                    </button>
                </h2>
                <div id="${accordionId}" class="accordion-collapse collapse" aria-labelledby="${headingId}" data-bs-parent="#modulesAccordion">
                    <div class="accordion-body">
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th>Module Name</th>
                                    <th>Duration</th>
                                </tr>
                            </thead>
                            <tbody>`;

            groupedModules[courseName].forEach(module => {
                html += `
                                <tr>
                                    <td>${module.moduleName}</td>
                                    <td>${module.duration}</td>
                                </tr>`;
            });

            html += `
                            </tbody>
                        </table>
                        <a href="#" data-course-id="${courseId}" class="btn btn-danger deleteAllModules">Delete All</a>
                        <a href="#" data-course-id="${courseId}" class="btn btn-primary editAllModules">Edit All</a>
                    </div>
                </div>
            </div>`;
        }

        html += '</div>';
        $('#ModulesSection').html(html);
    }

    function groupBy(array, key) {
        return array.reduce((result, currentValue) => {
            (result[currentValue[key]] = result[currentValue[key]] || []).push(currentValue);
            return result;
        }, {});
    }

    function loadCourses(selectedCourseId = 0, callback) {
        $.ajax({
            url: '/Modules/GetCourses',
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

    $(document).on('click', '.removeModuleBtn', function () {
        $(this).closest('tr').remove();
    });
    $('#addModuleBtn').click(function () {
        addModuleRow();
    });

    function addModuleRow(module = null) {
        var moduleIndex = $('#modulesTable tbody tr').length;
        var moduleRow = `
                <tr>
                    <td>
                        <input type="hidden" name="Modules[${moduleIndex}].ModuleId" value="${module ? module.moduleId : ''}" />
                        <input type="text" name="Modules[${moduleIndex}].ModuleName" class="form-control" value="${module ? module.moduleName : ''}" />
                    </td>
                    <td>
                        <input type="text" name="Modules[${moduleIndex}].Duration" class="form-control" value="${module ? module.duration : ''}" />
                    </td>
                    <td>
                        <button type="button" class="btn btn-danger removeModuleBtn">Remove</button>
                    </td>
                </tr>
            `;
        $('#modulesTable tbody').append(moduleRow);
    }

});