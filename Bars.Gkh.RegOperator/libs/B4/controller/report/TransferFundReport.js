Ext.define('B4.controller.report.TransferFundReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.TransferFundReportPanel',
    mainViewSelector: 'transferfundreportpanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.ux.button.Update'
    ],

    stores: [
        'realityobj.ByManOrgAndContractDate',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'ManOrgSelectField',
            selector: 'transferfundreportpanel [name=ManagingOrganization]'
        },
        {
            ref: 'StartDateField',
            selector: 'transferfundreportpanel [name=StartDate]'
        },
        {
            ref: 'EndDateField',
            selector: 'transferfundreportpanel [name=EndDate]'
        },
        {
            ref: 'RealObjsTriggerField',
            selector: 'transferfundreportpanel [name=RealObjs]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'transferFundRealObjsMultiselectwindowaspect',
            fieldSelector: 'transferfundreportpanel [name=RealObjs]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#transferFundManOrgSelectWindow',
            storeSelect: 'realityobj.ByManOrgAndContractDate',
            storeSelected: 'realityobj.RealityObjectForSelected',
            textProperty: 'Address',
            columnsGridSelect: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1 }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params.date = new Date(2014, 1, 1);
                operation.params.manOrgId = this.controller.getManOrgSelectField().getValue();
            }
        }
    ],

    init: function () {
        var actions = {};
        actions['transferfundreportpanel [name=ManagingOrganization]'] = {
            'change': { fn: this.ClearRealObjs, scope: this },
            'trigger2Click': { fn: this.ClearRealObjs, scope: this }
        };
        this.control(actions);

        this.callParent(arguments);
    },

    ClearRealObjs: function () {
        this.getRealObjsTriggerField().onTrigger2Click();
    },

    getParams: function () {
        var manOrgField = this.getManOrgSelectField(),
            dateStartField = this.getStartDateField(),
            dateEndField = this.getEndDateField(),
            realObjsField = this.getRealObjsTriggerField();

        return {
            manOrgId: (manOrgField ? manOrgField.getValue() : null),
            startDate: (dateStartField ? dateStartField.getValue() : null),
            endDate: (dateEndField ? dateEndField.getValue() : null),
            roIds: (realObjsField ? realObjsField.getValue() : null)
        };
    }
});