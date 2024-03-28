Ext.define('B4.controller.report.PlanOwnerCollection', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.PlanOwnerCollectionPanel',
    mainViewSelector: 'planownercollectionreportpanel',

    requires: [
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
        'SelectWindow.MultiSelectWindow',
        'report.PlanOwnerCollectionPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityField',
            selector: 'planownercollectionreportpanel gkhtriggerfield[name=Municipalities]'
        },
        {
            ref: 'YearField',
            selector: 'planownercollectionreportpanel combobox[name=Year]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'planownercollectionreportAspect',
            fieldSelector: 'planownercollectionreportpanel gkhtriggerfield[name="Municipalities"]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#planownercollectionreportpanelSelectWindow',
            storeSelect: 'dict.municipality.ByParam',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
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

    validateParams: function() {
        return true;
    },

    getParams: function() {
        var mcpField = this.getMunicipalityField(),
            yearField = this.getYearField();

        return {
            muIds: (mcpField ? mcpField.getValue() : null),
            year: (yearField ? yearField.getValue() : null)
        };
    },

    onLaunch: function() {
        var me = this,
            yearField = me.getYearField();

        B4.Ajax.request(
            B4.Url.action('GetParams', 'Params')
        ).next(function (resp) {
            var response = Ext.decode(resp.responseText),
                yearStart = response.data.ProgrammPeriodStart,
                shortTerm = response.data.ShortTermProgPeriod;

            for (var i = yearStart; i < yearStart + shortTerm; i++) {
                yearField.items.push([i, i]);
            }

            yearField.setValue(yearStart);

        }).error(function() {
            Ext.Msg.alert('Ошибка!', 'При получении параметров произошла ошибка!');
        });
    }
});