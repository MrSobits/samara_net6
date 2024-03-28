Ext.define('B4.controller.AdminCase', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['AdminCase'],
    stores: ['AdminCase'],
    views: [
        'admincase.AddWindow',
        'admincase.Grid'
    ],

    mainView: 'admincase.Grid',
    mainViewSelector: 'admincasegrid',

    refs: [
        {
            ref: 'mainView',
            selector: '#adminCaseGrid'
        }
    ],

    aspects: [
        {
            /*Вешаем аспект смены статуса в гриде*/
            xtype: 'b4_state_contextmenu',
            name: 'adminCaseStateTransferAspect',
            gridSelector: 'admincasegrid',
            stateType: 'gji_document_admincase',
            menuSelector: 'adminCaseGridStateMenu'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'adminCaseGridWindowAspect',
            gridSelector: '#adminCaseGrid',
            editFormSelector: '#adminCaseAddWindow',
            storeName: 'AdminCase',
            modelName: 'AdminCase',
            editWindowView: 'admincase.AddWindow',
            controllerEditName: 'B4.controller.admincase.Navigation',
            onSaveSuccess: function (aspect, rec) {
                //Закрываем окно после добавления новой записи
                aspect.getForm().close();
                
                //загружаем добавленный объект
                var model = this.controller.getModel(this.modelName);

                model.load(rec.getId(), {
                    success: function (record) {
                        //После загрузки объекта подменяем параметр и открываем вкладку
                        this.editRecord(record);
                    },
                    scope: this
                });
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView();
        
        if (!view) {
            view = Ext.widget('admincasegrid');

            me.bindContext(view);
            me.application.deployView(view);

            me.getStore('AdminCase').load();
        }
    }
});