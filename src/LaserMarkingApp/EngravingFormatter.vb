Imports System
Imports System.Globalization

Public NotInheritable Class EngravingFormatter
    Private Sub New()
    End Sub

    Public Shared Function Build(part As PartRecord, serialNumber As Integer, heatLotNumber As String, markDate As DateTime) As String
        If part Is Nothing Then
            Throw New ArgumentNullException(NameOf(part))
        End If

        If serialNumber <= 0 Then
            Throw New ArgumentOutOfRangeException(NameOf(serialNumber), "Serial number must be positive.")
        End If

        Dim heatLot = CleanSegment(heatLotNumber)
        If String.IsNullOrWhiteSpace(heatLot) Then
            Throw New ArgumentException("Heat / lot number is required.", NameOf(heatLotNumber))
        End If

        Dim segments = {
            CleanSegment(part.CustomerItemCode),
            CleanSegment(part.PartNumber),
            $"{BuildDatePrefix(markDate)}{serialNumber}",
            markDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            markDate.ToString("MMM-yy", CultureInfo.InvariantCulture).ToUpperInvariant(),
            heatLot,
            CleanSegment(part.Material),
            CleanSegment(part.Pattern),
            CleanSegment(part.ProductName),
            CleanSegment(part.SupplierName)
        }

        Return String.Join("$", segments) & "$"
    End Function

    Public Shared Function BuildDatePrefix(markDate As DateTime) As String
        Dim monthCode = ChrW(AscW("A"c) + markDate.Month - 1)
        Return $"{markDate:yy}{monthCode}-"
    End Function

    Public Shared Function CleanSegment(value As String) As String
        If value Is Nothing Then
            Return ""
        End If

        Dim cleaned = value.Trim()
        cleaned = cleaned.Replace("$", "")
        cleaned = cleaned.Replace(vbCr, "")
        cleaned = cleaned.Replace(vbLf, "")
        Return cleaned.Trim()
    End Function
End Class
