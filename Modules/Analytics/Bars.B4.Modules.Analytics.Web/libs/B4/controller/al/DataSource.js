Ext.define('B4.controller.al.DataSource', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.mixins.Context',
        'B4.enums.al.OwnerType'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'al.DataSourceGrid',
        'al.DataSourceEdit'
    ],

    models: [
        'al.DataSource',
        'al.FilterExpression'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'datasourcegrid'
        },
        {
            ref: 'providerCombo',
            selector: 'b4combobox[name=ProviderKey]'
        },
        {
            ref: 'parentField',
            selector: 'b4selectfield[name=Parent]'
        },
        {
            ref: 'idField',
            selector: 'hidden[name=Id]'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'datasourcegrid',
            editFormSelector: 'datasourceedit',
            modelName: 'al.DataSource',
            editWindowView: 'al.DataSourceEdit',
            listeners: {
                aftersetformdata: function(asp, rec) {
                    var cntrl = asp.controller;
                    cntrl.getParentField().setVisible(rec.get('OwnerType') === B4.enums.al.OwnerType.User);
                }
            },
            updateGrid: function () {
                this.controller.loadTreePanelData(this.controller.getMainView());
            }
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('datasourcegrid');
        me.bindContext(view);
        me.application.deployView(view);
    },

    init: function() {
        var me = this;
        me.control({
            'b4selectfield[name=Parent]': {
                change: me.onParentChange
            },
            'datasourcegrid': {
                show: me.loadTreePanelData
            }
        });
        me.callParent(arguments);
    },

    onParentChange: function(field, newVal, oldVal) {
        var me = this, record;
        if (newVal && newVal.Id == me.getIdField().getValue()) {
            Ext.MessageBox.alert('Ошибка', 'Источник данных не может быть своим родителем.');
            if (oldVal) {
                record = field.getStore().findRecord('Id', oldVal);
                field.setValue(record.raw);
            }
        }
    },

    loadTreePanelData: function (treepanel) {
        var me = this;
        me.mask('Загрузка...', treepanel);
        B4.Ajax.request({
            url: B4.Url.action('GetTree', 'DataSourceTree'),
            params: {
                checkable: false
            }
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText),
                rootNode = treepanel.getRootNode();
            me.unmask(treepanel);
            rootNode.removeAll();
            rootNode.appendChild(json.data);
        }).error(function() {
            me.unmask(treepanel);
        });
    }
});