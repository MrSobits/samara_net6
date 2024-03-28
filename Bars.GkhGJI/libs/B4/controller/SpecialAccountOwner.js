Ext.define('B4.controller.SpecialAccountOwner', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.enums.YesNo'
    ],

    realityObjectId: null,
    ownerProtocol: null,

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        //controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    stores: [
        'specaccowner.SpecialAccountOwner',
        'specaccowner.SPAccOwnerRealityObject'
    ],

    models: [
        'specaccowner.SpecialAccountOwner',
        'specaccowner.SPAccOwnerRealityObject'
    ],

    views: [
        'specaccowner.SpecialAccountOwnerGrid',
        'specaccowner.SpecialAccountOwnerEditWindow',
        'specaccowner.SPAccOwnerRealityObjectGrid',
        'specaccowner.SPAccOwnerRealityObjectEditWindow'
    ],

    mainView: 'specaccowner.SpecialAccountOwnerGrid',
    mainViewSelector: 'specaccownergrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'specaccownergrid'
        },
        {
            ref: 'specialAccountOwnerEditWindow',
            selector: 'specialAccountOwnerEditWindow'
        },
        {
            ref: 'sPAccOwnerRealityObjectGrid',
            selector: 'specaccownerrobjectgrid'
        }
    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'specialAccountOwnerGridWindowAspect',
            gridSelector: 'specaccownergrid',
            editFormSelector: '#specialAccountOwnerEditWindow',
            modelName: 'specaccowner.SpecialAccountOwner',
            editWindowView: 'specaccowner.SpecialAccountOwnerEditWindow',
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var me = this;
                    realityObjectId = rec.getId();
                    var grid = form.down('specaccownerrobjectgrid'),
                        store = grid.getStore();
                    store.filter('parentId', rec.getId());
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'sPAccOwnerRealityObjectGridWindowAspect',
            gridSelector: 'specaccownerrobjectgrid',
            editFormSelector: '#specialAccountOwnerRealityObjectEditWindow',
            modelName: 'specaccowner.SPAccOwnerRealityObject',
            storeName: 'specaccowner.SPAccOwnerRealityObject',
            editWindowView: 'specaccowner.SPAccOwnerRealityObjectEditWindow',
            //onSaveSuccess: function () {
            //    // перекрываем чтобы окно незакрывалось после сохранения
            //    B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            //},
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        debugger;
                        record.set('SpecialAccountOwner', realityObjectId);
                    }
                }
            }
        },

    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('specaccownergrid');
        me.bindContext(view);
        this.application.deployView(view);
        //me.getAspect('manOrgLicenseNotificationGisEditPanelAspect').setData(id);
        this.getStore('specaccowner.SpecialAccountOwner').load();
    },

    init: function () {
        var me = this,
            actions = {};
        me.callParent(arguments);

    },
    getGrid: function () {
        return Ext.ComponentQuery.query('specaccownerrobjectgrid')[0];
    },
});