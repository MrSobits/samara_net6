Ext.define('B4.controller.ActualiseSubProgram', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],

    stores: [
        'SubProgramCriterias',
    ],

    models: [
        'SubProgramCriterias',
    ],

    views: [
        'subprogram.Grid',
        'subprogram.Panel',
        'subprogram.EditWindow',
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainPanel', selector: 'subprogrampanel'},
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
        var view = this.getMainPanel() || Ext.widget('subprogrampanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('SubProgramCriterias').load();
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'subprogramGridAspect',
            gridSelector: 'subprogramgrid',
            editFormSelector: '#subprogramEditWindow',
            storeName: 'SubProgramCriterias',
            modelName: 'SubProgramCriterias',
            editWindowView: 'subprogram.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#subprogramEditWindow #cbStatusUsed'] = { 'change': { fn: this.onChangeStatusUsed, scope: this } },
                actions['#subprogramEditWindow #cbTypeHouseUsed'] = { 'change': { fn: this.onChangeTypeHouseUsed, scope: this } },
                actions['#subprogramEditWindow #cbConditionHouseUsed'] = { 'change': { fn: this.onChangeConditionHouseUsed, scope: this } };
                actions['#subprogramEditWindow #cbNumberApartmentsUsed'] = { 'change': { fn: this.onChangeNumberApartmentsUsed, scope: this } };
                actions['#subprogramEditWindow #cbYearRepairUsed'] = { 'change': { fn: this.onChangebYearRepairUsed, scope: this } };
                actions['#subprogramEditWindow #cbIsRepairNotAdvisableUsed'] = { 'change': { fn: this.onChangeIsRepairNotAdvisableUsed, scope: this } },
                actions['#subprogramEditWindow #cbIsNotInvolvedCrUsed'] = { 'change': { fn: this.onChangeIsNotInvolvedCrUsed, scope: this } },
                actions['#subprogramEditWindow #cbStructuralElementCountUsed'] = { 'change': { fn: this.onChangeStructuralElementCountUsed, scope: this } };
                actions['#subprogramEditWindow #cbFloorCountUsed'] = { 'change': { fn: this.onChangeFloorCountUsed, scope: this } };
                actions['#subprogramEditWindow #cbLifetimeUsed'] = { 'change': { fn: this.onChangeLifetimeUsed, scope: this } };
            },
            onChangeStatusUsed: function (field, newValue)
            {
                var form = this.getForm(),
                Status = form.down('#daStatus');

                Status.setDisabled(!newValue);
            },
            onChangeTypeHouseUsed: function (field, newValue)
            {
                var form = this.getForm(),
                TypeHouse = form.down('#ecTypeHouse');

                TypeHouse.setDisabled(!newValue);
            },
            onChangeConditionHouseUsed: function (field, newValue)
            {
                var form = this.getForm(),
                ConditionHouse = form.down('#cbConditionHouse');

                ConditionHouse.setDisabled(!newValue);
            },
            onChangeNumberApartmentsUsed: function (field, newValue)
            {
                var form = this.getForm(),
                    NumberApartmentsCondition = form.down('#cbNumberApartmentsCondition'),
                    NumberApartments = form.down('#nfNumberApartments');

                NumberApartmentsCondition.setDisabled(!newValue);
                NumberApartments.setDisabled(!newValue);
            },
            onChangebYearRepairUsed: function (field, newValue)
            {
                var form = this.getForm(),
                    YearRepairCondition = form.down('#cbYearRepairCondition'),
                    YearRepair = form.down('#nfYearRepair');

                YearRepairCondition.setDisabled(!newValue);
                YearRepair.setDisabled(!newValue);

            },
            onChangeIsRepairNotAdvisableUsed: function (field, newValue)
            {
                var form = this.getForm(),
                    RepairNotAdvisable = form.down('#cbRepairNotAdvisable');

                RepairNotAdvisable.setDisabled(!newValue);
            },
            onChangeIsNotInvolvedCrUsed: function (field, newValue)
            {
                var form = this.getForm(),
                    NotInvolvedCr = form.down('#cbNotInvolvedCr');

                NotInvolvedCr.setDisabled(!newValue);
            },
            onChangeStructuralElementCountUsed: function (field, newValue)
            {
                var form = this.getForm(),
                    cbStructuralElementCountCondition = form.down('#cbStructuralElementCountCondition'),
                    nfStructuralElementCount = form.down('#nfStructuralElementCount');;

                cbStructuralElementCountCondition.setDisabled(!newValue);
                nfStructuralElementCount.setDisabled(!newValue);
            },
            onChangeFloorCountUsed: function (field, newValue)
            {
                var form = this.getForm(),
                    FloorCountCondition = form.down('#cbFloorCountCondition'),
                    FloorCount = form.down('#nfFloorCount');;

                FloorCountCondition.setDisabled(!newValue);
                FloorCount.setDisabled(!newValue);
            },
            onChangeLifetimeUsed: function (field, newValue)
            {
                var form = this.getForm(),
                    LifetimeCondition = form.down('#cbLifetimeCondition'),
                    Lifetime = form.down('#nfLifetime');;

                LifetimeCondition.setDisabled(!newValue);
                Lifetime.setDisabled(!newValue);
            },
        }]
});