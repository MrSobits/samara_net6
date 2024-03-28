Ext.define('B4.controller.report.PrepareHeatSeason', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.PrepareHeatSeasonPanel',
    mainViewSelector: '#reportPrepareHeatSeasonPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    municipalityTriggerFieldSelector : '#reportPrepareHeatSeasonPanel #tfMunicipality',
    reportDateFieldSelector: '#reportPrepareHeatSeasonPanel #dfReportDate',
    heatSeasonPeriodFieldSelector: '#reportPrepareHeatSeasonPanel #sfHeatSeasonPeriodGji',
    realityObjectTypeFieldSelector: '#reportPrepareHeatSeasonPanel #cbRoType',
    docTypeFieldSelector: '#reportPrepareHeatSeasonPanel #cbDocType',
    stateFieldSelector: '#reportPrepareHeatSeasonPanel #cbState',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportPrepareHeatSeasonMultiselectwindowaspect',
            fieldSelector: '#reportPrepareHeatSeasonPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportPrepareHeatSeasonPanelMunicipalitySelectWindow',
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
        }
    ],

    onLaunch: function () {
        var me = this,
        sfHeatSeasonPeriod = Ext.ComponentQuery.query(this.heatSeasonPeriodFieldSelector)[0];
           
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
        var realityObjectTypeField = Ext.ComponentQuery.query(this.realityObjectTypeFieldSelector)[0];
        var docTypeField = Ext.ComponentQuery.query(this.docTypeFieldSelector)[0];
        var stateField = Ext.ComponentQuery.query(this.stateFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            heatSeasonPeriod: (heatSeasonPeriodField ? heatSeasonPeriodField.getValue() : null),
            realityObjectType: (realityObjectTypeField ? realityObjectTypeField.getValue() : null),
            docType: (docTypeField ? docTypeField.getValue() : null),
            state: (stateField ? stateField.getValue() : null)
        };
    }
});