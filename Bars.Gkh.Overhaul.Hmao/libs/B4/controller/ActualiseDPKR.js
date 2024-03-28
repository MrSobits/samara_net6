Ext.define('B4.controller.ActualiseDPKR', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],

    stores: [
        'DPKRActualCriterias',
    ],

    models: [
        'DPKRActualCriterias',
    ],

    views: [
        'actualisedpkr.Grid',
        'actualisedpkr.Panel',
        'actualisedpkr.EditWindow',
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainPanel', selector: 'actualisedpkrpanel' },
    ],

    codeParam: null,

    init: function () {        
        var me = this,
            actions = {
            };
        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainPanel() || Ext.widget('actualisedpkrpanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('DPKRActualCriterias').load();
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'actualisedpkrGridAspect',
            gridSelector: 'actualisedpkrgrid',
            editFormSelector: '#actualisedpkrEditWindow',
            storeName: 'DPKRActualCriterias',
            modelName: 'DPKRActualCriterias',
            editWindowView: 'actualisedpkr.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#actualisedpkrEditWindow #cbNumberApartments'] = { 'change': { fn: this.onChangeNumberApartments, scope: this } },
                actions['#actualisedpkrEditWindow #cbYearRepair'] = { 'change': { fn: this.onChangeYearRepair, scope: this } },
                actions['#actualisedpkrEditWindow #cbStructuralElementCount'] = { 'change': { fn: this.onChangeStructuralElementCount, scope: this } };
            },
            onChangeNumberApartments: function (field, newValue)
            {
                var form = this.getForm(),
                cbNumberApartmentsCondition = form.down('#cbNumberApartmentsCondition'),
                nfNumberApartments = form.down('#nfNumberApartments');
                if (newValue == true)
                {
                    cbNumberApartmentsCondition.setDisabled(false);
                    nfNumberApartments.setDisabled(false);
                }
                else
                {
                    cbNumberApartmentsCondition.setDisabled(true);
                    nfNumberApartments.setDisabled(true);
                }
            },
            onChangeYearRepair: function (field, newValue)
            {
                var form = this.getForm(),
                cbYearRepairCondition = form.down('#cbYearRepairCondition'),
                nfYearRepair = form.down('#nfYearRepair');
                if (newValue == true)
                {
                    cbYearRepairCondition.setDisabled(false);
                    nfYearRepair.setDisabled(false);
                }
                else
                {
                    cbYearRepairCondition.setDisabled(true);
                    nfYearRepair.setDisabled(true);
                }
            },
            onChangeStructuralElementCount: function (field, newValue)
            {
                var form = this.getForm(),
                cbStructuralElementCountCondition = form.down('#cbStructuralElementCountCondition'),
                nfStructuralElementCount = form.down('#nfStructuralElementCount');
                if (newValue == true)
                {
                    cbStructuralElementCountCondition.setDisabled(false);
                    nfStructuralElementCount.setDisabled(false);
                }
                else
                {
                    cbStructuralElementCountCondition.setDisabled(true);
                    nfStructuralElementCount.setDisabled(true);
                }
            },
        }]
});