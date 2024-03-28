Ext.define('B4.view.realityobj.AddAvatarImage', {
    extend: 'B4.form.Window',

    alias: 'widget.addavatarimagewin',
        
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 500,
    minWidth: 500,
    bodyPadding: 5,
    title: 'Выберите файл',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    store: 'realityobj.Image',
        
    requires: [
        'B4.store.dict.Period',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.enums.ImagesGroup'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    labelAlign: 'right',
                    anchor: '100%',
                    regex: /(.((i|I)(m|M)(g|G))|((j|J)(p|P)(g|G))|((j|J)(p|P)(e|E)(g|G))|((p|P)(n|N)(g|G))|((g|G)(i|I)(f|F)))$/,
                    regexText: 'Принимаются файлы с расширением .img, .jpg, .png, .gif'
                },
                {
                    xtype: 'hiddenfield',
                    name: 'ImagesGroup',
                    value: 50
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});