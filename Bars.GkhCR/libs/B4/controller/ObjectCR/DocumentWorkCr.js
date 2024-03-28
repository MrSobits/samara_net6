Ext.define('B4.controller.objectcr.DocumentWorkCr', {
    /*
    * Контроллер раздела документов
    */
    extend: 'B4.controller.MenuItemController',
    requires:
    [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.objectcr.Document'
    ],

    models: ['objectcr.DocumentWorkCr', 'objectcr.MonitoringSmr'],
    stores: ['objectcr.DocumentWorkCr'],
    views:
    [
        'objectcr.DocumentWorkCrEditWindow',
        'objectcr.DocumentWorkCrGrid'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'objectcr.DocumentWorkCrGrid',
    mainViewSelector: 'objectcrdocumentgrid',

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        {
            xtype: 'documentobjectcrperm',
            name: 'documentObjectCrPerm',
            editFormAspectName: 'documentWorkCrGridAspect'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования документов
            */
        xtype: 'grideditctxwindowaspect',
        name: 'documentWorkCrGridAspect',
        modelName: 'objectcr.DocumentWorkCr',
        gridSelector: 'objectcrdocumentgrid',
        editFormSelector: '#documentWorkCrEditWindow',
        editWindowView: 'objectcr.DocumentWorkCrEditWindow',
        listeners: {
            getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.ObjectCr = this.controller.getContextValue(this.controller.getMainView(), 'objectcrId');
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var contragentField = form.down('[name=Contragent]');

                    if (contragentField) {
                        contragentField.store.on('beforeload', asp.onBeforeContragentsLoad, asp);
                    }
                }
            },

            onBeforeContragentsLoad: function (store, operation) {
                operation.params.showAll = true;
            }
        }],

    init: function () {
        var actions = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            store,
            view = me.getMainView() || Ext.widget('objectcrdocumentgrid');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        store = view.getStore();
        store.load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectCrId = me.getContextValue(me.getMainComponent(), 'objectcrId');
    },

    onMainViewAfterRender: function (view) {

        var aspect = this.getAspect('documentObjectCrPerm'),
            objectCr = this.getContextValue(this.getMainView(), 'objectcrId');

        view.getStore().on('beforeload', this.onBeforeLoad, this);

        this.mask('Загрузка', this.getMainComponent());
        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'MonitoringSmr', {
            objectCrId: objectCr
        })).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText);
            var model = this.getModel('objectcr.MonitoringSmr');

            model.load(obj.MonitoringSmrId, {
                success: function (rec) {
                    aspect.setPermissionsByRecord(rec);
                },
                scope: this
            });
            this.unmask();
            return true;
        }, this).error(function () {
            this.unmask();
            Ext.Msg.alert('Сообщение', 'Произошла ошибка');
        }, this);

    }
});