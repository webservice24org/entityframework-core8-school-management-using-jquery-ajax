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

$(document).ready(function () {
    fetchAndDisplayModules();
});

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
        const accordionId = courseName.replace(/\s+/g, '') + 'Accordion'; // Unique ID for each accordion
        const headingId = courseName.replace(/\s+/g, '') + 'Heading'; // Unique ID for each heading

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
                    <a href="#" class="btn btn-success">Details</a>
                    <a href="#" class="btn btn-success">Edit</a>
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