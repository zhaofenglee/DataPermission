$(function () {
    var l = abp.localization.getResource("DataPermission");

    var permissionItemService = window.jS.abp.dataPermission.permissionItems.permissionItem;
    
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "DataPermission/PermissionItems/CreateModal",
        scriptUrl: "/Pages/DataPermission/PermissionItems/createModal.js",
        modalClass: "permissionItemCreate"
    });

    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "DataPermission/PermissionItems/EditModal",
        scriptUrl: "/Pages/DataPermission/PermissionItems/editModal.js",
        modalClass: "permissionItemEdit"
    });
    
    var getFilter = function() {
        return {
            filterText: $("#FilterItemText").val(),
            objectName: $("#ObjectNameFilter").val(),
            description: $("#DescriptionFilter").val(),
            targetType: $("#TargetTypeFilter").val(),
            canRead: (function () {
                var value = $("#CanReadFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            canCreate: (function () {
                var value = $("#CanCreateFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            canEdit: (function () {
                var value = $("#CanEditFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            canDelete: (function () {
                var value = $("#CanDeleteFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })()
        };
    };

    var dataTable = $("#PermissionItemsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[2, "asc"]],
        ajax: abp.libs.datatables.createAjax(permissionItemService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('DataPermission.PermissionItems.Edit'),
                                action: function (data) {
                                    editModal.open({
                                        id: data.record.id
                                    });
                                }
                            },
                            {
                                text: l("Copy"),
                                visible: abp.auth.isGranted('DataPermission.PermissionItems.Create'),
                                action: function (data) {
                                    permissionItemService.copy(data.record.id)
                                        .then(function () {
                                            dataTable.ajax.reload();
                                        });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('DataPermission.PermissionItems.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    permissionItemService.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
            //{ data: "objectName" },
            { data: "roleName" ,orderable: false},
            { data: "targetType" },
           
            {
                data: "canRead",
                render: function (canRead) {
                    return canRead ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            {
                data: "canCreate",
                render: function (canCreate) {
                    return canCreate ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            {
                data: "canEdit",
                render: function (canEdit) {
                    return canEdit ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            {
                data: "canDelete",
                render: function (canDelete) {
                    return canDelete ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            {
                data: "isActive",
                render: function (isActive) {
                    return isActive ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            { data: "description" },
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $("#NewPermissionItemButton").click(function (e) {
        e.preventDefault();
        createModal.open({ObjectName: $("#ObjectNameFilter").val()});
    });

    $("#SearchFormItem").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        permissionItemService.getDownloadToken().then(
            function(result){
                var input = getFilter();
                var url =  abp.appPath + 'api/data-permission/permission-items/as-excel-file' +
                    abp.utils.buildQueryString([
                        { name: 'downloadToken', value: result.token },
                        { name: 'filterText', value: input.filterText },
                        { name: 'objectName', value: input.objectName },
                        { name: 'description', value: input.description },
                        { name: 'targetType', value: input.targetType },
                        { name: 'canRead', value: input.canRead },
                        { name: 'canCreate', value: input.canCreate },
                        { name: 'canEdit', value: input.canEdit },
                        { name: 'canDelete', value: input.canDelete }
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

    // window.addEventListener("focus", function() {
    //     // 在用户切换回到选项卡时执行代码
    //     console.log("用户回到了这个选项卡");
    //     dataTable.ajax.reload();
    // });
    
    window.onload = function() {
        // 在这里放置需要在页面加载后执行的代码
        console.log("页面已经完全加载");
    };
});
