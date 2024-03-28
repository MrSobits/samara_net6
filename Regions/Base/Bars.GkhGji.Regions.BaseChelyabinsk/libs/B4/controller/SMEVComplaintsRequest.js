Ext.define('B4.controller.SMEVComplaintsRequest', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    smevMVD: null,

    models: [
        'complaints.SMEVComplaintsRequest',
        'complaints.SMEVComplaintsRequestFile'
    ],
    stores: [
        'complaints.SMEVComplaintsRequest',
        'complaints.SMEVComplaintsRequestFile'
    ],
    views: [

        'complaintsrequest.Grid',
        'complaintsrequest.EditWindow',
        'complaintsrequest.FileInfoGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'complaintsrequestGridAspect',
            gridSelector: 'complaintsrequestgrid',
            editFormSelector: '#complaintsrequestEditWindow',
            storeName: 'complaints.SMEVComplaintsRequest',
            modelName: 'complaints.SMEVComplaintsRequest',
            editWindowView: 'complaintsrequest.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevMVD = record.getId();
                    var me = this;
                    var grid = form.down('complaintsrequestfileinfogrid'),
                        store = grid.getStore();
                    store.filter('requestId', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
    ],

    mainView: 'complaintsrequest.Grid',
    mainViewSelector: 'complaintsrequestgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'complaintsrequestgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('complaintsrequestgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('complaints.SMEVComplaintsRequest').load();
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('complaintsrequestGridAspect').editRecord(this.params);
            this.params = null;
        }
    },
});