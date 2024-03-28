Ext.define('B4.view.actcheck.MenuButton', {
    extend: 'Ext.button.Button',
    alias: 'widget.actcheckmenubutton',
    
    iconCls: 'icon-accept',
    itemId: 'btnCreateDocument',
    text: 'Сформировать',
    menu: [
        {
            text: 'Предписание',
            textAlign: 'left',
            itemId: 'btnCreateActCheckToPrescription',
            actionName: 'createActCheckToPrescription'
        },
        {
            text: 'Протокол',
            textAlign: 'left',
            itemId: 'btnCreateActCheckToProtocol',
            actionName: 'createActCheckToProtocol'
        }
    ]
});