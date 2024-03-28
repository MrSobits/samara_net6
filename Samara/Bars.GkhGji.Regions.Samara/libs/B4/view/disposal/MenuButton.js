Ext.define('B4.view.disposal.MenuButton', {
    extend: 'Ext.button.Button',

    alias: 'widget.disposalmenubutton',
    text: 'Сформировать',
    itemId: 'btnCreateDocument',
    iconCls: 'icon-accept',
    menu: [
        {
            text: 'Акт проверки (Общий)',
            textAlign: 'left',
            itemId: 'btnCreateDisposalToActCheck',
            actionName: 'createDisposalToActCheck'
        },
        {
            text: 'Акт проверки (на 1 дом)',
            textAlign: 'left',
            itemId: 'btnCreateDisposalToActCheck1House',
            actionName: 'createDisposalToActCheck1House'
        },
        {
            text: 'Акт проверки',
            textAlign: 'left',
            itemId: 'btnCreateDisposalToActCheckDocument'
        },
        {
            text: 'Акт обследования',
            textAlign: 'left',
            itemId: 'btnCreateDisposalToActSurvey',
            actionName: 'createDisposalToActSurvey'
        }
    ]
});