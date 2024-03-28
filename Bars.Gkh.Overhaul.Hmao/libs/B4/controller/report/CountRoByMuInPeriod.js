Ext.define('B4.controller.report.CountRoByMuInPeriod', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CountRoByMuInPeriodPanel',
    mainViewSelector: 'countrobymuinperiodpanel',

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
        'report.CountRoByMuInPeriodPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'nfStart',
            selector: 'countrobymuinperiodpanel [name=StartYear]'
        },
        {
            ref: 'nfEnd',
            selector: 'countrobymuinperiodpanel [name=EndYear]'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: 'countrobymuinperiodpanel [name=Municipalities]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'countrobymuinperiodpanelMultiselectwindowaspect',
            fieldSelector: 'countrobymuinperiodpanel [name=Municipalities]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#countromubyperiodPanelMunicipalitySelectWindow',
            storeSelect: 'dict.municipality.ByParam',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        store: Ext.create('B4.store.dict.municipality.ByParam'),
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' }
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

                me.setLimits(nfStart, nfEnd, obj.data.ProgrammPeriodStart, obj.data.ProgrammPeriodEnd);
            })
            .error(function() {
                me.setLimits(nfStart, nfEnd, 2014, 2043);
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