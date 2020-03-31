﻿

Public Class frmMain

    Private driver As ASCOM.DriverAccess.Telescope

    Dim TargetRA As Double, TargetDec As Double

    ''' <summary>
    ''' This event is where the driver is choosen. The device ID will be saved in the settings.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub buttonChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonChoose.Click
        My.Settings.DriverId = ASCOM.DriverAccess.Telescope.Choose(My.Settings.DriverId)
        SetUIState()
    End Sub

    ''' <summary>
    ''' Connects to the device to be tested.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub buttonConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonConnect.Click
        If (IsConnected) Then
            driver.Connected = False
            Timer1.Enabled = False
        Else
            driver = New ASCOM.DriverAccess.Telescope(My.Settings.DriverId)
            driver.Connected = True
            updateValues()

            nud_Decd.Value = CInt(txtMountDec.Text.Substring(0, 2))
            nud_Decm.Value = CInt(txtMountDec.Text.Substring(4, 2))
            nud_Decs.Value = CInt(txtMountDec.Text.Substring(8, 2))

            nud_RAh.Value = CInt(txtMountRA.Text.Substring(0, 2))
            nud_RAm.Value = CInt(txtMountRA.Text.Substring(4, 2))
            nud_RAs.Value = CInt(txtMountRA.Text.Substring(8, 2))

            lblVersion.Text = driver.Action("Telescope:getFirmwareVer","")

            'Timer1.Enabled = True
        End If
        SetUIState()

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If IsConnected Then
            driver.Connected = False
        End If
        ' the settings are saved automatically when this application is closed.
    End Sub

    ''' <summary>
    ''' Sets the state of the UI depending on the device state
    ''' </summary>
    Private Sub SetUIState()
        buttonConnect.Enabled = Not String.IsNullOrEmpty(My.Settings.DriverId)
        buttonChoose.Enabled = Not IsConnected
        buttonConnect.Text = IIf(IsConnected, "Disconnect", "Connect")
    End Sub

    ''' <summary>
    ''' Gets a value indicating whether this instance is connected.
    ''' </summary>
    ''' <value>
    ''' 
    ''' <c>true</c> if this instance is connected; otherwise, <c>false</c>.
    ''' 
    ''' </value>
    Private ReadOnly Property IsConnected() As Boolean
        Get
            If Me.driver Is Nothing Then Return False
            Return driver.Connected
        End Get
    End Property

    ' TODO: Add additional UI and controls to test more of the driver being tested.

    Private Sub btnSlewSync_Click(sender As Object, e As EventArgs) Handles btnSlewSync.Click
        If IsConnected Then
            TargetRA = HMStoDbl(nud_RAh.Value.ToString + ":" + nud_RAm.Value.ToString + ":" + nud_RAs.Value.ToString)
            TargetDec = DMStoDbl(nud_Decd.Value.ToString + ":" + nud_Decm.Value.ToString + ":" + nud_Decs.Value.ToString)
            txtTargetRA.Text = nud_RAh.Value.ToString + "H " + nud_RAm.Value.ToString + "' " + nud_RAs.Value.ToString + "''"
            txtTargetDec.Text = nud_Decd.Value.ToString + "° " + nud_Decm.Value.ToString + "' " + nud_Decs.Value.ToString + "''"
            driver.SlewToCoordinates(TargetRA, TargetDec)

            updateValues()

            'nud_Decd.Value = CInt(txtMountDec.Text.Substring(0, 2))
            'nud_Decm.Value = CInt(txtMountDec.Text.Substring(4, 2))
            'nud_Decs.Value = CInt(txtMountDec.Text.Substring(8, 2))

            'nud_RAh.Value = CInt(txtMountRA.Text.Substring(0, 2))
            'nud_RAm.Value = CInt(txtMountRA.Text.Substring(4, 2))
            'nud_RAs.Value = CInt(txtMountRA.Text.Substring(8, 2))

        End If
    End Sub

    Private Sub btnSlewAsync_Click(sender As Object, e As EventArgs) Handles btnSlewAsync.Click

    End Sub
    Private Sub updateValues()
        txtMountDec.Text = DblToDMS(driver.Declination).ToString
        txtMountRA.Text = DbltoHMS(driver.RightAscension).ToString
        txtTargetDec.Text = DblToDMS(driver.TargetDeclination).ToString
        txtTargetRA.Text = DbltoHMS(driver.TargetRightAscension).ToString

    End Sub
    Private Function HMStoDbl(HMS As String) As Double
        Dim aryHMS() As String
        Dim dblVal As Double
        aryHMS = HMS.Split(":")
        dblVal = CDbl(aryHMS(0)) + (CDbl(aryHMS(1)) / 60.0) + (CDbl(aryHMS(2)) / 3600.0)
        Return dblVal
    End Function

    Private Function DMStoDbl(DMS As String) As Double
        Dim signVal As Integer = 1
        Dim aryDMS() As String
        Dim dblVal As Double
        aryDMS = DMS.Split(":")
        If CDbl(aryDMS(0)) < 0 Then signVal = -1
        dblVal = CDbl(aryDMS(0)) + (signVal * CDbl(aryDMS(1)) / 60.0) + (signVal * CDbl(aryDMS(2)) / 3600.0)
        Return dblVal
    End Function

    Private Function DblToDMS(dblDeg As Double) As String
        Dim Degrees As Double = Int(dblDeg)
        Dim Minutes As Double = (dblDeg - Degrees) * 60.0
        Dim Seconds As Double = (Minutes - Int(Minutes)) * 60
        Dim strDMS As String = Degrees.ToString("00") + "° " + Int(Minutes).ToString("00") + "' " + Int(Seconds).ToString("00") + "''"
        Return strDMS
    End Function

    Private Function DbltoHMS(dblHMS As Double) As String
        Dim Hours As Double = Int(dblHMS)
        Dim Minutes As Double = (dblHMS - Hours) * 60.0
        Dim Seconds As Double = (Minutes - Int(Minutes)) * 60
        Dim strHMS As String = Hours.ToString("00") + "h " + Int(Minutes).ToString("00") + "' " + Int(Seconds).ToString("00") + "''"
        Return strHMS
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        updateValues()
    End Sub

    Private Sub btnPark_Click(sender As Object, e As EventArgs) Handles btnPark.Click
        If btnPark.Text = "Park" Then
            driver.Park()
            btnPark.Text = "Unpark"
            btnSlewSync.Enabled = False
            btnHalt.Enabled = False
        Else
            driver.Unpark()
            btnPark.Text = "Park"
            btnSlewSync.Enabled = True
            btnHalt.Enabled = True
        End If
    End Sub
End Class