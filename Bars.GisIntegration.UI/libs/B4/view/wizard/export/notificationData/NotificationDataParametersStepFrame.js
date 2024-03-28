Ext.define('B4.view.wizard.export.notificationData.NotificationDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.store.integrations.houseManagement.House',
        'B4.model.integrations.houseManagement.House',
        'B4.ux.grid.filter.YesNo'
    ],
    layout: 'anchor',
    mixins: ['B4.mixins.window.ModalMask'],
    items: [
        {
            xtype: 'b4selectfield',
            name: 'Notification',
            anchor: '100%',
            margin: 5,
            modalWindow: true,
            textProperty: 'NotifTopic',
            selectionMode: 'MULTI',
            fieldLabel: 'Новости',
            store: 'B4.store.integrations.houseManagement.Notification',
            model: 'B4.model.integrations.houseManagement.Notification',
            columns: [
                {
                    text: 'Заголовок',
                    dataIndex: 'NotifTopic',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Текст',
                    dataIndex: 'NotifContent',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Адрес дома',
                    dataIndex: 'Address',
                    flex: 3,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Важность',
                    dataIndex: 'IsImportant',
                    width: 60,
                    filter: { xtype: 'b4dgridfilteryesno' },
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата начала',
                    dataIndex: 'NotifFrom',
                    format: 'd.m.Y',
                    width: 75,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата окончания',
                    dataIndex: 'NotifTo',
                    format: 'd.m.Y',
                    width: 75,
                    filter: { xtype: 'textfield' }
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('b4selectfield[name=Notification]').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('b4selectfield[name=Notification]');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedNotifications: multiSuppliers ? 'ALL' : me.wizard.down('b4selectfield[name=Notification]').getValue()
        };
    }
});