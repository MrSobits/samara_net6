Ext.define('B4.controller.report.HeatSeasonReceivedDocuments', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.HeatSeasonReceivedDocumentsPanel',
    mainViewSelector: '#heatSeasonReceivedDocumentsPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.HeatTypeForSelect',
        'dict.HeatTypeForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    municipalityTriggerFieldSelector: '#heatSeasonReceivedDocumentsPanel #tfMunicipality',
    reportDateFieldSelector: '#heatSeasonReceivedDocumentsPanel #dfReportDate',
    heatSeasonPeriodFieldSelector: '#heatSeasonReceivedDocumentsPanel #sfHeatSeasonPeriodGji',
    heatTypeFieldSelector: '#heatSeasonReceivedDocumentsPanel #cbHeatType',
    realityObjectTypeFieldSelector: '#heatSeasonReceivedDocumentsPanel #cbRoType',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'heatSeasonReceivedDocumentsMultiselectwindowaspect',
            fieldSelector: '#heatSeasonReceivedDocumentsPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#heatSeasonReceivedDocumentsMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'heatSeasonReceivedDocumentsHeatTypewindowaspect',
            fieldSelector: '#heatSeasonReceivedDocumentsPanel #cbHeatType',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#heatSeasonReceivedDocumentsHeatTypeSelectWindow',
            storeSelect: 'dict.HeatTypeForSelect',
            storeSelected: 'dict.HeatTypeForSelected',
            columnsGridSelect: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    onLaunch: function () {
        var me = this,
            sfHeatSeasonPeriod = Ext.ComponentQuery.query(me.heatSeasonPeriodFieldSelector)[0];
        me.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetCurrentPeriod', 'HeatSeasonPeriodGji'),
            method: 'GET'
        }).next(function (response) {
            me.unmask();
            //десериализуем полученную строку
            if (!Ext.isEmpty(response.responseText)) {
                var obj = Ext.JSON.decode(response.responseText);

                if (obj) {
                    sfHeatSeasonPeriod.setValue({ Id: obj.periodId, Name: obj.periodName });
                }
            }
        }).error(function () {
            me.unmask();
        });
    },
    
    validateParams: function () {
        var reportDateField = Ext.ComponentQuery.query(this.reportDateFieldSelector)[0];
        if (!reportDateField.isValid()) {
            return "Не указан параметр \"Отопительный сезон\"";
        }

        var heatSeasonPeriodField = Ext.ComponentQuery.query(this.heatSeasonPeriodFieldSelector)[0];
        if (!heatSeasonPeriodField.isValid()) {
            return "Не указан параметр \"Дата отчета\"";
        }
        
        return true;
    },

    getParams: function () {

        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var reportDateField = Ext.ComponentQuery.query(this.reportDateFieldSelector)[0];
        var heatSeasonPeriodField = Ext.ComponentQuery.query(this.heatSeasonPeriodFieldSelector)[0];
        var heatTypeField = Ext.ComponentQuery.query(this.heatTypeFieldSelector)[0];
        var realityObjectTypeField = Ext.ComponentQuery.query(this.realityObjectTypeFieldSelector)[0];
        
        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            heatSeasonPeriod: (heatSeasonPeriodField ? heatSeasonPeriodField.getValue() : null),
            realityObjectType: (realityObjectTypeField ? realityObjectTypeField.getValue() : null),
            heatType: (heatTypeField ? heatTypeField.getValue() : null)
        };
    }
});