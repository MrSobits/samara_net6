Ext.define('B4.controller.report.CountCeoByMuInPeriod', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CountCeoByMuInPeriodPanel',
    mainViewSelector: 'countceobymuinperiodpanel',

    requires: [
        'B4.Url', 'B4.Ajax',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update',
        'dict.municipality.ByParam'
    ],

    views: [
        'report.CountCeoByMuInPeriodPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'nfStart',
            selector: 'countceobymuinperiodpanel [name=StartYear]'
        },
        {
            ref: 'nfEnd',
            selector: 'countceobymuinperiodpanel [name=EndYear]'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: 'countceobymuinperiodpanel [name=Municipalities]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'countceobymuinperiodpanelMultiselectwindowaspect',
            fieldSelector: 'countceobymuinperiodpanel [name=Municipalities]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#countceomubyperiodPanelMunicipalitySelectWindow',
            storeSelect: 'dict.municipality.ByParam',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        store: Ext.create('B4.store.dict.municipality.ByParam')
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
            nfStart = me.getNfStart(),
            nfEnd = me.getNfEnd();

        B4.Ajax.request(B4.Url.action('GetParams', 'Params'))
            .next(function(response) {
                var obj = Ext.decode(response.responseText);

                me.setLimits(nfStart, nfEnd, obj.data.ProgrammPeriodStart || 2014, obj.data.ProgrammPeriodEnd || 2034);
            })
            .error(function() {
                me.setLimits(nfStart, nfEnd, 2014, 2034);
            });
    },

    setLimits: function (nfStart, nfEnd, minValue, maxValue) {
        //начальный год является минимальным, конечный - максимальным
        //проставляем их еще и как параметры по умолчанию
        nfStart.minValue = nfEnd.minValue = minValue;
        nfStart.setValue(minValue);
        nfStart.maxValue = nfEnd.maxValue = maxValue;
        nfEnd.setValue(maxValue);
    },

    validateParams: function () {
        var nfStart = this.getNfStart(),
            nfEnd = this.getNfEnd();
        if (nfStart.getValue() > nfEnd.getValue()) {
            return 'Год окончания должен быть больше года начала';
        }

        return true;
    },

    getParams: function () {
        var mcpField = this.getMunicipalityTriggerField(),
            nfStart = this.getNfStart(),
            nfEnd = this.getNfEnd();
        
        return {
            municipalityIds: mcpField.getValue(),
            startYear: nfStart.getValue(),
            endYear: nfEnd.getValue()
        };
    }
});