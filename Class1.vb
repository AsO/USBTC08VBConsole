Imports System
Imports System.Threading
Imports System.Runtime.InteropServices
Imports USBTC08VBConsole.picotech.USBTC08API

Public Class Win32Interop
    <DllImport("crtdll.dll")>
    Public Shared Function _kbhit() As Integer
        '
    End Function
End Class

Namespace picotech.USBTC08
    Public Class USBTC08Console
        Private ReadOnly _handle As Short

        Public info As USBTC08API.UnitInfo
        Shared tempReadings(8) As Single

        Private Sub openUSBTC08(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim handle As Short = USBTC08API.OpenUnit()
            Console.Write(handle)
            'info.size = Marshal.SizeOf(info)
            Dim infook As Short = USBTC08API.GetUnitInfo(handle, info)
            Console.Write(infook)
        End Sub

        Private Sub getUSBTC08()
            Dim unitResult As Long
            Dim handle As Integer = 1
            Dim ofFlag As Integer
            Dim tempUnits As Integer = 1

            For n = 0 To 8 - 1
                unitResult = USBTC08API.SetChannel(handle, n, "K")
                Debug.Print(unitResult)
            Next

            unitResult = USBTC08API.GetSingle(handle, tempReadings, ofFlag, tempUnits)
            Debug.Print(unitResult & " " & ofFlag)

            For n = 0 To 8 - 1
                Debug.Print(tempReadings(n))
            Next

        End Sub

        '/****************************************************************************
        ' * Read the device information
        ' ****************************************************************************/
        Private Sub GetDeviceInfo()
            Dim line As System.Text.StringBuilder = New System.Text.StringBuilder(USBTC08API.MAX_INFO_CHARS)

            If _handle >= 0 Then
                USBTC08API.GetFormattedInfo(_handle, line, USBTC08API.MAX_INFO_CHARS)
                Console.WriteLine("{0}", line)
            End If
        End Sub

        '/****************************************************************************
        ' * Read temperature information from the unit
        ' ****************************************************************************/
        Private Sub GetValues()
            Dim status As Short
            Dim tempbuffer() As Single = New Single(8) {}
            Dim overflow As Short
            Dim lines As Integer = 0

            Console.WriteLine()
            '// label the columns
            For chan As Short = 1 To USBTC08API.MAX_CHANNELS
                Console.Write("Chan{0}:    ", chan)
            Next

            Console.WriteLine()

            Do
                status = USBTC08API.GetSingle(_handle, tempbuffer, overflow, USBTC08API.TempUnit.CENTIGRADE)

                If status = PICO_OK Then
                    For chan As Short = 1 To USBTC08API.MAX_CHANNELS
                        Console.Write("{0:0.0000}   ", tempbuffer(chan))
                    Next
                    Console.WriteLine()
                    Thread.Sleep(1000)
                Else
                    Dim err As Short = USBTC08API.GetLastError(_handle)
                    Console.Write("GetValues:ERR CODE:{0}", err)
                End If

                lines += 1
                If lines > 9 Then
                    Console.WriteLine("Hit any key to stop....")
                    Console.WriteLine("Cold Junction Temperature: {0:0.0000}   ", tempbuffer(0))

                    lines = 0
                    For chan As Short = 1 To USBTC08API.MAX_CHANNELS
                        Console.Write("Chan{0}:    ", chan)
                    Next
                    Console.WriteLine()
                End If
            Loop While Win32Interop._kbhit() = 0

            Dim ch As Char = (Console.ReadKey().KeyChar)       ' // use up keypress
            status = USBTC08API.StopUnit(_handle)
        End Sub

        '/****************************************************************************
        ' *  Set channels 
        ' ****************************************************************************/
        Private Sub SetChannels()
            Dim status As Short

            For channel As Short = 0 To USBTC08API.MAX_CHANNELS
                status = USBTC08API.SetChannel(_handle, channel, USBTC08API.TC_TYPE_K)
                If status <> PICO_OK Then
                    Dim err As Short = USBTC08API.GetLastError(_handle)
                    Console.Write("SetChannels:ERR CODE:{0}", err)
                End If
            Next
        End Sub

        '/****************************************************************************
        '*  Run
        '****************************************************************************/
        Public Sub Run()
            ' //// main loop - read key and call routine
            Dim ch As Char = " "
            While ch <> "X"
                Console.WriteLine()
                Console.WriteLine("I - Device Info")
                Console.WriteLine("G - Get Temperatures")
                Console.WriteLine("X - exit")
                Console.WriteLine("Operation:")

                ch = Char.ToUpper(Console.ReadKey(True).KeyChar)

                Console.WriteLine()

                Select Case ch
                    Case "I"
                        GetDeviceInfo()

                    Case "G"
                        SetChannels()
                        GetValues()

                    Case "X"
                        '/* Handled by outer loop */

                    Case Else
                        Console.WriteLine("Invalid operation")
                End Select
            End While
        End Sub

        Public Sub New(handle As Short)
            _handle = handle
        End Sub

    End Class

    Module Module1
        '/****************************************************************************
        '*  WaitForKey
        '*  Wait for user's keypress
        '****************************************************************************/
        Public Sub WaitForKey()
            While (Not Console.KeyAvailable)
                Thread.Sleep(100)
            End While

            If Console.KeyAvailable Then
                Console.ReadKey(True) '// clear the key 
            End If
        End Sub

        Public Sub Main()
            Console.WriteLine("TC08 driver example program")
            Console.WriteLine("Version 1.0")
            Console.WriteLine()
            '//open unit and show splash screen
            Console.WriteLine("Opening the device...")

            Dim handle As Short = USBTC08API.OpenUnit()
            Console.WriteLine("Handle: {0}", handle)
            If handle = 0 Then
                Console.WriteLine("Unable to open device")
                Console.WriteLine("Error code : {0}", handle)
                WaitForKey()
            Else
                Console.WriteLine("Device opened successfully")
                Console.WriteLine()

                Dim consoleExample As USBTC08Console = New USBTC08Console(handle)
                consoleExample.Run()

                USBTC08API.CloseUnit(handle)
            End If

        End Sub

    End Module
End Namespace
