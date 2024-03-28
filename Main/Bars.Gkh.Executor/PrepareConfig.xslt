<?xml version="1.0"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  
  <xsl:output method="xml" indent="yes" />
  <xsl:strip-space elements="*"/>

  <xsl:template match="*">
    <xsl:copy>
      <xsl:copy-of select="./@*"/>
      <xsl:apply-templates />
    </xsl:copy>
  </xsl:template>
  
  <xsl:template match="asm:assemblyBinding" xmlns:asm="urn:schemas-microsoft-com:asm.v1">
    <xsl:copy-of select="//tmpAssemblyBinding/*" />
  </xsl:template>
  
  <xsl:template match="tmpAssemblyBinding" />
  
</xsl:stylesheet>