$(function () {
    var l = abp.localization.getResource("DataPermission");
	
	var objectPermissionService = window.jS.abp.dataPermission.objectPermissions.objectPermission;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "DataPermission/ObjectPermissions/CreateModal",
        scriptUrl: "/Pages/DataPermission/ObjectPermissions/createModal.js",
        modalClass: "objectPermissionCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "DataPermission/ObjectPermissions/EditModal",
        scriptUrl: "/Pages/DataPermission/ObjectPermissions/editModal.js",
        modalClass: "objectPermissionEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            objectName: $("#ObjectNameFilter").val(),
			description: $("#DescriptionFilter").val()
        };
    };

    var dataTable = $("#ObjectPermissionsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(objectPermissionService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("EditDetails"),
                                visible: abp.auth.isGranted('DataPermission.ObjectPermissions'),
                                action: function(data) {
                                    window.location = "/DataPermission/ObjectPermissions/" + data.record.objectName;
                                }
                            },
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('DataPermission.ObjectPermissions.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('DataPermission.ObjectPermissions.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    objectPermissionService.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
			{ data: "objectName" },
			{ data: "description" }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $("#NewObjectPermissionButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        objectPermissionService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/data-permission/object-permissions/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'objectName', value: input.objectName }, 
                            { name: 'description', value: input.description }
                            ]);
                            
                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
            }
        )
    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reload();
        }
    });

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reload();
    });
    
    
});
