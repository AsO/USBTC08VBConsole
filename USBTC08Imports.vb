'/**************************************************************************
'*
'* Filename:    TC08Imports.cs
'*
'* Copyright:   Pico Technology Limited 2011
'*
'* Author:      CPY
'*
'* Description:
'*   This file contains all the .NET wrapper calls needed to support
'*   the console example. It also has the enums and structs required
'*   by the (wrapped) function calls.
'*
'* History:
'*    23/05/2011 	CPY	Created
'*
'* Revision Info: "file %n date %f revision %v"
'*						""
'*
'***************************************************************************/

Imports System
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO

Namespace picotech
    Public Class USBTC08API

#Region "Constants"
        Private Const _DRIVER_FILENAME As String = "usbtc08.dll"
        Public Const MAX_INFO_CHARS As Short = 256
        Public ReadOnly TC_Type As String() = {"X", "B", "E", "J", "K", "N", "R", "S", "T"}
        Public Const MAX_CHANNELS As Integer = 8
        Public Const TC_TYPE_K As Char = "K"
        Public Const PICO_OK As Integer = 1
#End Region

#Region "Driver enums"
        Public Enum TempUnit As Short
            CENTIGRADE
            FAHRENHEIT
            KELVIN
            RANKINE
        End Enum

#End Region

#Region "Structs"
        Public Structure UnitInfo
            Public size As Short
            Public DriverVersion As Short
            Public PicoppVersion As Short
            Public HardwareVersion As Short
            Public picoVar As Short
            Public szSerial As String
            Public szCalDate As String
        End Structure
#End Region

#Region "Driver Imports"
#Region "Callback delegates"

#End Region

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_open_unit")>
        Public Shared Function OpenUnit() As Short
            '
        End Function

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_close_unit")>
        Public Shared Function CloseUnit( _
            ByVal handle As Short) As Short
            '
        End Function

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_run")>
        Public Shared Function Run( _
            ByVal handle As Short, _
            interval As Integer) As Short
            '
        End Function

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_stop")>
        Public Shared Function StopUnit( _
            ByVal handle As Short) As Short
            '
        End Function

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_get_last_error")>
        Public Shared Function GetLastError( _
            ByVal handle As Short) As Short
            '
        End Function

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_get_unit_info")>
        Public Shared Function GetUnitInfo( _
            ByVal handle As Short, _
            ByRef unit_info As UnitInfo) As Short
            '
        End Function

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_get_formatted_info")>
        Public Shared Function GetFormattedInfo( _
            ByVal handle As Short, _
            unit_info As StringBuilder, _
            string_length As Short) As Short
            '
        End Function

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_set_channel")>
        Public Shared Function SetChannel( _
            ByVal handle As Short, _
            ByVal channel As Short, _
            ByVal tc_type As Char) As Short
            '
        End Function

        <DllImport(_DRIVER_FILENAME, EntryPoint:="usb_tc08_get_single")>
        Public Shared Function GetSingle( _
            ByVal handle As Short, _
            ByVal temp As Single(), _
            ByRef overflow_flags As Short, _
            ByVal units As TempUnit) As Short
        End Function

#End Region

    End Class
End Namespace
