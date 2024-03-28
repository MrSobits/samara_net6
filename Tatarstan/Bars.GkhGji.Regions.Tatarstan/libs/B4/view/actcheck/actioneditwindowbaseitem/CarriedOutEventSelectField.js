Ext.define('B4.view.actcheck.actioneditwindowbaseitem.CarriedOutEventSelectField', {
    extend: 'B4.form.SelectField',

    requires: [
        'B4.enums.ActCheckActionCarriedOutEventType'
    ],

    alias: 'widget.actcheckactioncarriedouteventselectfield',
    itemId: 'actCheckActionCarriedOutEventSelectField',

    store: B4.enums.ActCheckActionCarriedOutEventType.getStore(),
    textProperty: 'Display',
    selectionMode: 'MULTI',
    editable: false,
    modalWindow: true,

    columns: [
        {
            text: 'Наименование',
            dataIndex: 'Display',
            flex: 1,
            filter: { xtype: 'textfield' }
        }
    ]
});