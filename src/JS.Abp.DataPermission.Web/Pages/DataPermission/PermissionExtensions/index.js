$(function () {
    var l = abp.localization.getResource("DataPermission");
	
	var permissionExtensionService = window.jS.abp.dataPermission.permissionExtensions.permissionExtension;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "DataPermission/PermissionExtensions/CreateModal",
        scriptUrl: "/Pages/DataPermission/PermissionExtensions/createModal.js",
        modalClass: "permissionExtensionCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "DataPermission/PermissionExtensions/EditModal",
        scriptUrl: "/Pages/DataPermission/PermissionExtensions/editModal.js",
        modalClass: "permissionExtensionEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            objectName: $("#ObjectNameFilter").val(),
			roleId: $("#RoleIdFilter").val(),
            roleName: $("#RoleNameFilter").val(),
			// excludedRoleId: $("#ExcludedRoleIdFilter").val(),
			permissionType: $("#PermissionTypeFilter").val(),
			lambdaString: $("#LambdaStringFilter").val(),
            isActive: (function () {
                var value = $("#IsActiveFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            description: $("#DescriptionFilter").val(),
        };
    };

    var dataTable = $("#PermissionExtensionsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(permissionExtensionService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('DataPermission.PermissionExtensions.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('DataPermission.PermissionExtensions.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    permissionExtensionService.delete(data.record.id)
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
			{ data: "roleName" },
			// { data: "excludedRoleId" },
            {
                data: "permissionType",
                render: function (permissionType) {
                    if (permissionType === undefined ||
                        permissionType === null) {
                        return "";
                    }

                    var localizationKey = "Enum:PermissionType." + permissionType;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
			{ data: "lambdaString" },
            { data: "description" },
            {
                data: "isActive",
                render: function (isActive) {
                    return isActive ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
        
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $("#NewPermissionExtensionButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        permissionExtensionService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/data-permission/permission-extensions/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'objectName', value: input.objectName }, 
                            { name: 'RoleName', value: input.roleName }, 
                            { name: 'excludedRoleId', value: input.excludedRoleId }, 
                            { name: 'permissionType', value: input.permissionType }, 
                            { name: 'lambdaString', value: input.lambdaString }, 
                            { name: 'isActive', value: input.isActive },
                            { name: 'description', value: input.description },
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
