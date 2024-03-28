Ext.define('B4.controller.MassPercentCalculation', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],
    stores: ['dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'],
    views: ['MassPercentCalcPanel', 'SelectWindow.MultiSelectWindow'],
    
    municipalityTriggerFieldSelector: '#massPercentCalcPanel #tfMunicipality',
    periodDiSelectFieldSelector: '#massPercentCalcPanel #sfPeriodDi',
      
    mainView: 'MassPercentCalcPanel',
    mainViewSelector: '#massPercentCalcPanel',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects: [
           {
               xtype: 'gkhtriggerfieldmultiselectwindowaspect',
               name: 'massperccalcmultiselectwindowaspect',
               fieldSelector: '#massPercentCalcPanel #tfMunicipality',
               multiSelectWindow: 'SelectWindow.MultiSelectWindow',
               multiSelectWindowSelector: '#massPercCalcPanelMunicipalitySelectWindow',
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
    
    init: function () {
        var actions = {};
        actions['#massPercentCalcPanel #btnMassPercentCalc'] = { 'click': { fn: this.onClickMassPercCalc, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },
    
    onClickMassPercCalc: function () {
        
        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var periodDiSelectField = Ext.ComponentQuery.query(this.periodDiSelectFieldSelector)[0];

        this.mask('Расчет процентов...', this.getMainComponent());
        B4.Ajax.request({
            url: B4.Url.action('MassCalculate', 'PercentCalculation', {
                period: periodDiSelectField ? periodDiSelectField.getValue() : null,
                municipalityIds: mcpField ? mcpField.getValue() : null
            }),
            timeout: 1000 * 60 * 60 * 10 // 10 часов
        }).next(function (response) {
            //var obj = Ext.JSON.decode(response.responseText);
            Ext.Msg.alert('Cообщение', 'Расчет процентов произведен успешно');
            this.unmask();
        }, this).error(function () {
            Ext.Msg.alert('Ошибка!', 'Расчет процентов произведен ошибочно');
            this.unmask();
        }, this);
    }
});


