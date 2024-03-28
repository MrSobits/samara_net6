Ext.define('B4.model.administration.fsTownImportSettingsSubData', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FsGorodMapItem'
    },
    fields: [
        { name: 'ImportInfo' },
        { name: 'IsMeta' },
        { name: 'Index' },
        { name: 'PropertyName' },
        { name: 'Regex' },
        { name: 'GetValueFromRegex' },
        { name: 'RegexSuccessValue' },
        { name: 'ErrorText' },
        { name: 'UseFilename' },
        { name: 'Format' },
        { name: 'Required' },
        { name: 'PaymentAgent' }
    ]
});