Ext.define('B4.controller.dict.MeteringDevice', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.MeteringDevice'
    ],

    models: ['dict.MeteringDevice'],
    stores: ['dict.MeteringDevice'],
    views: [
        'dict.meteringdevice.Grid',
        'dict.meteringdevice.EditWindow'
    ],

    mainView: 'dict.meteringdevice.Grid',
    mainViewSelector: 'meteringDeviceGrid',

    mixins: {
            context: 'B4.mixins.Context'
        },

    refs: [
        {
            ref: 'mainView',
            selector: 'meteringDeviceGrid'
        }
    ],

    aspects: [
        {
            xtype: 'meteringdevicedictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'meteringDeviceGridWindowAspect',
            gridSelector: 'meteringDeviceGrid',
            editFormSelector: '#meteringDeviceEditWindow',
            storeName: 'dict.MeteringDevice',
            modelName: 'dict.MeteringDevice',
            editWindowView: 'dict.meteringdevice.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('meteringDeviceGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.MeteringDevice').load();
    }
});