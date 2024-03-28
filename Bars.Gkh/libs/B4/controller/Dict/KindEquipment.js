Ext.define('B4.controller.dict.KindEquipment', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.KindEquipment'
    ],

    models: ['dict.KindEquipment'],
    stores: ['dict.KindEquipment'],
    views: [
        'dict.kindequipment.Grid',
        'dict.kindequipment.EditWindow'
    ],

    mainView: 'dict.kindequipment.Grid',
    mainViewSelector: 'kindEquipmentGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'kindEquipmentGrid'
        }
    ],

    aspects: [
        {
            xtype: 'kindequipmentdictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'kindEquipmentGridWindowAspect',
            gridSelector: 'kindEquipmentGrid',
            editFormSelector: '#kindEquipmentEditWindow',
            storeName: 'dict.KindEquipment',
            modelName: 'dict.KindEquipment',
            editWindowView: 'dict.kindequipment.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('kindEquipmentGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.KindEquipment').load();
    }
});