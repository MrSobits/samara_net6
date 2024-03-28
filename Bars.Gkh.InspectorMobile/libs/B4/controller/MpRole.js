Ext.define('B4.controller.MpRole', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    models: ['MpRole'],
    stores: [
        'MpRole',
        'dict.RoleForSelect',
        'MpRoleForSelected'
    ],
    views: ['MpRoleGrid'],

    mainView: 'MpRoleGrid',
    mainViewSelector: 'mpRoleGrid',

    refs: [{
        ref: 'mainView',
        selector: 'mpRoleGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'mpRoleMultiSelectWindowAspect',
            fieldSelector: '#selectedRoles',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#mpRoleSelectWindow',
            storeSelect: 'dict.RoleForSelect',
            storeSelected: 'MpRoleForSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор ролей',
            titleGridSelect: 'Роли для отбора',
            titleGridSelected: 'Выбранные роли',
            triggerOpenForm: function () {
                var me = this;

                me.getForm().show();
                me.updateSelectedGrid();
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) { recordIds.push(rec.get('Id')); });
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('AddRoles', 'MpRole'),
                        params: {
                            roleIds: Ext.encode(recordIds),
                            qualMemberId: asp.controller.qualMemberId
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getForm().hide();
                        asp.controller.getMainView().getStore().load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        }
    ],

    init: function () {
        var actions = {};

        actions['mpRoleGrid button[name="addRole"]'] = { click: { fn: this.openRoleSelectForm, scope: this } };
        actions['mpRoleGrid'] = { 'rowaction': { fn: this.rowAction, scope: this } };

        this.control(actions);
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('mpRoleGrid');
        this.bindContext(view);
        this.getStore('MpRole').load();
    },

    openRoleSelectForm: function () {
        this.getAspect('mpRoleMultiSelectWindowAspect').triggerOpenForm();
    },

    rowAction: function (grid, action, record) {
        if (!grid || grid.isDestroyed) return;
        if (this.fireEvent('beforerowaction', this, grid, action, record) !== false && action.toLowerCase() == 'delete') {
            var me = this;

            Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                if (result == 'yes') {
                    me.mask('Удаление', B4.getBody());
                    record.destroy()
                        .next(function () {
                            me.unmask();
                            me.getMainView().getStore().load();
                        }, this)
                        .error(function (result) {
                            Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            me.unmask();
                        }, this);
                }
            }, me);
        }
    }
});