:: heat можно найти в папке \Wix\bin, по-умолчанию Wix ставится C:\Program Files (x86)\WiX Toolset v3.9\
:: "Sources" - путь к папке с необходимыми компонентами для установки - dll, ресурсы, папки. Такая же структура будет установлена установщиком.
:: var.SourcePath объявлен в переменной Variables.wxi
:: ProductComponents.wxs и ProductTransformer.xslt лежат в папке проекта установщика
heat dir "AppSources" -gg -g1 -sfrag -sreg -cg ProductComponents -var var.AppSourcePath -srd -dr INSTALLLOCATION -out "ProductComponents.wxs" -t "ProductTransformer.xslt"

:: Получение компонентов для фичи "Сервер расчетов"
heat dir "CalcSources" -gg -g1 -sfrag -sreg -cg ProductCalcComponents -var var.CalcSourcePath -srd -dr CALCSERVERFOLDER -out "ProductComponents.wxs" -t "ProductTransformer.xslt"