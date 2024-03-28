Ext.define('B4.controller.EmergencyObj', {
    /*
    * Контроллер раздела аварийных домов
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context:'B4.mixins.Context'
    },
    
    models: ['EmergencyObject'],
    stores: ['EmergencyObject'],
    views: [
        'emergencyobj.Grid',
        'emergencyobj.AddWindow'
    ],

    mainView: 'emergencyobj.Grid',
    mainViewSelector: 'emergencyObjGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'emergencyObjGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.EmergencyObject.Create', applyTo: 'b4addbutton', selector: 'emergencyObjGrid' },
                { name: 'Gkh.EmergencyObject.Edit', applyTo: 'b4savebutton', selector: '#emergencyObjEditPanel' },
                { name: 'Gkh.EmergencyObject.Delete', applyTo: 'b4deletecolumn',  selector: 'emergencyObjGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'emergencyObjectStateTransferAspect',
            gridSelector: 'emergencyObjGrid',
            menuSelector: 'emergencyObjGridStateMenu',
            stateType: 'gkh_emergency_object'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'emergencyObjectButtonExportAspect',
            gridSelector: 'emergencyObjGrid',
            buttonSelector: 'emergencyObjGrid #btnExport',
            controllerName: 'EmergencyObject',
            actionName: 'Export'
        },
        {
            /*
            аспект взаимодействия таблицы аварийных домов и формы добавления
            */
            xtype: 'gkhgrideditformaspect',
            name: 'emergencyObjGridWindowAspect',
            gridSelector: 'emergencyObjGrid',
            editFormSelector: '#emergencyObjAddWindow',
            storeName: 'EmergencyObject',
            modelName: 'EmergencyObject',
            editWindowView: 'emergencyobj.AddWindow',
            controllerEditName: 'B4.controller.emergencyobj.Navigation',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' b4selectfield'] = { 'beforeload': { fn: this.beforeLoad, scope: this } };
            },
            beforeLoad: function(store, operation) {
                operation.params = {};
                operation.params.onlyEmergency = true;
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('emergencyObjGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('EmergencyObject').load();
        
        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);
            if (json.ShowStlRealityGrid) {

            } else {
                var col = Ext.ComponentQuery.query('emergencyObjGrid #SettlementColumn')[0];
                if (col) {
                    col.hide();
                }
            }
        }).error(function () {
            Log('Ошибка получения параметров приложения');
        });
    }
});