:: heat ����� ����� � ����� \Wix\bin, ��-��������� Wix �������� C:\Program Files (x86)\WiX Toolset v3.9\
:: "Sources" - ���� � ����� � ������������ ������������ ��� ��������� - dll, �������, �����. ����� �� ��������� ����� ����������� ������������.
:: var.SourcePath �������� � ���������� Variables.wxi
:: ProductComponents.wxs � ProductTransformer.xslt ����� � ����� ������� �����������
heat dir "AppSources" -gg -g1 -sfrag -sreg -cg ProductComponents -var var.AppSourcePath -srd -dr INSTALLLOCATION -out "ProductComponents.wxs" -t "ProductTransformer.xslt"

:: ��������� ����������� ��� ���� "������ ��������"
heat dir "CalcSources" -gg -g1 -sfrag -sreg -cg ProductCalcComponents -var var.CalcSourcePath -srd -dr CALCSERVERFOLDER -out "ProductComponents.wxs" -t "ProductTransformer.xslt"