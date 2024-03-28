Ext.define('B4.controller.MaxSumByYear', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],

    stores: [
        'MaxSumByYear',
    ],

    models: [
        'MaxSumByYear',
    ],

    views: [
        'maxsumbyyear.Grid',
      //  'maxsumbyyear.Panel',
        'maxsumbyyear.EditWindow',
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'maxsumbyyeargrid' },
          {
              ref: 'editWindow',
              selector: '#maxsumbyyearEditWindow'
          }
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
        var view = this.getMainView() || Ext.widget('maxsumbyyeargrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('MaxSumByYear').load();
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'maxsumbyyearGridAspect',
            gridSelector: 'maxsumbyyeargrid',
            editFormSelector: '#maxsumbyyearEditWindow',
            storeName: 'MaxSumByYear',
            modelName: 'MaxSumByYear',
            editWindowView: 'maxsumbyyear.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#maxsumbyyearEditWindow #sfProgram'] = { 'beforeload': { fn: this.onBeforeLoadProgram, scope: this } };
            },
            onBeforeLoadProgram: function (store, operation, rec) {
                var me = this;
                operation = operation || {};
                operation.params = operation.params || {};
                var editWind = me.controller.getEditWindow();

                var cellMU = editWind.down('#sfMunicipality');
                if (cellMU.value)
                {
                    var municipality = cellMU.value['Id'];
                    operation.params.muId = municipality;
                }

                var checker = editWind.down('#cbShowActive');                
                var isChecked = checker.value;                
                operation.params.isChecked = !isChecked;
                
            }
        }]
});